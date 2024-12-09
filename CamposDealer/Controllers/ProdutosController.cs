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
    public class ProdutosController : Controller
    {
        private readonly Contexto _context;

        public ProdutosController(Contexto context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                var produtos = string.IsNullOrWhiteSpace(searchTerm)
                    ? await _context.Produtos.ToListAsync()
                    : await _context.Produtos
                                    .Where(p => p.dscProduto.Contains(searchTerm))
                                    .ToListAsync();

                ViewBag.SearchTerm = searchTerm;
                return View(produtos);
            }
            catch (Exception ex)
            {
                // Log para desenvolvedores
                Console.Error.WriteLine($"Erro ao carregar a lista de produtos: {ex.Message}");

                // Mensagem amigável para o usuário
                TempData["Error"] = "Ocorreu um erro ao carregar a lista de produtos. Por favor, tente novamente mais tarde.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.idProduto == id);
                if (produto == null)
                {
                    return NotFound();
                }

                return View(produto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar detalhes do produto: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar os detalhes do produto.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idProduto,dscProduto,vlrUnitario")] Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(produto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(produto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao criar produto: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao criar o produto. Por favor, tente novamente.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }

                return View(produto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar produto para edição: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar o produto para edição.";
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idProduto,dscProduto,vlrUnitario")] Produto produto)
        {
            try
            {
                if (id != produto.idProduto)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(produto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao editar produto: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao editar o produto.";
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var produto = await _context.Produtos.FirstOrDefaultAsync(m => m.idProduto == id);
                if (produto == null)
                {
                    return NotFound();
                }

                return View(produto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao carregar produto para exclusão: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao carregar o produto para exclusão.";
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto != null)
                {
                    _context.Produtos.Remove(produto);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao excluir produto: {ex.Message}");
                TempData["Error"] = "Ocorreu um erro ao excluir o produto.";
                return RedirectToAction("Error", "Home");
            }
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.idProduto == id);
        }
    }
}
