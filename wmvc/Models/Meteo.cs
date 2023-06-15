using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Meteo
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? MeteoId { get; set; }

    public double? Tc { get; set; }
}
