using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Chartdesc
{
    public int SqlId { get; set; }

    public string? ChartWindow { get; set; }

    public string? BaseTable { get; set; }

    public string? BaseKeyFields { get; set; }

    public string? BaseKeyValues { get; set; }

    public string? ChannelNameTemplate { get; set; }

    public string? ChannelNameFields { get; set; }

    public string? Y2xselectTemplate { get; set; }

    public string? Y2xselectFields { get; set; }

    public int? Yalgorithm { get; set; }

    public int? Xalgorithm { get; set; }

    public string? Ytable { get; set; }

    public string? Xtable { get; set; }

    public string? Yfields { get; set; }

    public string? Xfields { get; set; }

    public int? Flags { get; set; }

    public int? Width { get; set; }

    public int? Color { get; set; }

    public int? Mode { get; set; }

    public string? Xselect { get; set; }

    public string? XselectFields { get; set; }

    public string? Yselect { get; set; }

    public string? YselectFields { get; set; }

    public int? AxisGroup { get; set; }

    public string? TemplateTags { get; set; }

    public int? LineType { get; set; }
}
