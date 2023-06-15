using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class ComTi
{
    public int SqlId { get; set; }

    public double? SensErrorProc { get; set; }

    public double? SensWrongProc { get; set; }

    public int? CreatePtiUOnlyGen { get; set; }

    public int? RewriteTi { get; set; }

    public string? FiltrTi { get; set; }

    public int? ShowFiltrForm { get; set; }

    public int? GenModel { get; set; }

    public double? SensErrorMwt { get; set; }

    public double? SensWrongMwt { get; set; }

    public int? PtiAddType { get; set; }

    public int? PtiCalcType { get; set; }

    public int? UpdateSta { get; set; }

    public int? ActiveFiltr { get; set; }

    public int? ActiveFiltrTopology { get; set; }

    public int? DtiSource { get; set; }

    public int? DtsSource { get; set; }

    public int? TiSource { get; set; }

    public int? TsSource { get; set; }

    public int? PtiSelArea { get; set; }

    public int? AddUnknownTi { get; set; }

    public int? AddUnknownTs { get; set; }

    public int? UpdateStaNode { get; set; }

    public double? SensErrorUProc { get; set; }

    public int? Oik { get; set; }
}
