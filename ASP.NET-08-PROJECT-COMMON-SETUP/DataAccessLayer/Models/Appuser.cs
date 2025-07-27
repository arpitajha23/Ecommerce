using System;
using System.Collections.Generic;
using DataAccessLayer.Models;

namespace PresentationLayer.Models;

public partial class Appuser
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; } = null!;
    public string Salt { get; set; } = null!;

    public string? Phone { get; set; }

    public bool? Isemailconfirmed { get; set; }

    public string? Googleid { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Passwordcreatedat { get; set; }

    public DateTime? Passwordmodifiedat { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public DateTime? Deletedat { get; set; }

    public virtual ICollection<Otpverification> Otpverifications { get; set; } = new List<Otpverification>();

    public virtual ICollection<Cart> Carts { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();


}
