using System.Net.Http.Json;
using PwtBlazor2.Models;

namespace PwtBlazor2.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5198/";
    private const int PageSize = 10;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        try
        {
            Console.WriteLine("Fetching products from API...");
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("products") ?? new List<Product>();
            Console.WriteLine($"Successfully fetched {products.Count} products");
            return products;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task<List<Product>> GetProductsSortedByPriceAsync(bool ascending = true)
    {
        try
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>($"products/sort/price?ascending={ascending}") ?? new List<Product>();
            return products;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching sorted products: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task<List<Product>> GetInStockProductsAsync()
    {
        try
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("products/instock") ?? new List<Product>();
            return products;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching in-stock products: {ex.Message}");
            return new List<Product>();
        }
    }
}

public class ProductsResponse
{
    public List<Product> Products { get; set; } = new();
    public int TotalCount { get; set; }
} 