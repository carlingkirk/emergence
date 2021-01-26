using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Search.Models;
using Nest;

namespace Emergence.Service.Search
{
    public class SpecimenIndex : IIndex<Specimen, Data.Shared.Models.Specimen>
    {
        private readonly ISearchClient<Specimen> _searchClient;
        public string IndexName => "specimens_01";
        public string Alias => "specimens";
        public string NameTokenizer => "name_tokenizer";
        public string NameAnalyzer => "name_analyzer";

        public SpecimenIndex(ISearchClient<Specimen> searchClient)
        {
            _searchClient = searchClient;
            _searchClient.ConfigureClient(IndexName, Alias, GetClrMapping, GetMapping, GetSetting);
        }

        private ITypeMapping GetMapping(TypeMappingDescriptor<Specimen> mapping) =>
            mapping.AutoMap()
            .Properties(pi => pi
            .Text(t => t.Name(n => n.Name).Fields(f => f.Text(t => t.Name("nameSearch").Analyzer(NameAnalyzer)))));

        private IPromise<IndexSettings> GetSetting(IndexSettingsDescriptor setting) =>
            setting.Analysis(a => a
                .Analyzers(azs => azs.Custom(NameAnalyzer, c => c.Tokenizer(NameTokenizer).Filters("lowercase")))
                .Tokenizers(t => t.EdgeNGram(NameTokenizer, ng => ng.MinGram(3).MaxGram(20).TokenChars(new[] { TokenChar.Letter }))));

        private IClrTypeMapping<Specimen> GetClrMapping(ClrTypeMappingDescriptor<Specimen> mapping) =>
            mapping.IndexName(IndexName)
                .PropertyName(s => s.Id, "id")
                .PropertyName(s => s.Name, "name")
                .PropertyName(s => s.Quantity, "quantity")
                .PropertyName(s => s.InventoryItem, "inventoryItem")
                .PropertyName(s => s.CreatedBy, "createdBy")
                .PropertyName(s => s.ModifiedBy, "modifiedBy")
                .PropertyName(s => s.DateCreated, "dateCreated")
                .PropertyName(s => s.DateModified, "dateModified");

        public async Task<bool> IndexAsync(Specimen document) => await _searchClient.IndexAsync(document);

        public async Task<BulkIndexResponse> IndexManyAsync(IEnumerable<Specimen> documents) => await _searchClient.IndexManyAsync(documents);
        public async Task<SearchResponse<Specimen>> SearchAsync(FindParams<Data.Shared.Models.Specimen> findParams, Data.Shared.Models.User user)
        {
            var searchTerm = findParams.SearchText;
            var musts = GetFilters(findParams);
            var shoulds = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<Specimen>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                shoulds.Add(query.MultiMatch(mm => mm.Fields(mmf => mmf
                            .Field(m => m.Name)
                            .Field("name.nameSearch"))
                            .Query(searchTerm)
                            .Fuzziness(Fuzziness.AutoLength(1, 5))));
            }

            musts.Add(FilterByVisibility(query, user));

            var searchDescriptor = new SearchDescriptor<Specimen>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray())));

            var countDescriptor = new CountDescriptor<Specimen>()
                .Query(q => q
                    .Bool(b => b
                        .Should(shoulds.ToArray())
                        .Must(musts.ToArray())));

            // Sort
            if (findParams.SortDirection != SortDirection.None)
            {
                if (findParams.SortBy == null)
                {
                    findParams.SortBy = "DateCreated";
                }

                var inventorySorts = GetInvetorySorts();

                if (findParams.SortDirection == SortDirection.Ascending)
                {
                    searchDescriptor.Sort(s => s.Field(f => f.Field(inventorySorts[findParams.SortBy]).Ascending()));
                }
                else
                {
                    searchDescriptor.Sort(s => s.Field(f => f.Field(inventorySorts[findParams.SortBy]).Descending()));
                }
            }

            // Aggregations
            searchDescriptor.Aggregations(a => a.Terms("Stage", t => t.Field(f => f.SpecimenStage)));


            var response = await _searchClient.SearchAsync(pi => searchDescriptor.Skip(findParams.Skip).Take(findParams.Take), pi => countDescriptor);

            return response;
        }

        private Dictionary<string, Expression<Func<Specimen, object>>> GetInvetorySorts() => new Dictionary<string, Expression<Func<Specimen, object>>>
        {
            { "ScientificName", s => s.Lifeform.ScientificName.Suffix("keyword") },
            { "CommonName", s => s.Lifeform.CommonName.Suffix("keyword") },
            { "Quantity", s => s.Quantity},
            { "Stage", s => s.SpecimenStage },
            { "Status", s => s.InventoryItem.Status },
            { "DateAcquired", s => s.InventoryItem.DateAcquired },
            { "Origin", s => s.InventoryItem.Origin },
            { "DateCreated", s => s.DateCreated }
        };

        private QueryContainer FilterByVisibility(QueryContainerDescriptor<Specimen> query, Data.Shared.Models.User user) =>
            query.Bool(b => b
                    .Should(s => !s.Exists(t => t.Field(f => f.InventoryItem.User)) ||
                                 s.Term(t => t.InventoryItem.Visibility, Visibility.Public) ||
                                 s.Term(t => t.InventoryItem.User.Id, user.Id) ||
                                // Not hidden
                                (!(s.Term(t => t.InventoryItem.Visibility, Visibility.Hidden) ||
                                    (s.Term(t => t.InventoryItem.Visibility, Visibility.Inherit) &&
                                    s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Hidden)) ||
                                    (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Inherit) &&
                                    s.Term(t => t.InventoryItem.User.ProfileVisibility, Visibility.Hidden))) &&
                                    // Inherited
                                    ((s.Term(t => t.InventoryItem.Visibility, Visibility.Inherit) &&
                                        (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Public) ||
                                        (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Inherit) &&
                                        s.Term(t => t.InventoryItem.User.ProfileVisibility, Visibility.Public)) ||
                                        (s.Term(t => t.InventoryItem.User.PlantInfoVisibility, Visibility.Contacts) &&
                                        s.Term(t => t.InventoryItem.User.ContactIds, user.Id)))) ||
                                    // Contacts
                                    (s.Term(t => t.InventoryItem.Visibility, Visibility.Contacts) &&
                                    s.Term(t => t.InventoryItem.User.ContactIds, user.Id))))));

        private List<QueryContainer> GetFilters(FindParams<Data.Shared.Models.Specimen> findParams)
        {
            var musts = new List<QueryContainer>();
            var query = new QueryContainerDescriptor<Specimen>();
            var specimenFindParams = findParams as SpecimenFindParams;
            if (findParams.CreatedBy != null)
            {
                musts.Add(query.Match(m => m.Field(f => f.CreatedBy).Query(findParams.CreatedBy)));
            }

            if (specimenFindParams.Filters != null)
            {
                var filters = specimenFindParams.Filters;

                var stageFilter = filters.StageFilter;

                if (!string.IsNullOrEmpty(stageFilter.Value))
                {
                    musts.Add(query.Term(t => t.SpecimenStage, stageFilter.Value));
                }
            }

            return musts;
        }
    }
}
