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
        Dormant,
        Growing,
        [Description("In Ground")]
        InGround,
        Blooming,
        Diseased,
        Deceased
    }
}
