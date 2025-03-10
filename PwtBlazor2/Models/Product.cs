using System.Text.Json.Serialization;

namespace PwtBlazor2.Models;

public class Product
{
    public int SupplierNo { get; set; }
    public int ItemGroupId { get; set; }
    public string ItemGroupName { get; set; } = string.Empty;
    public string StyleNo { get; set; } = string.Empty;
    public string ItemDescription { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string EAN { get; set; } = string.Empty;
    public string ColorCodeName { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public decimal SuggestedRetailPrice { get; set; }
    public bool InStock { get; set; }
} 