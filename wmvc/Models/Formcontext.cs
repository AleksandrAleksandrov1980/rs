using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Formcontext
{
    public int SqlId { get; set; }

    public string? Form { get; set; }

    public string? Linkedform { get; set; }

    public string? Linkedname { get; set; }

    public string? Vibork { get; set; }

    public string? Bind { get; set; }

    public int? SrcType { get; set; }

    public int? DstType { get; set; }

    public int? Defaultappendix { get; set; }

    public string? TemplateTags { get; set; }
}
