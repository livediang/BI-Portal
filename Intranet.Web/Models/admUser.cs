using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Web.Models
{
    public class admUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUser { get; set; }
        public string nameUser { get; set; }
        public string mailUser { get; set; }
        public string passwordUser { get; set; }
        public int idRol { get; set; }

        [ForeignKey("idRol")]
        public admRol? Rol { get; set; }
    }
}
