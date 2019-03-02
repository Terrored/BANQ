namespace Model.Identity
{
    public class ApplicationUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public int UserId { get; set; }
    }
}
