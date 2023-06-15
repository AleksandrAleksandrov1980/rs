using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Treelevel
{
    public int SqlId { get; set; }

    public int? TreeId { get; set; }

    public int? LevelId { get; set; }

    public int? ParentLevelId { get; set; }

    public string? SysTabName { get; set; }

    public string? SelfKeys { get; set; }

    public string? ParentKeys { get; set; }

    public string? Caption { get; set; }

    public string? ItemName { get; set; }

    public string? Sel { get; set; }

    public int? DontShowOrphans { get; set; }

    public int? Hide { get; set; }

    public int? Uniqize { get; set; }

    public int? RecurseLvl { get; set; }
}
