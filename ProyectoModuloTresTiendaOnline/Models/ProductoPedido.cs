using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoModuloTresTiendaOnline.Models
{
    public class ProductoPedido
    {
        public int Id { get; set; }
        public int CantidadProducto { get; set; }
        public decimal PrecioFinal { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}
