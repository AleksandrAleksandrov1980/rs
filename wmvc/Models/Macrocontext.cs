using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Macrocontext
{
    public int SqlId { get; set; }

    public string? Form { get; set; }

    public string? Col { get; set; }

    public string? Macrofile { get; set; }

    public string? Macrodesc { get; set; }

    public int? Defaultappendix { get; set; }

    public string? Addstr { get; set; }

    public int? Formtype { get; set; }

    public string? TemplateTags { get; set; }
}
