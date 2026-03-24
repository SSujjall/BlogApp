namespace BlogApp.Domain.GlobalConfigs
{
    public class EsewaConfig
    {
        public string SecretKey { get; set; }
        public string InitiateUrl { get; set; }
        public string ProductCode { get; set; }
        public string SuccessUrl { get; set; }
        public string FailureUrl { get; set; }
        public string StatusCheckUrl { get; set; }
    }
}
