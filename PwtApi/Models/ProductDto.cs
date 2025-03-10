namespace PwtApi.Models;

public class ProductDto
{
    public int? SupplierNo { get; set; }
    public int? ItemGroupId { get; set; }
    public string? ItemGroupName { get; set; }
    public string? StyleNo { get; set; }
    public string? ItemDescription { get; set; }
    public string? Size { get; set; }
    public string? EAN { get; set; }
    public string? ColorCodeName { get; set; }
    public string? Season { get; set; }
    public decimal? SuggestedRetailPrice { get; set; }
    public bool InStock { get; set; }
} 