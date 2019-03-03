namespace Model.Identity
{
    public class ApplicationUserClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
