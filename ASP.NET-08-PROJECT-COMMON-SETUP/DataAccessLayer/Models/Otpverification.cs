using System;
using System.Collections.Generic;
using PresentationLayer.Models;

namespace DataAccessLayer.Models;

public partial class Otpverification
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public string Otpcode { get; set; } = null!;

    public DateTime Expirytime { get; set; }

    public bool? Isused { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Appuser User { get; set; } = null!;
}
