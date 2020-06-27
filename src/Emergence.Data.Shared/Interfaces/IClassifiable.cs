namespace Emergence.Data.Shared.Interfaces
{
    interface IClassifiable
    {
        string Kingdom { get; set; }
        string Phylum { get; set; }
        string Subphylum { get; set; }
        string Class { get; set; }
        string Subclass { get; set; }
        string Order { get; set; }
        string Superfamily { get; set; }
        string Tribe { get; set; }
        string Family { get; set; }
        string Subfamily { get; set; }
        string Genus { get; set; }
        string Subgenus { get; set; }
        string Species { get; set; }
        string Subspecies { get; set; }
        string Variety { get; set; }
        string Form { get; set; }
    }
}
