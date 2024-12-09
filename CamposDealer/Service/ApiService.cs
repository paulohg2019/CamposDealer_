using CamposDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    private async Task<List<T>> GetDataFromApiAsync<T>(string url)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("A URL inválida.", nameof(url));

            var response = await _httpClient.GetStringAsync(url);

            if (string.IsNullOrEmpty(response))
                throw new InvalidOperationException("A resposta da API está vazia.");

            // Desserializa o JSON recebido
            var correctedJson = JsonConvert.DeserializeObject<string>(response);

            if (string.IsNullOrEmpty(correctedJson))
            {
                throw new InvalidOperationException("A API não retornou dados válidos.");
            }

            var result = JsonConvert.DeserializeObject<List<T>>(correctedJson);


            if (result == null || result.Count == 0)
                throw new InvalidOperationException("A API não retornou dados válidos na deserialização.");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Cliente>> GetDataFromApiCli(string url)
    {
        return await GetDataFromApiAsync<Cliente>(url);
    }

    public async Task<List<Produto>> GetDataFromApiProd(string url)
    {
        return await GetDataFromApiAsync<Produto>(url);
    }

    public async Task<List<Venda>> GetDataFromApiVenda(string url)
    {
        return await GetDataFromApiAsync<Venda>(url);
    }
}
