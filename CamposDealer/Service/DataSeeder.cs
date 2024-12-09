namespace CamposDealer.Service
{
    using CamposDealer.DB;
    using CamposDealer.Models;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataSeeder
    {
        private readonly Contexto _context;
        private readonly ApiService _apiService;

        public DataSeeder(Contexto context, ApiService apiService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        public async Task SeedData()
        {
            try
            {
                if (!_context.Clientes.Any())
                {
                    await SeedClientes();
                }

                if (!_context.Produtos.Any())
                {
                    await SeedProdutos();
                }

                if (!_context.Vendas.Any())
                {
                    await SeedVendas();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao realizar o SeedData: {ex.Message}");
                throw;
            }
        }

        private async Task SeedClientes()
        {
            try
            {
                var clientes = await _apiService.GetDataFromApiCli("https://camposdealer.dev/Sites/TesteAPI/cliente");
                var novosClientes = clientes.Select(c => new Cliente
                {
                    nmCliente = c.nmCliente,
                    cidade = c.cidade
                }).ToList();

                _context.Clientes.AddRange(novosClientes);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir clientes: {ex.Message}");
                throw;
            }
        }

        private async Task SeedProdutos()
        {
            try
            {
                var produtos = await _apiService.GetDataFromApiProd("https://camposdealer.dev/Sites/TesteAPI/produto");
                var novosProdutos = produtos.Select(p => new Produto
                {
                    dscProduto = p.dscProduto,
                    vlrUnitario = p.vlrUnitario
                }).ToList();

                _context.Produtos.AddRange(novosProdutos);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir produtos: {ex.Message}");
                throw;
            }
        }

        private async Task SeedVendas()
        {
            try
            {
                var vendas = await _apiService.GetDataFromApiVenda("https://camposdealer.dev/Sites/TesteAPI/venda");

                var vendasValidas = vendas
                .Where(v =>
                _context.Clientes.Any(c => c.idCliente == v.idCliente) &&
                _context.Produtos.Any(p => p.idProduto == v.idProduto))
                .Select(v => new Venda
                {
                    idCliente = v.idCliente,
                    idProduto = v.idProduto,
                    qtdVenda = v.qtdVenda,
                    vlrUnitarioVenda = v.vlrUnitarioVenda,
                    dthVenda = v.dthVenda,
                    Cliente = _context.Clientes.FirstOrDefault(c => c.idCliente == v.idCliente) ?? throw new InvalidOperationException("Cliente não encontrado."),
                    Produto = _context.Produtos.FirstOrDefault(p => p.idProduto == v.idProduto) ?? throw new InvalidOperationException("Produto não encontrado."),

                }).ToList();

                _context.Vendas.AddRange(vendasValidas);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir vendas: {ex.Message}");
                throw;
            }

        }
    }
}
