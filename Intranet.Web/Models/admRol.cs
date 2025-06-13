using System.ComponentModel.DataAnnotations;

namespace Intranet.Web.Models
{
    public class admRol
    {
        [Key]
        public int idRol { get; set; }
        public string nameRol { get; set; }

        public ICollection<admUser> Users { get; set; } = new List<admUser>();
    }
}
