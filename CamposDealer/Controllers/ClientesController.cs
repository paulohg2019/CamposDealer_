using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CamposDealer.DB;
using CamposDealer.Models;

namespace CamposDealer.Controllers
{
    public class ClientesController : Controller
    {
        private readonly Contexto _context;

        public ClientesController(Contexto context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                var clientes = string.IsNullOrWhiteSpace(searchTerm)
                    ? await _context.Clientes.ToListAsync()
                    : await _context.Clientes
                                    .Where(c => c.nmCliente.Contains(searchTerm))
                                    .ToListAsync();

                ViewBag.SearchTerm = searchTerm;
                return View(clientes);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar a lista de clientes: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar a lista de clientes. Tente novamente mais tarde.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Clientes/Details
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.idCliente == id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar detalhes do cliente: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar os detalhes do cliente.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idCliente,nmCliente,cidade")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao criar cliente: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao criar o cliente. Por favor, tente novamente.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Clientes/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar cliente para edição: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar o cliente para edição.";
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Clientes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idCliente,nmCliente,cidade")] Cliente cliente)
        {
            try
            {
                if (id != cliente.idCliente)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao editar cliente: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao editar o cliente.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Clientes/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.idCliente == id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar cliente para exclusão: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar o cliente para exclusão.";
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Clientes/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao excluir cliente: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao excluir o cliente.";
                return RedirectToAction("Error", "Home");
            }
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.idCliente == id);
        }
    }
}
