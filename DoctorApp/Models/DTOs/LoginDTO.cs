using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Models.DTOs
{
    public class LoginDTO
    {
        public string Usuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
    }
}
