using System.ComponentModel.DataAnnotations;

namespace ChienVHShopOnline.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }
}
