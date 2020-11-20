using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoModuloTresTiendaOnline.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaDePedido { get; set; }
        public string Direccion { get; set; }
        public string PedidoFinalizado { get; set; }
        public decimal PrecioTotal { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<ProductoPedido> ProductoPedidos { get; set; }
    }
}
