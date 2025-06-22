using PresentationLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Appuser? User { get; set; }
}
