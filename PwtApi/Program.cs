using Microsoft.EntityFrameworkCore;
using PwtApi.Data;
using PwtApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

// Add DbContext
builder.Services.AddDbContext<PwtDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

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
    try
    {
        var products = await db.Products.ToListAsync();
        var inventories = await db.Inventories.ToListAsync();

        var productsWithStock = products.Select(p => new ProductDto
        {
            SupplierNo = p.SupplierNo,
            ItemGroupId = p.ItemGroupId,
            ItemGroupName = p.ItemGroupName,
            StyleNo = p.StyleNo,
            ItemDescription = p.ItemDescription,
            Size = p.Size,
            EAN = p.EAN,
            ColorCodeName = p.ColorCodeName,
            Season = p.Season,
            SuggestedRetailPrice = p.SuggestedRetailPrice,
            InStock = inventories.Any(i => i.EAN == p.EAN && i.InventoryQuantity > 0)
        });

        return Results.Ok(productsWithStock);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to get products: {ex.Message}", statusCode: 500);
    }
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
            SupplierNo = product.SupplierNo,
            ItemGroupId = product.ItemGroupId,
            ItemGroupName = product.ItemGroupName,
            StyleNo = product.StyleNo,
            ItemDescription = product.ItemDescription,
            Size = product.Size,
            EAN = product.EAN,
            ColorCodeName = product.ColorCodeName,
            Season = product.Season,
            SuggestedRetailPrice = product.SuggestedRetailPrice,
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
            SupplierNo = p.SupplierNo,
            ItemGroupId = p.ItemGroupId,
            ItemGroupName = p.ItemGroupName,
            StyleNo = p.StyleNo,
            ItemDescription = p.ItemDescription,
            Size = p.Size,
            EAN = p.EAN,
            ColorCodeName = p.ColorCodeName,
            Season = p.Season,
            SuggestedRetailPrice = p.SuggestedRetailPrice,
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
            SupplierNo = p.SupplierNo,
            ItemGroupId = p.ItemGroupId,
            ItemGroupName = p.ItemGroupName,
            StyleNo = p.StyleNo,
            ItemDescription = p.ItemDescription,
            Size = p.Size,
            EAN = p.EAN,
            ColorCodeName = p.ColorCodeName,
            Season = p.Season,
            SuggestedRetailPrice = p.SuggestedRetailPrice,
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
            SupplierNo = p.SupplierNo,
            ItemGroupId = p.ItemGroupId,
            ItemGroupName = p.ItemGroupName,
            StyleNo = p.StyleNo,
            ItemDescription = p.ItemDescription,
            Size = p.Size,
            EAN = p.EAN,
            ColorCodeName = p.ColorCodeName,
            Season = p.Season,
            SuggestedRetailPrice = p.SuggestedRetailPrice,
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
