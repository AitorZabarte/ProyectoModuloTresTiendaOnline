using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProyectoModuloTresTiendaOnline.Data;
using ProyectoModuloTresTiendaOnline.Models;

[assembly: HostingStartup(typeof(ProyectoModuloTresTiendaOnline.Areas.Identity.IdentityHostingStartup))]
namespace ProyectoModuloTresTiendaOnline.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}