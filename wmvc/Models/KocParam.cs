using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class KocParam
{
    public int SqlId { get; set; }

    public int? KdataBeforeOc { get; set; }

    public int? KdataAfterOc { get; set; }

    public int? OcResults { get; set; }

    public int? OcKakDemidov { get; set; }
}
