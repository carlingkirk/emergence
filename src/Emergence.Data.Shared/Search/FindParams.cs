using System.Collections.Generic;
using Emergence.Data.Shared.Interfaces;
using Emergence.Data.Shared.Search;

namespace Emergence.Data.Shared
{
    public class FindParams : IFindParams
    {
        public FindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }

        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public string CreatedBy { get; set; }
        public bool ContactsOnly { get; set; }
        public string SearchTextQuery => SearchText != null ? "%" + SearchText + "%" : null;
        public IEnumerable<Filter> Filters { get; set; }
    }

    public class FindParams<T> : IFindParams
    {
        public FindParams()
        {
            Skip = 0;
            Take = 10;
            SortBy = null;
            SortDirection = SortDirection.Ascending;
        }
        public T Shape { get; set; }

        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}