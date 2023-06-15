using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class KurParam
{
    public int SqlId { get; set; }

    public int? KurKakR { get; set; }

    public string? UrResults { get; set; }

    public int? KdataAfterUr { get; set; }
}
