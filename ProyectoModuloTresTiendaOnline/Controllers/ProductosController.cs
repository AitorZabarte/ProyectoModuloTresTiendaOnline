using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoModuloTresTiendaOnline.Data;
using ProyectoModuloTresTiendaOnline.Models;

namespace ProyectoModuloTresTiendaOnline.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public ProductosController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Productos
        public async Task<IActionResult> Index(string buscarTexto, int buscarPrecio)
        {
            var productos = from p in _context.Producto.Include(x=>x.ProductoPedidos).ThenInclude(p=>p.Pedido).ThenInclude(p=>p.AppUser)
                            select p;
            if (!string.IsNullOrEmpty(buscarTexto)&& buscarPrecio==0)
            {
                productos = productos.Where(x => x.Nombre.Contains(buscarTexto) ||x.Equipo.Contains(buscarTexto) ||x.Liga.Contains(buscarTexto) ||x.Deporte.Contains(buscarTexto));
            }
            else if (string.IsNullOrEmpty(buscarTexto)&&buscarPrecio > 0)
            {
                productos = productos.Where(x => x.Precio > buscarPrecio);
            }
            else if (!string.IsNullOrEmpty(buscarTexto)&&buscarPrecio>0)
            {
                productos = productos.Where(x => (x.Nombre.Contains(buscarTexto) || x.Equipo.Contains(buscarTexto) || x.Liga.Contains(buscarTexto) || x.Deporte.Contains(buscarTexto))&& x.Precio > buscarPrecio);
            }
            return View(await productos.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aniadir(int id,int cantidad)
        {
            var producto = await _context.Producto.FirstOrDefaultAsync(m => m.Id == id);

            if (id != producto.Id)
            {
                return NotFound();
            }
                try
                {
                    bool pedidoFin = true;
                    var pedidos = from p in _context.Pedido.Where(z => z.AppUserId == _userManager.GetUserId(User))
                                  select p;
                    foreach (var pedido in pedidos)
                    {
                        if (pedido.PedidoFinalizado == "")
                        {
                            pedidoFin = false;
                        }
                    }
                    if (pedidoFin == true)
                    {
                        _context.Add(new Pedido
                        {
                            FechaDePedido = DateTime.Now,
                            PedidoFinalizado = "",
                            AppUserId = _userManager.GetUserId(User)
                        });
                    }
                    await _context.SaveChangesAsync();
                    _context.Add(new ProductoPedido
                    {
                        CantidadProducto = cantidad,
                        ProductoId = id,
                        PedidoId = pedidos.FirstOrDefault(z => z.PedidoFinalizado == "").Id,
                        PrecioFinal=cantidad*producto.Precio
                        
                    });
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
        }
        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Imagen,Deporte,Liga,Equipo")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Imagen,Deporte,Liga,Equipo")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }
    }
}
