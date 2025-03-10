using Microsoft.EntityFrameworkCore;
using PwtApi.Data;
using PwtApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

// Add CORS 
// Need to hardcore the localhost link..
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("http://localhost:5259")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add DbContext
builder.Services.AddDbContext<PwtDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


// Test endpoint for database connection
app.MapGet("/test-connection", async (PwtDbContext db) =>
{
    try
    {
        await db.Database.OpenConnectionAsync();
        return Results.Ok("Database connection successful!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database connection failed: {ex.Message}", statusCode: 500);
    }
});

// Get products with stock status
app.MapGet("/products", async (PwtDbContext db) =>
{
    var products = await db.Products
        .Select(p => new ProductDto
        {
            SupplierNo = p.SupplierNo ?? 0,
            StyleNo = p.StyleNo ?? "",
            ItemDescription = p.ItemDescription ?? "",
            Size = p.Size ?? "",
            ColorCodeName = p.ColorCodeName ?? "",
            EAN = p.EAN ?? "",
            InStock = db.Inventories.Any(i => i.EAN == p.EAN && i.InventoryQuantity > 0),
            SuggestedRetailPrice = p.SuggestedRetailPrice ?? 0
        })
        .ToListAsync();
    return Results.Ok(products);
});

// Get single product by EAN
app.MapGet("/products/{ean}", async (string ean, PwtDbContext db) =>
{
    try
    {
        var product = await db.Products.FirstOrDefaultAsync(p => p.EAN == ean);
        if (product == null)
        {
            return Results.NotFound($"Product with EAN {ean} not found");
        }

        var inventories = await db.Inventories.ToListAsync();
        var productDto = new ProductDto
        {
            SupplierNo = product.SupplierNo ?? 0,
            ItemGroupId = product.ItemGroupId ?? 0,
            ItemGroupName = product.ItemGroupName ?? string.Empty,
            StyleNo = product.StyleNo ?? string.Empty,
            ItemDescription = product.ItemDescription ?? string.Empty,
            Size = product.Size ?? string.Empty,
            EAN = product.EAN ?? string.Empty,
            ColorCodeName = product.ColorCodeName ?? string.Empty,
            Season = product.Season ?? string.Empty,
            SuggestedRetailPrice = product.SuggestedRetailPrice ?? 0,
            InStock = inventories.Any(i => i.EAN == product.EAN && i.InventoryQuantity > 0)
        };

        return Results.Ok(productDto);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to get product: {ex.Message}", statusCode: 500);
    }
});

// Get products by size
app.MapGet("/products/size/{size}", async (string size, PwtDbContext db) =>
{
    try
    {
        var products = await db.Products.Where(p => p.Size == size).ToListAsync();
        var inventories = await db.Inventories.ToListAsync();

        var productsWithStock = products.Select(p => new ProductDto
        {
            SupplierNo = p.SupplierNo ?? 0,
            ItemGroupId = p.ItemGroupId ?? 0,
            ItemGroupName = p.ItemGroupName ?? string.Empty,
            StyleNo = p.StyleNo ?? string.Empty,
            ItemDescription = p.ItemDescription ?? string.Empty,
            Size = p.Size ?? string.Empty,
            EAN = p.EAN ?? string.Empty,
            ColorCodeName = p.ColorCodeName ?? string.Empty,
            Season = p.Season ?? string.Empty,
            SuggestedRetailPrice = p.SuggestedRetailPrice ?? 0,
            InStock = inventories.Any(i => i.EAN == p.EAN && i.InventoryQuantity > 0)
        });

        return Results.Ok(productsWithStock);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to get products: {ex.Message}", statusCode: 500);
    }
});

// Get products sorted by price (ascending or descending)
app.MapGet("/products/sort/price", async (bool? ascending, PwtDbContext db) =>
{
    try
    {
        var products = await db.Products.ToListAsync();
        var inventories = await db.Inventories.ToListAsync();

        var productsWithStock = products.Select(p => new ProductDto
        {
            SupplierNo = p.SupplierNo ?? 0,
            ItemGroupId = p.ItemGroupId ?? 0,
            ItemGroupName = p.ItemGroupName ?? string.Empty,
            StyleNo = p.StyleNo ?? string.Empty,
            ItemDescription = p.ItemDescription ?? string.Empty,
            Size = p.Size ?? string.Empty,
            EAN = p.EAN ?? string.Empty,
            ColorCodeName = p.ColorCodeName ?? string.Empty,
            Season = p.Season ?? string.Empty,
            SuggestedRetailPrice = p.SuggestedRetailPrice ?? 0,
            InStock = inventories.Any(i => i.EAN == p.EAN && i.InventoryQuantity > 0)
        });

        var sortedProducts = ascending == true
            ? productsWithStock.OrderBy(p => p.SuggestedRetailPrice)
            : productsWithStock.OrderByDescending(p => p.SuggestedRetailPrice);

        return Results.Ok(sortedProducts);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to get products: {ex.Message}", statusCode: 500);
    }
});

// Get only in-stock products
app.MapGet("/products/instock", async (PwtDbContext db) =>
{
    try
    {
        var products = await db.Products.ToListAsync();
        var inventories = await db.Inventories.ToListAsync();

        var productsWithStock = products.Select(p => new ProductDto
        {
            SupplierNo = p.SupplierNo ?? 0,
            ItemGroupId = p.ItemGroupId ?? 0,
            ItemGroupName = p.ItemGroupName ?? string.Empty,
            StyleNo = p.StyleNo ?? string.Empty,
            ItemDescription = p.ItemDescription ?? string.Empty,
            Size = p.Size ?? string.Empty,
            EAN = p.EAN ?? string.Empty,
            ColorCodeName = p.ColorCodeName ?? string.Empty,
            Season = p.Season ?? string.Empty,
            SuggestedRetailPrice = p.SuggestedRetailPrice ?? 0,
            InStock = inventories.Any(i => i.EAN == p.EAN && i.InventoryQuantity > 0)
        })
        .Where(p => p.InStock)
        .ToList();

        return Results.Ok(productsWithStock);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to get products: {ex.Message}", statusCode: 500);
    }
});

app.Run();
