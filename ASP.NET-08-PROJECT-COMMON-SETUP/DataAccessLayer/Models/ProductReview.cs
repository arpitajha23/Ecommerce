using System;
using System.Collections.Generic;
using DataAccessLayer.Models;
using PresentationLayer.Models;

namespace DataAccessLayer.Models;

public partial class ProductReview
{
    public int ReviewId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductReviewImage> ProductReviewImages { get; set; } = new List<ProductReviewImage>();

    public virtual Appuser? User { get; set; }
}
