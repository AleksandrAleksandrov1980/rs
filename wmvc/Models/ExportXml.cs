using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class ExportXml
{
    public int SqlId { get; set; }

    public int? Num { get; set; }

    public int? Group { get; set; }

    public int? Sel { get; set; }

    public string? Level1 { get; set; }

    public string? SubLevel1 { get; set; }

    public string? Rname1 { get; set; }

    public string? Level2 { get; set; }

    public string? SubLevel2 { get; set; }

    public string? Rname2 { get; set; }

    public string? Level3 { get; set; }

    public string? SubLevel3 { get; set; }

    public string? Rname3 { get; set; }
}
