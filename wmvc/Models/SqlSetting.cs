using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class SqlSetting
{
    public int SqlId { get; set; }

    public string? ConnString { get; set; }

    public string? Database { get; set; }

    public int? ReadNullTable { get; set; }

    public int? WriteNullTable { get; set; }

    public int? AllowDelRows { get; set; }

    public int? AllowAddRows { get; set; }

    public int? AllowAddCols { get; set; }

    public int? AllowAddTable { get; set; }

    public string? SmzuDatabase { get; set; }

    public string? SmzuServers { get; set; }

    public string? SmzuFiles { get; set; }

    public string? ConnStringPg { get; set; }
}
