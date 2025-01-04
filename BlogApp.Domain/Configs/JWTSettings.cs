namespace BlogApp.Domain.Configs
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}
