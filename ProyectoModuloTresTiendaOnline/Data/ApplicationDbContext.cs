using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoModuloTresTiendaOnline.Models;

namespace ProyectoModuloTresTiendaOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProyectoModuloTresTiendaOnline.Models.Pedido> Pedido { get; set; }
        public DbSet<ProyectoModuloTresTiendaOnline.Models.Producto> Producto { get; set; }
        public DbSet<ProyectoModuloTresTiendaOnline.Models.ProductoPedido> ProductoPedido { get; set; }
    }
}
