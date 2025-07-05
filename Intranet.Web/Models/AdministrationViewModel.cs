namespace Intranet.Web.Models
{
    public class AdministrationViewModel
    {
        public List<admUser> Users { get; set; } = new List<admUser>();
        public string SearchTerm { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
