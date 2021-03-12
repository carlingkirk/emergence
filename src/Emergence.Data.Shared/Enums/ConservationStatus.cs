using System.ComponentModel;

namespace Emergence.Data.Shared
{
    public enum ConservationStatus
    {
        Unknown = 0,
        [Description("Critically Endangered")]
        CriticallyEndangered = 1,
        Endangered = 2,
        Vulnerable = 3,
        [Description("Near Threatened")]
        NearThreatened = 4,
        [Description("Least Concern")]
        LeastConcern = 5,
        [Description("Possibly Extirpated")]
        PossiblyExtirpated = 6,
        [Description("Presumed Extirpated")]
        PresumedExtirpated = 7,
        [Description("Not Evaluated")]
        NotEvaluated = 97,
        [Description("Not Applicable")]
        NotApplicable = 98,
        [Description("Data Deficient")]
        DataDeficient = 99
    }
}
