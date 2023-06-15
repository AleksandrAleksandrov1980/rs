using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class TiSetting
{
    public int SqlId { get; set; }

    public double? Timer { get; set; }

    public string? CosRegimsPath { get; set; }

    public int? CosSaveFiles { get; set; }

    public string? PathBd { get; set; }

    public int? CosSaveBd { get; set; }

    public int? MaxNSrez { get; set; }

    public int? DelNSrez { get; set; }

    public int? CalcStat { get; set; }

    public int? DeleteCosFiles { get; set; }

    public int? LiveHoursCosFiles { get; set; }

    public string? PathGetTm { get; set; }

    public string? PathTmp { get; set; }

    public string? OikName { get; set; }

    public string? CosShabl { get; set; }
}
