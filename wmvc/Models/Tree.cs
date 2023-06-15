using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Tree
{
    public int SqlId { get; set; }

    public int? TreeId { get; set; }

    public string? TreeName { get; set; }
}
