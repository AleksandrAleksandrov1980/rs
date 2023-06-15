using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Area
{
    public int Na { get; set; }

    public string? Name { get; set; }

    public int? No { get; set; }

    public int? Nng { get; set; }

    public double? Pg { get; set; }

    public double? Pn { get; set; }

    public double? Qg { get; set; }

    public double? Qn { get; set; }

    public double? Dp { get; set; }

    public double? Dq { get; set; }

    public double? Pop { get; set; }

    public double? Poq { get; set; }

    public double? Vnp { get; set; }

    public double? Vnq { get; set; }

    public double? Pfull { get; set; }

    public string? Navet { get; set; }

    public string? NaArea { get; set; }

    public double? Un { get; set; }

    public int? Sta { get; set; }

    public int? Sel { get; set; }

    public int? Tip { get; set; }

    public double? Epg { get; set; }

    public double? Epn { get; set; }

    public double? Esh { get; set; }

    public double? Ekn { get; set; }

    public string? Tmpun { get; set; }

    public double? DpLine { get; set; }

    public double? DpTran { get; set; }

    public double? DpShunt { get; set; }

    public double? ShLine { get; set; }

    public double? ShTran { get; set; }

    public double? DpNag { get; set; }

    public double? DpXx { get; set; }

    public double? DqLine { get; set; }

    public double? DqTran { get; set; }

    public double? DqShunt { get; set; }

    public double? ShqLine { get; set; }

    public double? ShqTran { get; set; }

    public double? DqNag { get; set; }

    public double? DqXx { get; set; }

    public double? Tc { get; set; }

    public double? PnMin { get; set; }

    public double? PnMax { get; set; }

    public double? PgMin { get; set; }

    public double? PgMax { get; set; }

    public double? Npnag { get; set; }

    public double? Npgen { get; set; }

    public double? TiPn { get; set; }

    public double? TiPg { get; set; }

    public double? PnZb { get; set; }

    public double? PgZb { get; set; }

    public double? Kpodpg { get; set; }

    public double? Kpodpn { get; set; }

    public double? PopCmp { get; set; }

    public double? PopCmpD { get; set; }

    public int? MeteoId { get; set; }

    public double? Tpos { get; set; }

    public virtual ICollection<Interarea> InterareaBNaNavigations { get; set; } = new List<Interarea>();

    public virtual ICollection<Interarea> InterareaENaNavigations { get; set; } = new List<Interarea>();

    public virtual ICollection<KocBaseRg> KocBaseRgs { get; set; } = new List<KocBaseRg>();
}
