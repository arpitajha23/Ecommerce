using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class SellerUser
{
    public int SellerId { get; set; }

    public string? SellerName { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
