namespace Emergence.Data.Shared
{
    public enum ConservationStatus
    {
        NotEvaluated,
        CriticallyEndangered = 1,
        Endangered = 2,
        Vulnerable = 3,
        NearThreatened = 4,
        LeastConcern = 5,
        PossiblyExtirpated = 6,
        PresumedExtirpated = 7,
        NotApplicable = 98,
        DataDeficient = 99
    }
}
