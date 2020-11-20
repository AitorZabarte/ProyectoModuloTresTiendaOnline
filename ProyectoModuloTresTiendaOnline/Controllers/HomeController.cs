using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProyectoModuloTresTiendaOnline.Data;
using ProyectoModuloTresTiendaOnline.Models;

namespace ProyectoModuloTresTiendaOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Producto.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> MiPerfil(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> CambiarDatos(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarDatos(string id, AppUser user)
        {
            if (id != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Where(z => z.Id == id).FirstOrDefault().Nombre = user.Nombre;
                    _context.Users.Where(z => z.Id == id).FirstOrDefault().Apellidos = user.Apellidos;
                    _context.Users.Where(z => z.Id == id).FirstOrDefault().Foto = user.Foto;
                    _context.Users.Where(z => z.Id == id).FirstOrDefault().FechaDeNacimiento = user.FechaDeNacimiento;
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DarPermiso()
        {
            List<AppUser> usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> HacerAdmin(string id)
        {
            if (id == null)
            {
                NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (id != null)
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }

            return RedirectToAction(nameof(DarPermiso));
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> QuitarAdmin(string id)
        {
            if (id == null)
            {
                NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (id != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "admin");
            }

            return RedirectToAction(nameof(DarPermiso));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
