using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Node
{
    public int Ny { get; set; }

    public string? Name { get; set; }

    public int? Na { get; set; }

    public int? Npa { get; set; }

    public int? Nsx { get; set; }

    public int? Sel { get; set; }

    public int? Sta { get; set; }

    public int? Tip { get; set; }

    public double? Uhom { get; set; }

    public double? Pg { get; set; }

    public double? Qg { get; set; }

    public double? Pn { get; set; }

    public double? Qn { get; set; }

    public double? Gsh { get; set; }

    public double? Bsh { get; set; }

    public double? Vzd { get; set; }

    public double? Qmax { get; set; }

    public double? Qmin { get; set; }

    public double? Umax { get; set; }

    public double? Umin { get; set; }

    public double? Vras { get; set; }

    public double? Delta { get; set; }

    public double? Otv { get; set; }

    public double? Kct { get; set; }

    public double? PgMax { get; set; }

    public double? PgMin { get; set; }

    public double? PgNom { get; set; }

    public int? Nrk { get; set; }

    public double? Brk { get; set; }

    public double? Grk { get; set; }

    public double? Bshr { get; set; }

    public double? Gshr { get; set; }

    public double? Psh { get; set; }

    public double? Qsh { get; set; }

    public int? StaR { get; set; }

    public double? Pnr { get; set; }

    public double? Qnr { get; set; }

    public double? Pgr { get; set; }

    public double? Qgr { get; set; }

    public double? Nebal { get; set; }

    public double? NebalQ { get; set; }

    public int? Nga { get; set; }

    public double? Dpg { get; set; }

    public double? Dpn { get; set; }

    public double? Dqn { get; set; }

    public int? ContrV { get; set; }

    public int? RegQ { get; set; }

    public double? Lp { get; set; }

    public double? Lq { get; set; }

    public string? NaName { get; set; }

    public double? NaPop { get; set; }

    public int? Epn { get; set; }

    public int? Epg { get; set; }

    public int? Esh { get; set; }

    public int? Ekn { get; set; }

    public string? Ysh { get; set; }

    public string? Sn { get; set; }

    public string? Sg { get; set; }

    public string? Uc { get; set; }

    public string? Ssh { get; set; }

    public string? Qmima { get; set; }

    public string? Umima { get; set; }

    public string? Najact { get; set; }

    public int? NaNo { get; set; }

    public string? Nadjgen { get; set; }

    public int? Numgen { get; set; }

    public string? Snr { get; set; }

    public string? Sgr { get; set; }

    public int? Nf { get; set; }

    public int? Muskod { get; set; }

    public double? Dqmin { get; set; }

    public double? Dqmax { get; set; }

    public double? PnMin { get; set; }

    public double? PnMax { get; set; }

    public double? Sn1 { get; set; }

    public double? Tsn { get; set; }

    public double? TgPhi { get; set; }

    public double? QnMin { get; set; }

    public double? QnMax { get; set; }

    public int? ExistLoad { get; set; }

    public int? ExistGen { get; set; }

    public int? BasePriority { get; set; }

    public int? BaseArea { get; set; }

    public int? NablP { get; set; }

    public int? NablQ { get; set; }

    public int? NablPFrag { get; set; }

    public int? NablQFrag { get; set; }

    public double? PnIzm { get; set; }

    public double? QnIzm { get; set; }

    public double? UIzm { get; set; }

    public double? TiPn { get; set; }

    public double? TiQn { get; set; }

    public double? TiPg { get; set; }

    public double? TiQg { get; set; }

    public string? TiSn { get; set; }

    public string? TiSg { get; set; }

    public double? TiVras { get; set; }

    public double? TiPnDiff { get; set; }

    public double? TiQnDiff { get; set; }

    public double? TiPgDiff { get; set; }

    public double? TiQgDiff { get; set; }

    public double? TiVrasDiff { get; set; }

    public double? TiBalP { get; set; }

    public double? TiBalQ { get; set; }

    public string? TiBalPq { get; set; }

    public string? TiSnDiff { get; set; }

    public string? TiSgDiff { get; set; }

    public int? NySubst { get; set; }

    public int? Supernode { get; set; }

    public int? Rang { get; set; }

    public string? TiOcSn { get; set; }

    public string? TiOcSg { get; set; }

    public string? TiOcVras { get; set; }

    public int? Numload { get; set; }

    public string? Nadjload { get; set; }

    public int? NablPGen { get; set; }

    public virtual ICollection<CurrentLimit> CurrentLimitIpNavigations { get; set; } = new List<CurrentLimit>();

    public virtual ICollection<CurrentLimit> CurrentLimitIqNavigations { get; set; } = new List<CurrentLimit>();

    public virtual ICollection<Generator> Generators { get; set; } = new List<Generator>();

    public virtual ICollection<Load> Loads { get; set; } = new List<Load>();

    public virtual ICollection<Vetv> VetvIpNavigations { get; set; } = new List<Vetv>();

    public virtual ICollection<Vetv> VetvIqNavigations { get; set; } = new List<Vetv>();
}
