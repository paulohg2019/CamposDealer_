using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CamposDealer.DB;
using CamposDealer.Models;
using NuGet.Packaging;
using Microsoft.Extensions.Logging; // Adicionei para logs

namespace CamposDealer.Controllers
{
    public class VendasController : Controller
    {
        private readonly Contexto _context;
        private readonly ILogger<VendasController> _logger; // Adicionando logger

        public VendasController(Contexto context, ILogger<VendasController> logger)
        {
            _context = context;
            _logger = logger; // Inicializando logger
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            List<Venda> vendas = new List<Venda>();

            try
            {
                vendas = await BuscarVendas(vendas, searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar vendas.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar buscar as vendas. Por favor, tente novamente mais tarde.";
            }

            ViewBag.SearchTerm = searchTerm;
            return View(vendas);
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.Cliente)
                    .Include(v => v.Produto)
                    .FirstOrDefaultAsync(m => m.idVenda == id);
                if (venda == null)
                {
                    return NotFound();
                }

                return View(venda);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar detalhes da venda.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar exibir os detalhes da venda. Por favor, tente novamente mais tarde.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["idCliente"] = new SelectList(_context.Clientes, "idCliente", "nmCliente");
                ViewData["idProduto"] = new SelectList(_context.Produtos, "idProduto", "dscProduto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dados para criação da venda.");
                TempData["ErrorMessage"] = "Houve um erro ao carregar os dados. Por favor, tente novamente mais tarde.";
            }

            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idVenda,idCliente,idProduto,qtdVenda,vlrUnitarioVenda,dthVenda")] Venda venda)
        {
            try
            {
                venda = await BuscarCliProdVendas(venda);

                if (ModelState.IsValid)
                {
                    _context.Add(venda);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar venda.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar salvar a venda. Por favor, tente novamente mais tarde.";
            }

            ViewData["idCliente"] = new SelectList(_context.Clientes, "idCliente", "nmCliente", venda.idCliente);
            ViewData["idProduto"] = new SelectList(_context.Produtos, "idProduto", "dscProduto", venda.idProduto);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var venda = await _context.Vendas.FindAsync(id);
                if (venda == null)
                {
                    return NotFound();
                }

                ViewData["idCliente"] = new SelectList(_context.Clientes, "idCliente", "nmCliente");
                ViewData["idProduto"] = new SelectList(_context.Produtos, "idProduto", "dscProduto");
                return View(venda);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar venda.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar editar a venda. Por favor, tente novamente mais tarde.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idVenda,idCliente,idProduto,qtdVenda,vlrUnitarioVenda,dthVenda")] Venda venda)
        {
            try
            {
                venda = await BuscarCliProdVendas(venda);

                if (id != venda.idVenda)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(venda.idVenda))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Erro de concorrência ao editar venda.");
                    TempData["ErrorMessage"] = "Houve um erro de concorrência ao editar a venda. Por favor, tente novamente mais tarde.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar venda.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar editar a venda. Por favor, tente novamente mais tarde.";
            }

            ViewData["idCliente"] = new SelectList(_context.Clientes, "idCliente", "nmCliente", venda.idCliente);
            ViewData["idProduto"] = new SelectList(_context.Produtos, "idProduto", "dscProduto", venda.idProduto);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.Cliente)
                    .Include(v => v.Produto)
                    .FirstOrDefaultAsync(m => m.idVenda == id);
                if (venda == null)
                {
                    return NotFound();
                }

                return View(venda);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dados para exclusão.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar carregar os dados da venda para exclusão. Por favor, tente novamente mais tarde.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var venda = await _context.Vendas.FindAsync(id);
                if (venda != null)
                {
                    _context.Vendas.Remove(venda);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao confirmar exclusão da venda.");
                TempData["ErrorMessage"] = "Houve um erro ao tentar excluir a venda. Por favor, tente novamente mais tarde.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.idVenda == id);
        }

        private async Task<List<Venda>> BuscarVendas(List<Venda> vendas, string searchTerm)
        {
            vendas = string.IsNullOrWhiteSpace(searchTerm)
                   ? await _context.Vendas
                                   .Include(v => v.Cliente)
                                   .Include(v => v.Produto)
                                   .ToListAsync()
                   : await _context.Vendas
                                   .Include(v => v.Cliente)
                                   .Include(v => v.Produto)
                                   .Where(v => v.Cliente.nmCliente.Contains(searchTerm) || v.Produto.dscProduto.Contains(searchTerm))
                                   .ToListAsync();
            return vendas;
        }

        private async Task<Venda> BuscarCliProdVendas(Venda venda)
        {
            var cliente = await _context.Clientes
                                          .Where(c => c.idCliente == venda.idCliente)
                                          .FirstOrDefaultAsync();

            var produto = await _context.Produtos
                                  .Where(c => c.idProduto == venda.idProduto)
                                  .FirstOrDefaultAsync();

            if (venda.Cliente.idCliente == 0 && venda.Produto.idProduto == 0)
            {
                venda.Cliente = new Cliente();
                venda.Produto = new Produto();

                if (cliente != null)
                    venda.Cliente = cliente;

                if (produto != null)
                    venda.Produto = produto;
            }

            return venda;
        }
    }
}
