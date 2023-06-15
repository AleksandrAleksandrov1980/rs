using System;
using System.Collections.Generic;

namespace wmvc.Models;

public partial class Vetv
{
    public int Ip { get; set; }

    public int Iq { get; set; }

    public int Np { get; set; }

    public int? Sel { get; set; }

    public int? Sta { get; set; }

    public int? Tip { get; set; }

    public double? R { get; set; }

    public double? X { get; set; }

    public double? B { get; set; }

    public double? G { get; set; }

    public double? Ktr { get; set; }

    public double? KrMax { get; set; }

    public double? KrMin { get; set; }

    public int? Bd { get; set; }

    public int? NAnc { get; set; }

    public int? Na { get; set; }

    public double? Div { get; set; }

    public int? Npa { get; set; }

    public double? Div2 { get; set; }

    public int? Nga { get; set; }

    public double? Div3 { get; set; }

    public double? IDop { get; set; }

    public int? Msi { get; set; }

    public double? IMsi { get; set; }

    public double? Kti { get; set; }

    public double? KiMax { get; set; }

    public double? KiMin { get; set; }

    public int? NrIp { get; set; }

    public int? SrIp { get; set; }

    public double? BrIp { get; set; }

    public double? GrIp { get; set; }

    public int? NrIq { get; set; }

    public int? SrIq { get; set; }

    public double? BrIq { get; set; }

    public double? GrIq { get; set; }

    public double? BIp { get; set; }

    public double? BIq { get; set; }

    public double? GIp { get; set; }

    public double? GIq { get; set; }

    public double? PlIp { get; set; }

    public double? QlIp { get; set; }

    public double? PlIq { get; set; }

    public double? QlIq { get; set; }

    public double? VIp { get; set; }

    public double? VIq { get; set; }

    public double? DIp { get; set; }

    public double? DIq { get; set; }

    public double? Dp { get; set; }

    public double? Dq { get; set; }

    public double? Ib { get; set; }

    public double? Ie { get; set; }

    public double? Psh { get; set; }

    public double? Qsh { get; set; }

    public string? Name { get; set; }

    public string? Dname { get; set; }

    public int? RegKt { get; set; }

    public int? ContrI { get; set; }

    public string? Z { get; set; }

    public string? Slb { get; set; }

    public string? Sle { get; set; }

    public int? Zbg { get; set; }

    public int? Zen { get; set; }

    public int? Tmpny { get; set; }

    public string? NameNy { get; set; }

    public int? InNy { get; set; }

    public double? PlNy { get; set; }

    public double? QlNy { get; set; }

    public double? INy { get; set; }

    public string? NaNy { get; set; }

    public string? NaName { get; set; }

    public double? NaPl { get; set; }

    public int? NaNa { get; set; }

    public double? NaDp { get; set; }

    public double? ZagI { get; set; }

    public double? ZagIt { get; set; }

    public double? NaDv { get; set; }

    public double? KtB { get; set; }

    public double? Dv { get; set; }

    public double? Dij { get; set; }

    public double? DvNy { get; set; }

    public double? DijNy { get; set; }

    public int? NpaNa { get; set; }

    public string? NpaNy { get; set; }

    public string? NpaName { get; set; }

    public double? NpaPl { get; set; }

    public double? NpaDp { get; set; }

    public double? NpaDv { get; set; }

    public double? Plmax { get; set; }

    public string? Slmax { get; set; }

    public int? Sup { get; set; }

    public string? Sup2 { get; set; }

    public int? SignP { get; set; }

    public int? SignQip { get; set; }

    public int? SignQiq { get; set; }

    public double? V2Ny { get; set; }

    public double? PlBal { get; set; }

    public double? IDopR { get; set; }

    public int? NIt { get; set; }

    public double? Tc { get; set; }

    public double? IDopOb { get; set; }

    public int? Groupid { get; set; }

    public int? NAncI { get; set; }

    public int? Ta { get; set; }

    public double? MaxDd { get; set; }

    public double? MaxDv { get; set; }

    public int? IsBreaker { get; set; }

    public double? IMax { get; set; }

    public double? IZag { get; set; }

    public double? DivNga { get; set; }

    public int? NgaNa { get; set; }

    public string? NgaNy { get; set; }

    public string? NgaName { get; set; }

    public double? NgaPl { get; set; }

    public double? NgaDp { get; set; }

    public double? NgaDv { get; set; }

    public double? TiPlIp { get; set; }

    public double? TiPlIq { get; set; }

    public double? TiQlIp { get; set; }

    public double? TiQlIq { get; set; }

    public double? TiV2Ny { get; set; }

    public double? TiPlIpNy { get; set; }

    public double? TiQlIpNy { get; set; }

    public double? TiPlIqNy { get; set; }

    public double? TiQlIqNy { get; set; }

    public double? TiPlIpNyDiff { get; set; }

    public double? TiQlIpNyDiff { get; set; }

    public double? TiPlIqNyDiff { get; set; }

    public double? TiQlIqNyDiff { get; set; }

    public double? PlIpNy { get; set; }

    public double? QlIpNy { get; set; }

    public double? PlIqNy { get; set; }

    public double? QlIqNy { get; set; }

    public string? TiSlb { get; set; }

    public string? TiSle { get; set; }

    public double? TiVrasDiff { get; set; }

    public int? Brand { get; set; }

    public double? L { get; set; }

    public int? NItAv { get; set; }

    public double? IDopAv { get; set; }

    public double? IDopObAv { get; set; }

    public double? IDopRAv { get; set; }

    public double? ZagIAv { get; set; }

    public string? TiSlbDiff { get; set; }

    public string? TiSleDiff { get; set; }

    public double? L2 { get; set; }

    public double? L3 { get; set; }

    public int? Brand2 { get; set; }

    public int? Brand3 { get; set; }

    public double? ZagItAv { get; set; }

    public int? MeteoIdIp { get; set; }

    public int? MeteoIdIq { get; set; }

    public string? StrMeteoIdIp { get; set; }

    public string? StrMeteoIdIq { get; set; }

    public int? SupernodeIp { get; set; }

    public int? SupernodeIq { get; set; }

    public double? Lsum { get; set; }

    public string? TiOcPlIpNy { get; set; }

    public string? TiOcPlIqNy { get; set; }

    public string? TiOcQlIpNy { get; set; }

    public string? TiOcQlIqNy { get; set; }

    public string? TiOcSlb { get; set; }

    public string? TiOcSle { get; set; }

    public virtual Ancapf? BdNavigation { get; set; }

    public virtual Node IpNavigation { get; set; } = null!;

    public virtual Node IqNavigation { get; set; } = null!;
}
