using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum SpecimenStage
    {
        [Description("")]
        Unknown,
        Seed,
        Ordered,
        Stratification,
        Germination,
        Growing,
        [Description("In Ground")]
        InGround,
        Blooming,
        Diseased,
        Deceased
    }
}
