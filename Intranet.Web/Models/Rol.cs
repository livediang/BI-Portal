namespace Intranet.Web.Models
{
    public class Rol
    {
        public int rolId { get; set; }
        public string rolName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
