using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoModuloTresTiendaOnline.Models
{
    public class AppUser:IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Foto { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
    }
}
