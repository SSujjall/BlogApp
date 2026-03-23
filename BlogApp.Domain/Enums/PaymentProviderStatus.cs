using System.Net.NetworkInformation;

namespace BlogApp.Domain.Enums
{
    public enum EsewaResStatus
    {
        COMPLETE,
        PENDING,
        FULL_REFUND,
        PARTIAL_REFUND,
        AMBIGUOUS,
        NOT_FOUND,
        CANCELED
    }

    public static class KhaltiResStatus
    {
        public const string Completed = "Completed";
        public const string Pending = "Pending";
        public const string Initiated = "Initiated";
        public const string Refunded = "Refunded";
        public const string Expired = "Expired";
        public const string UserCanceled = "User Canceled";
    }
}
