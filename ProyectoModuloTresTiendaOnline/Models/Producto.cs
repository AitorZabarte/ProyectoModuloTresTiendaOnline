using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoModuloTresTiendaOnline.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public string Deporte { get; set; }
        public string Liga { get; set; }
        public string Equipo { get; set; }

        public List<ProductoPedido> ProductoPedidos { get; set; }
    }
}
