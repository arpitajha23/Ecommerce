using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class ProductReviewImage
{
    public int ReviewImageId { get; set; }

    public int? ReviewId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ProductReview? Review { get; set; }
}
