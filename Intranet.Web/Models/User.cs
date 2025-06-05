using Microsoft.AspNetCore.Identity;

namespace Intranet.Web.Models
{
    public class User
    {
        public int idUser { get; set; }
        public string nameUser { get; set; }
        public string mailUser { get; set; }
        public string passwordUser { get; set; } = string.Empty;
        public int idRol { get; set; }
        public Rol? nameRol { get; set; }
    }
}
