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
    public class ProductoPedidosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductoPedidosController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ProductoPedidos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductoPedido.Include(p => p.Pedido).Include(p => p.Producto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductoPedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoPedido = await _context.ProductoPedido
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoPedido == null)
            {
                return NotFound();
            }

            return View(productoPedido);
        }
        public async Task<IActionResult> ConfirmarPedido()
        {
            var productoPedidos = from p in _context.ProductoPedido.Include(p=>p.Pedido).Include(z => z.Producto).Where(z => z.Pedido.AppUserId == _userManager.GetUserId(User))
                          select p;
            return View(await productoPedidos.Where(x=>x.Pedido.PedidoFinalizado!="Finalizado").ToListAsync());
        }
        public async Task<IActionResult> CambiarDatosDelPedido(string direccion)
        {
            decimal precioTotal = 0;
            decimal precioFinal = 0;
            var pedidos = from p in _context.Pedido.Where(z => z.AppUserId == _userManager.GetUserId(User))
                                  select p;
            var productos=from p in _context.ProductoPedido.Include(p => p.Pedido).Include(z => z.Producto).Where(z => z.Pedido.AppUserId == _userManager.GetUserId(User)).Where(x => x.Pedido.PedidoFinalizado=="")
                          select p;
            foreach (var prod in productos)
            {   
                precioFinal= prod.CantidadProducto * prod.Producto.Precio;
                precioTotal += precioFinal;
            }
            pedidos.Where(z => z.PedidoFinalizado == "").FirstOrDefault().PedidoFinalizado = "Finalizado";
            pedidos.Where(z => z.PedidoFinalizado == "").FirstOrDefault().Direccion = direccion;
            pedidos.Where(z => z.PedidoFinalizado == "").FirstOrDefault().PrecioTotal = precioTotal;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Productos");
        }
        // GET: ProductoPedidos/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id");
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre");
            return View();
        }

        // POST: ProductoPedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CantidadProducto,ProductoId,PedidoId")] ProductoPedido productoPedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoPedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id", productoPedido.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre", productoPedido.ProductoId);
            return View(productoPedido);
        }

        // GET: ProductoPedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoPedido = await _context.ProductoPedido.FindAsync(id);
            if (productoPedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id", productoPedido.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre", productoPedido.ProductoId);
            return View(productoPedido);
        }

        // POST: ProductoPedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CantidadProducto,ProductoId,PedidoId")] ProductoPedido productoPedido)
        {
            if (id != productoPedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoPedidoExists(productoPedido.Id))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedido, "Id", "Id", productoPedido.PedidoId);
            ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre", productoPedido.ProductoId);
            return View(productoPedido);
        }

        // GET: ProductoPedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoPedido = await _context.ProductoPedido
                .Include(p => p.Pedido)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoPedido == null)
            {
                return NotFound();
            }

            return View(productoPedido);
        }

        // POST: ProductoPedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoPedido = await _context.ProductoPedido.FindAsync(id);
            _context.ProductoPedido.Remove(productoPedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoPedidoExists(int id)
        {
            return _context.ProductoPedido.Any(e => e.Id == id);
        }
    }
}
