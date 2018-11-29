namespace MyIdeaPool
{
    public class AuthenticationSettings
    {
        public virtual string SymmetricSecurityKey { get; set; }
        public virtual int TokenExpiryMinutes { get; set; }
    }
}