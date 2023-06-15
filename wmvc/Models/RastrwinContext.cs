using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace wmvc.Models;

public partial class RastrwinContext : DbContext
{
    public RastrwinContext()
    {
    }

    public RastrwinContext(DbContextOptions<RastrwinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdvgridForm> AdvgridForms { get; set; }

    public virtual DbSet<Altunit> Altunits { get; set; }

    public virtual DbSet<Ancapf> Ancapfs { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Area2> Area2s { get; set; }

    public virtual DbSet<Background> Backgrounds { get; set; }

    public virtual DbSet<Background2> Background2s { get; set; }

    public virtual DbSet<Background3> Background3s { get; set; }

    public virtual DbSet<Background4> Background4s { get; set; }

    public virtual DbSet<Chartdesc> Chartdescs { get; set; }

    public virtual DbSet<ComCxema> ComCxemas { get; set; }

    public virtual DbSet<ComEkviv> ComEkvivs { get; set; }

    public virtual DbSet<ComOpf> ComOpfs { get; set; }

    public virtual DbSet<ComOptim> ComOptims { get; set; }

    public virtual DbSet<ComRegim> ComRegims { get; set; }

    public virtual DbSet<ComTi> ComTis { get; set; }

    public virtual DbSet<Consumer> Consumers { get; set; }

    public virtual DbSet<CurrentLimit> CurrentLimits { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Darea> Dareas { get; set; }

    public virtual DbSet<Dclink> Dclinks { get; set; }

    public virtual DbSet<Dfwchartcontent> Dfwchartcontents { get; set; }

    public virtual DbSet<Dfwchartdir> Dfwchartdirs { get; set; }

    public virtual DbSet<Diff> Diffs { get; set; }

    public virtual DbSet<ExportXml> ExportXmls { get; set; }

    public virtual DbSet<Fact> Facts { get; set; }

    public virtual DbSet<Formcontext> Formcontexts { get; set; }

    public virtual DbSet<Funcoverk> Funcoverks { get; set; }

    public virtual DbSet<Generator> Generators { get; set; }

    public virtual DbSet<Gou> Gous { get; set; }

    public virtual DbSet<Graphik2> Graphik2s { get; set; }

    public virtual DbSet<Graphikit> Graphikits { get; set; }

    public virtual DbSet<GraphikitSource> GraphikitSources { get; set; }

    public virtual DbSet<Guid> Guids { get; set; }

    public virtual DbSet<Interarea> Interareas { get; set; }

    public virtual DbSet<Island> Islands { get; set; }

    public virtual DbSet<KocBaseRg> KocBaseRgs { get; set; }

    public virtual DbSet<KocConst> KocConsts { get; set; }

    public virtual DbSet<KocKweight> KocKweights { get; set; }

    public virtual DbSet<KocParam> KocParams { get; set; }

    public virtual DbSet<KocUogr> KocUogrs { get; set; }

    public virtual DbSet<KurConst> KurConsts { get; set; }

    public virtual DbSet<KurParam> KurParams { get; set; }

    public virtual DbSet<Load> Loads { get; set; }

    public virtual DbSet<Macrocontext> Macrocontexts { get; set; }

    public virtual DbSet<Meteo> Meteos { get; set; }

    public virtual DbSet<Nagr> Nagrs { get; set; }

    public virtual DbSet<Ngroup> Ngroups { get; set; }

    public virtual DbSet<Node> Nodes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Polin> Polins { get; set; }

    public virtual DbSet<Poteri> Poteris { get; set; }

    public virtual DbSet<Reactor> Reactors { get; set; }

    public virtual DbSet<Sendcommandmainform> Sendcommandmainforms { get; set; }

    public virtual DbSet<SqlSetting> SqlSettings { get; set; }

    public virtual DbSet<Ti> Tis { get; set; }

    public virtual DbSet<TiBalansP> TiBalansPs { get; set; }

    public virtual DbSet<TiBalansQ> TiBalansQs { get; set; }

    public virtual DbSet<TiPrv> TiPrvs { get; set; }

    public virtual DbSet<TiSetting> TiSettings { get; set; }

    public virtual DbSet<Tree> Trees { get; set; }

    public virtual DbSet<Treelevel> Treelevels { get; set; }

    public virtual DbSet<Ushr> Ushrs { get; set; }

    public virtual DbSet<Vetv> Vetvs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.1.84;Database=rastrwin;Username=postgres;Password=pgadmin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdvgridForm>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("advgrid_form_pkey");

            entity.ToTable("advgrid_form");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.FormName).HasColumnType("character varying");
            entity.Property(e => e.SubForm).HasColumnType("character varying");
        });

        modelBuilder.Entity<Altunit>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("altunit_pkey");

            entity.ToTable("altunit");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Alt).HasColumnType("character varying");
            entity.Property(e => e.Formula).HasColumnType("character varying");
            entity.Property(e => e.Tabl).HasColumnType("character varying");
            entity.Property(e => e.Unit).HasColumnType("character varying");
        });

        modelBuilder.Entity<Ancapf>(entity =>
        {
            entity.HasKey(e => e.Nbd).HasName("ancapf_pkey");

            entity.ToTable("ancapf");

            entity.Property(e => e.Nbd)
                .ValueGeneratedNever()
                .HasColumnName("nbd");
            entity.Property(e => e.Ei).HasColumnName("ei");
            entity.Property(e => e.Kne).HasColumnName("kne");
            entity.Property(e => e.KtMax).HasColumnName("kt_max");
            entity.Property(e => e.KtMid).HasColumnName("kt_mid");
            entity.Property(e => e.KtMin).HasColumnName("kt_min");
            entity.Property(e => e.Mesto).HasColumnName("mesto");
            entity.Property(e => e.NAnc1).HasColumnName("n_anc1");
            entity.Property(e => e.NAnc10).HasColumnName("n_anc10");
            entity.Property(e => e.NAnc2).HasColumnName("n_anc2");
            entity.Property(e => e.NAnc3).HasColumnName("n_anc3");
            entity.Property(e => e.NAnc4).HasColumnName("n_anc4");
            entity.Property(e => e.NAnc5).HasColumnName("n_anc5");
            entity.Property(e => e.NAnc6).HasColumnName("n_anc6");
            entity.Property(e => e.NAnc7).HasColumnName("n_anc7");
            entity.Property(e => e.NAnc8).HasColumnName("n_anc8");
            entity.Property(e => e.NAnc9).HasColumnName("n_anc9");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Pm).HasColumnName("pm");
            entity.Property(e => e.Sh1).HasColumnName("sh1");
            entity.Property(e => e.Sh10).HasColumnName("sh10");
            entity.Property(e => e.Sh2).HasColumnName("sh2");
            entity.Property(e => e.Sh3).HasColumnName("sh3");
            entity.Property(e => e.Sh4).HasColumnName("sh4");
            entity.Property(e => e.Sh5).HasColumnName("sh5");
            entity.Property(e => e.Sh6).HasColumnName("sh6");
            entity.Property(e => e.Sh7).HasColumnName("sh7");
            entity.Property(e => e.Sh8).HasColumnName("sh8");
            entity.Property(e => e.Sh9).HasColumnName("sh9");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Vnr).HasColumnName("vnr");
            entity.Property(e => e.Vreg).HasColumnName("vreg");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Na).HasName("area_pkey");

            entity.ToTable("area");

            entity.Property(e => e.Na)
                .ValueGeneratedNever()
                .HasColumnName("na");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.DpLine).HasColumnName("dp_line");
            entity.Property(e => e.DpNag).HasColumnName("dp_nag");
            entity.Property(e => e.DpShunt).HasColumnName("dp_shunt");
            entity.Property(e => e.DpTran).HasColumnName("dp_tran");
            entity.Property(e => e.DpXx).HasColumnName("dp_xx");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.DqLine).HasColumnName("dq_line");
            entity.Property(e => e.DqNag).HasColumnName("dq_nag");
            entity.Property(e => e.DqShunt).HasColumnName("dq_shunt");
            entity.Property(e => e.DqTran).HasColumnName("dq_tran");
            entity.Property(e => e.DqXx).HasColumnName("dq_xx");
            entity.Property(e => e.Ekn).HasColumnName("_ekn");
            entity.Property(e => e.Epg).HasColumnName("_epg");
            entity.Property(e => e.Epn).HasColumnName("_epn");
            entity.Property(e => e.Esh).HasColumnName("_esh");
            entity.Property(e => e.Kpodpg).HasColumnName("kpodpg");
            entity.Property(e => e.Kpodpn).HasColumnName("kpodpn");
            entity.Property(e => e.NaArea)
                .HasColumnType("character varying")
                .HasColumnName("_na_area");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Navet)
                .HasColumnType("character varying")
                .HasColumnName("_navet");
            entity.Property(e => e.Nng).HasColumnName("nng");
            entity.Property(e => e.No).HasColumnName("no");
            entity.Property(e => e.Npgen).HasColumnName("NPgen");
            entity.Property(e => e.Npnag).HasColumnName("NPnag");
            entity.Property(e => e.Pfull).HasColumnName("pfull");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.PgMax).HasColumnName("pg_max");
            entity.Property(e => e.PgMin).HasColumnName("pg_min");
            entity.Property(e => e.PgZb).HasColumnName("pg_zb");
            entity.Property(e => e.Pn).HasColumnName("pn");
            entity.Property(e => e.PnMax).HasColumnName("pn_max");
            entity.Property(e => e.PnMin).HasColumnName("pn_min");
            entity.Property(e => e.PnZb).HasColumnName("pn_zb");
            entity.Property(e => e.Pop).HasColumnName("pop");
            entity.Property(e => e.PopCmp).HasColumnName("pop_cmp");
            entity.Property(e => e.PopCmpD).HasColumnName("pop_cmp_d");
            entity.Property(e => e.Poq).HasColumnName("poq");
            entity.Property(e => e.Qg).HasColumnName("qg");
            entity.Property(e => e.Qn).HasColumnName("qn");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.ShLine).HasColumnName("sh_line");
            entity.Property(e => e.ShTran).HasColumnName("sh_tran");
            entity.Property(e => e.ShqLine).HasColumnName("shq_line");
            entity.Property(e => e.ShqTran).HasColumnName("shq_tran");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.TiPg).HasColumnName("ti_pg");
            entity.Property(e => e.TiPn).HasColumnName("ti_pn");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Tmpun)
                .HasColumnType("character varying")
                .HasColumnName("_tmpun");
            entity.Property(e => e.Tpos).HasColumnName("tpos");
            entity.Property(e => e.Un).HasColumnName("_un");
            entity.Property(e => e.Vnp).HasColumnName("vnp");
            entity.Property(e => e.Vnq).HasColumnName("vnq");
        });

        modelBuilder.Entity<Area2>(entity =>
        {
            entity.HasKey(e => e.Npa).HasName("area2_pkey");

            entity.ToTable("area2");

            entity.Property(e => e.Npa)
                .ValueGeneratedNever()
                .HasColumnName("npa");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.NaNpa)
                .HasColumnType("character varying")
                .HasColumnName("_na_npa");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Navet)
                .HasColumnType("character varying")
                .HasColumnName("_navet");
            entity.Property(e => e.Pfull).HasColumnName("pfull");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.Pn).HasColumnName("pn");
            entity.Property(e => e.Pop).HasColumnName("pop");
            entity.Property(e => e.Poq).HasColumnName("poq");
            entity.Property(e => e.Qg).HasColumnName("qg");
            entity.Property(e => e.Qn).HasColumnName("qn");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Tpos).HasColumnName("tpos");
            entity.Property(e => e.Vnp).HasColumnName("vnp");
            entity.Property(e => e.Vnq).HasColumnName("vnq");
            entity.Property(e => e.X2)
                .HasColumnType("character varying")
                .HasColumnName("_x2");
        });

        modelBuilder.Entity<Background>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("background_pkey");

            entity.ToTable("background");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Byvib).HasColumnName("byvib");
            entity.Property(e => e.Col)
                .HasColumnType("character varying")
                .HasColumnName("col");
            entity.Property(e => e.Gradstr)
                .HasColumnType("character varying")
                .HasColumnName("gradstr");
            entity.Property(e => e.Onoff).HasColumnName("onoff");
            entity.Property(e => e.Tabl)
                .HasColumnType("character varying")
                .HasColumnName("tabl");
            entity.Property(e => e.Vibor)
                .HasColumnType("character varying")
                .HasColumnName("vibor");
        });

        modelBuilder.Entity<Background2>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("background2_pkey");

            entity.ToTable("background2");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Defcolor).HasColumnName("defcolor");
            entity.Property(e => e.Tabl)
                .HasColumnType("character varying")
                .HasColumnName("tabl");
            entity.Property(e => e.Vibor)
                .HasColumnType("character varying")
                .HasColumnName("vibor");
        });

        modelBuilder.Entity<Background3>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("background3_pkey");

            entity.ToTable("background3");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Defcolor).HasColumnName("defcolor");
            entity.Property(e => e.Tabl)
                .HasColumnType("character varying")
                .HasColumnName("tabl");
            entity.Property(e => e.Vib)
                .HasColumnType("character varying")
                .HasColumnName("vib");
        });

        modelBuilder.Entity<Background4>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("background4_pkey");

            entity.ToTable("background4");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Col)
                .HasColumnType("character varying")
                .HasColumnName("col");
            entity.Property(e => e.Expr)
                .HasColumnType("character varying")
                .HasColumnName("expr");
            entity.Property(e => e.Gradstr)
                .HasColumnType("character varying")
                .HasColumnName("gradstr");
            entity.Property(e => e.Isgrad).HasColumnName("isgrad");
            entity.Property(e => e.Onoff).HasColumnName("onoff");
            entity.Property(e => e.Tabl)
                .HasColumnType("character varying")
                .HasColumnName("tabl");
        });

        modelBuilder.Entity<Chartdesc>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("chartdescs_pkey");

            entity.ToTable("chartdescs");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.BaseKeyFields).HasColumnType("character varying");
            entity.Property(e => e.BaseKeyValues).HasColumnType("character varying");
            entity.Property(e => e.BaseTable).HasColumnType("character varying");
            entity.Property(e => e.ChannelNameFields).HasColumnType("character varying");
            entity.Property(e => e.ChannelNameTemplate).HasColumnType("character varying");
            entity.Property(e => e.ChartWindow).HasColumnType("character varying");
            entity.Property(e => e.TemplateTags).HasColumnType("character varying");
            entity.Property(e => e.Xalgorithm).HasColumnName("XAlgorithm");
            entity.Property(e => e.Xfields)
                .HasColumnType("character varying")
                .HasColumnName("XFields");
            entity.Property(e => e.Xselect)
                .HasColumnType("character varying")
                .HasColumnName("XSelect");
            entity.Property(e => e.XselectFields)
                .HasColumnType("character varying")
                .HasColumnName("XSelectFields");
            entity.Property(e => e.Xtable)
                .HasColumnType("character varying")
                .HasColumnName("XTable");
            entity.Property(e => e.Y2xselectFields)
                .HasColumnType("character varying")
                .HasColumnName("Y2XSelectFields");
            entity.Property(e => e.Y2xselectTemplate)
                .HasColumnType("character varying")
                .HasColumnName("Y2XSelectTemplate");
            entity.Property(e => e.Yalgorithm).HasColumnName("YAlgorithm");
            entity.Property(e => e.Yfields)
                .HasColumnType("character varying")
                .HasColumnName("YFields");
            entity.Property(e => e.Yselect)
                .HasColumnType("character varying")
                .HasColumnName("YSelect");
            entity.Property(e => e.YselectFields)
                .HasColumnType("character varying")
                .HasColumnName("YSelectFields");
            entity.Property(e => e.Ytable)
                .HasColumnType("character varying")
                .HasColumnName("YTable");
        });

        modelBuilder.Entity<ComCxema>(entity =>
        {
            entity.HasKey(e => e.Nn).HasName("com_cxema_pkey");

            entity.ToTable("com_cxema");

            entity.Property(e => e.Nn)
                .ValueGeneratedNever()
                .HasColumnName("nn");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.Dpsh).HasColumnName("dpsh");
            entity.Property(e => e.DvMax).HasColumnName("dv_max");
            entity.Property(e => e.DvMin).HasColumnName("dv_min");
            entity.Property(e => e.MaxIl).HasColumnName("max_il");
            entity.Property(e => e.MaxIt).HasColumnName("max_it");
            entity.Property(e => e.Na).HasColumnName("na");
            entity.Property(e => e.Nby).HasColumnName("nby");
            entity.Property(e => e.Ngen).HasColumnName("ngen");
            entity.Property(e => e.NlNax).HasColumnName("nl_nax");
            entity.Property(e => e.Nlep).HasColumnName("nlep");
            entity.Property(e => e.NtMax).HasColumnName("nt_max");
            entity.Property(e => e.Ntran).HasColumnName("ntran");
            entity.Property(e => e.Nv).HasColumnName("nv");
            entity.Property(e => e.NvO).HasColumnName("nv_o");
            entity.Property(e => e.Nvikl).HasColumnName("nvikl");
            entity.Property(e => e.Ny).HasColumnName("ny");
            entity.Property(e => e.NyO).HasColumnName("ny_o");
            entity.Property(e => e.Pby).HasColumnName("pby");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.Pn).HasColumnName("pn");
        });

        modelBuilder.Entity<ComEkviv>(entity =>
        {
            entity.HasKey(e => e.Nra).HasName("com_ekviv_pkey");

            entity.ToTable("com_ekviv");

            entity.Property(e => e.Nra)
                .ValueGeneratedNever()
                .HasColumnName("nra");
            entity.Property(e => e.EkSh).HasColumnName("ek_sh");
            entity.Property(e => e.Ekvgen).HasColumnName("ekvgen");
            entity.Property(e => e.KfcX).HasColumnName("kfc_x");
            entity.Property(e => e.Kpg).HasColumnName("kpg");
            entity.Property(e => e.Kpn).HasColumnName("kpn");
            entity.Property(e => e.MetEkv).HasColumnName("met_ekv");
            entity.Property(e => e.OtmN).HasColumnName("otm_n");
            entity.Property(e => e.PotGen).HasColumnName("pot_gen");
            entity.Property(e => e.Selekv).HasColumnName("selekv");
            entity.Property(e => e.Smart).HasColumnName("smart");
            entity.Property(e => e.TipEkv).HasColumnName("tip_ekv");
            entity.Property(e => e.TipGen).HasColumnName("tip_gen");
            entity.Property(e => e.TipSxn).HasColumnName("tip_sxn");
            entity.Property(e => e.Zmax).HasColumnName("zmax");
        });

        modelBuilder.Entity<ComOpf>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("com_opf_pkey");

            entity.ToTable("com_opf");

            entity.Property(e => e.Num)
                .ValueGeneratedNever()
                .HasColumnName("num");
            entity.Property(e => e.Centr).HasColumnName("centr");
            entity.Property(e => e.Criteria1).HasColumnName("criteria1");
            entity.Property(e => e.Formula1)
                .HasColumnType("character varying")
                .HasColumnName("formula1");
            entity.Property(e => e.ItMax).HasColumnName("it_max");
            entity.Property(e => e.LdpgDta).HasColumnName("LDPG_dta");
            entity.Property(e => e.LdpgMax).HasColumnName("LDPG_max");
            entity.Property(e => e.LdqDta).HasColumnName("LDQ_dta");
            entity.Property(e => e.LdqMax).HasColumnName("LDQ_max");
            entity.Property(e => e.LiDta).HasColumnName("LI_dta");
            entity.Property(e => e.LiMax).HasColumnName("LI_max");
            entity.Property(e => e.LioDta).HasColumnName("LIO_dta");
            entity.Property(e => e.LioMax).HasColumnName("LIO_max");
            entity.Property(e => e.LktDta).HasColumnName("LKT_dta");
            entity.Property(e => e.LktMax).HasColumnName("LKT_max");
            entity.Property(e => e.LpDta).HasColumnName("LP_dta");
            entity.Property(e => e.LpMax).HasColumnName("LP_max");
            entity.Property(e => e.LpsDta).HasColumnName("LPS_dta");
            entity.Property(e => e.LpsMax).HasColumnName("LPS_max");
            entity.Property(e => e.LqDta).HasColumnName("LQ_dta");
            entity.Property(e => e.LqMax).HasColumnName("LQ_max");
            entity.Property(e => e.LvDta).HasColumnName("LV_dta");
            entity.Property(e => e.LvMax).HasColumnName("LV_max");
            entity.Property(e => e.MainMethod).HasColumnName("main_method");
            entity.Property(e => e.MainSigma).HasColumnName("main_sigma");
            entity.Property(e => e.MinMu).HasColumnName("min_mu");
            entity.Property(e => e.MinMu2).HasColumnName("min_mu2");
            entity.Property(e => e.OutLevel).HasColumnName("out_level");
            entity.Property(e => e.Plos).HasColumnName("plos");
            entity.Property(e => e.Potr).HasColumnName("potr");
            entity.Property(e => e.RIt).HasColumnName("R_it");
            entity.Property(e => e.RPdad).HasColumnName("R_pdad");
            entity.Property(e => e.RPdadAf).HasColumnName("R_pdad_af");
            entity.Property(e => e.RRo).HasColumnName("R_ro");
            entity.Property(e => e.RSigma).HasColumnName("R_sigma");
            entity.Property(e => e.StartMethod).HasColumnName("start_method");
            entity.Property(e => e.StartSigma).HasColumnName("start_sigma");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TFunc).HasColumnName("T_func");
            entity.Property(e => e.Tarif).HasColumnName("tarif");
            entity.Property(e => e.TgFunc).HasColumnName("Tg_func");
            entity.Property(e => e.Tipr).HasColumnName("tipr");
        });

        modelBuilder.Entity<ComOptim>(entity =>
        {
            entity.HasKey(e => e.Nopt).HasName("com_optim_pkey");

            entity.ToTable("com_optim");

            entity.Property(e => e.Nopt)
                .ValueGeneratedNever()
                .HasColumnName("nopt");
            entity.Property(e => e.Anc).HasColumnName("anc");
            entity.Property(e => e.Divider).HasColumnName("divider");
            entity.Property(e => e.DpotMax).HasColumnName("dpot_max");
            entity.Property(e => e.DshtrMax).HasColumnName("dshtr_max");
            entity.Property(e => e.IrmRegul).HasColumnName("irm_regul");
            entity.Property(e => e.IterBasis).HasColumnName("iter_basis");
            entity.Property(e => e.IterMax).HasColumnName("iter_max");
            entity.Property(e => e.IterMm).HasColumnName("iter_mm");
            entity.Property(e => e.KoefKt).HasColumnName("koef_kt");
            entity.Property(e => e.KoefShtr).HasColumnName("koef_shtr");
            entity.Property(e => e.MinPot).HasColumnName("min_pot");
            entity.Property(e => e.Regul).HasColumnName("regul");
            entity.Property(e => e.Ud).HasColumnName("ud");
        });

        modelBuilder.Entity<ComRegim>(entity =>
        {
            entity.HasKey(e => e.Nrge).HasName("com_regim_pkey");

            entity.ToTable("com_regim");

            entity.Property(e => e.Nrge)
                .ValueGeneratedNever()
                .HasColumnName("nrge");
            entity.Property(e => e.CalcTr).HasColumnName("calc_tr");
            entity.Property(e => e.CtrlBaza).HasColumnName("ctrl_baza");
            entity.Property(e => e.DdMax).HasColumnName("dd_max");
            entity.Property(e => e.DvMax).HasColumnName("dv_max");
            entity.Property(e => e.DvMin).HasColumnName("dv_min");
            entity.Property(e => e.Flot).HasColumnName("flot");
            entity.Property(e => e.Fwc).HasColumnName("fwc");
            entity.Property(e => e.GenP).HasColumnName("gen_p");
            entity.Property(e => e.Gram).HasColumnName("gram");
            entity.Property(e => e.It).HasColumnName("it");
            entity.Property(e => e.ItMax).HasColumnName("it_max");
            entity.Property(e => e.Itz).HasColumnName("itz");
            entity.Property(e => e.ItzOgrMax).HasColumnName("itz_ogr_max");
            entity.Property(e => e.ItzOgrMin).HasColumnName("itz_ogr_min");
            entity.Property(e => e.Kfd).HasColumnName("kfd");
            entity.Property(e => e.Kima).HasColumnName("kima");
            entity.Property(e => e.LoadP).HasColumnName("load_p");
            entity.Property(e => e.MaxSlpSxn).HasColumnName("max_slp_sxn");
            entity.Property(e => e.Method).HasColumnName("method");
            entity.Property(e => e.MethodOgr).HasColumnName("method_ogr");
            entity.Property(e => e.MinNodesInIsland).HasColumnName("min_nodes_in_island");
            entity.Property(e => e.MinSlpSxn).HasColumnName("min_slp_sxn");
            entity.Property(e => e.MinX).HasColumnName("min_x");
            entity.Property(e => e.NagP).HasColumnName("nag_p");
            entity.Property(e => e.NebP).HasColumnName("neb_p");
            entity.Property(e => e.NebQ).HasColumnName("neb_q");
            entity.Property(e => e.PrintMode).HasColumnName("print_mode");
            entity.Property(e => e.QMmPnom).HasColumnName("q_mm_pnom");
            entity.Property(e => e.Qmax).HasColumnName("qmax");
            entity.Property(e => e.RemBreaker).HasColumnName("rem_breaker");
            entity.Property(e => e.Rr).HasColumnName("rr");
            entity.Property(e => e.Start).HasColumnName("start");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdateSta).HasColumnName("update_sta");
            entity.Property(e => e.Wt).HasColumnName("wt");
        });

        modelBuilder.Entity<ComTi>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("com_ti_pkey");

            entity.ToTable("com_ti");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.ActiveFiltr).HasColumnName("active_filtr");
            entity.Property(e => e.ActiveFiltrTopology).HasColumnName("active_filtr_topology");
            entity.Property(e => e.AddUnknownTi).HasColumnName("add_unknown_ti");
            entity.Property(e => e.AddUnknownTs).HasColumnName("add_unknown_ts");
            entity.Property(e => e.CreatePtiUOnlyGen).HasColumnName("createPTI_U_onlyGen");
            entity.Property(e => e.DtiSource).HasColumnName("dti_source");
            entity.Property(e => e.DtsSource).HasColumnName("dts_source");
            entity.Property(e => e.FiltrTi)
                .HasColumnType("character varying")
                .HasColumnName("Filtr_TI");
            entity.Property(e => e.GenModel).HasColumnName("gen_model");
            entity.Property(e => e.Oik).HasColumnName("oik");
            entity.Property(e => e.PtiAddType).HasColumnName("PTI_add_type");
            entity.Property(e => e.PtiCalcType).HasColumnName("PTI_calc_type");
            entity.Property(e => e.PtiSelArea).HasColumnName("PTI_sel_area");
            entity.Property(e => e.RewriteTi).HasColumnName("rewrite_ti");
            entity.Property(e => e.SensErrorMwt).HasColumnName("sens_error_mwt");
            entity.Property(e => e.SensErrorProc).HasColumnName("sens_error_proc");
            entity.Property(e => e.SensErrorUProc).HasColumnName("sens_error_u_proc");
            entity.Property(e => e.SensWrongMwt).HasColumnName("sens_wrong_mwt");
            entity.Property(e => e.SensWrongProc).HasColumnName("sens_wrong_proc");
            entity.Property(e => e.ShowFiltrForm).HasColumnName("show_filtr_form");
            entity.Property(e => e.TiSource).HasColumnName("ti_source");
            entity.Property(e => e.TsSource).HasColumnName("ts_source");
            entity.Property(e => e.UpdateSta).HasColumnName("update_sta");
            entity.Property(e => e.UpdateStaNode).HasColumnName("update_sta_node");
        });

        modelBuilder.Entity<Consumer>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("consumer_pkey");

            entity.ToTable("consumer");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<CurrentLimit>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("current_limit_pkey");

            entity.ToTable("current_limit");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ElType).HasColumnName("el_type");
            entity.Property(e => e.IDop).HasColumnName("i_dop");
            entity.Property(e => e.IDopR).HasColumnName("i_dop_r");
            entity.Property(e => e.Ib).HasColumnName("ib");
            entity.Property(e => e.Ie).HasColumnName("ie");
            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.Iq).HasColumnName("iq");
            entity.Property(e => e.Msi).HasColumnName("msi");
            entity.Property(e => e.NIt).HasColumnName("n_it");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Np).HasColumnName("np");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UIp).HasColumnName("u_ip");
            entity.Property(e => e.UIq).HasColumnName("u_iq");
            entity.Property(e => e.VetvName)
                .HasColumnType("character varying")
                .HasColumnName("vetv_name");
            entity.Property(e => e.ZagI).HasColumnName("zag_i");

            entity.HasOne(d => d.IpNavigation).WithMany(p => p.CurrentLimitIpNavigations)
                .HasForeignKey(d => d.Ip)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("current_limit_ip_fkey");

            entity.HasOne(d => d.IqNavigation).WithMany(p => p.CurrentLimitIqNavigations)
                .HasForeignKey(d => d.Iq)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("current_limit_iq_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(30)
                .HasColumnName("lastname");
        });

        modelBuilder.Entity<Darea>(entity =>
        {
            entity.HasKey(e => e.No).HasName("darea_pkey");

            entity.ToTable("darea");

            entity.Property(e => e.No)
                .ValueGeneratedNever()
                .HasColumnName("no");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.DpLine).HasColumnName("dp_line");
            entity.Property(e => e.DpNagr).HasColumnName("dp_nagr");
            entity.Property(e => e.DpTran).HasColumnName("dp_tran");
            entity.Property(e => e.DpXx).HasColumnName("dp_xx");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.DqLine).HasColumnName("dq_line");
            entity.Property(e => e.DqNagr).HasColumnName("dq_nagr");
            entity.Property(e => e.DqTran).HasColumnName("dq_tran");
            entity.Property(e => e.DqXx).HasColumnName("dq_xx");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.Pp).HasColumnName("pp");
            entity.Property(e => e.Pvn).HasColumnName("pvn");
            entity.Property(e => e.Qg).HasColumnName("qg");
            entity.Property(e => e.Qp).HasColumnName("qp");
            entity.Property(e => e.Qvn).HasColumnName("qvn");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.ShLine).HasColumnName("sh_line");
            entity.Property(e => e.ShShunt).HasColumnName("sh_shunt");
            entity.Property(e => e.ShTran).HasColumnName("sh_tran");
            entity.Property(e => e.ShqLine).HasColumnName("shq_line");
            entity.Property(e => e.ShqShunt).HasColumnName("shq_shunt");
            entity.Property(e => e.ShqTran).HasColumnName("shq_tran");
            entity.Property(e => e.Tmp2)
                .HasColumnType("character varying")
                .HasColumnName("_tmp2");
            entity.Property(e => e.Tmpn)
                .HasColumnType("character varying")
                .HasColumnName("_tmpn");
            entity.Property(e => e.Tmpun)
                .HasColumnType("character varying")
                .HasColumnName("_tmpun");
        });

        modelBuilder.Entity<Dclink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dclink_pkey");

            entity.ToTable("dclink");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AiR).HasColumnName("Ai_r");
            entity.Property(e => e.ArR).HasColumnName("Ar_r");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.PI).HasColumnName("P_i");
            entity.Property(e => e.PR).HasColumnName("P_r");
            entity.Property(e => e.QI).HasColumnName("Q_i");
            entity.Property(e => e.QR).HasColumnName("Q_r");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.TypeVt).HasColumnName("TypeVT");
            entity.Property(e => e.UdI).HasColumnName("Ud_i");
            entity.Property(e => e.UdR).HasColumnName("Ud_r");
            entity.Property(e => e.UvI).HasColumnName("Uv_i");
            entity.Property(e => e.UvR).HasColumnName("Uv_r");
        });

        modelBuilder.Entity<Dfwchartcontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dfwchartcontent_pkey");

            entity.ToTable("dfwchartcontent");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Br).HasColumnName("br");
            entity.Property(e => e.Dt).HasColumnName("dt");
            entity.Property(e => e.Wd).HasColumnName("wd");
        });

        modelBuilder.Entity<Dfwchartdir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dfwchartdir_pkey");

            entity.ToTable("dfwchartdir");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Diff>(entity =>
        {
            entity.HasKey(e => e.DiffIt).HasName("diff_pkey");

            entity.ToTable("diff");

            entity.Property(e => e.DiffIt)
                .ValueGeneratedNever()
                .HasColumnName("diff_it");
            entity.Property(e => e.Addit)
                .HasColumnType("character varying")
                .HasColumnName("addit");
            entity.Property(e => e.Col)
                .HasColumnType("character varying")
                .HasColumnName("col");
            entity.Property(e => e.Tabl)
                .HasColumnType("character varying")
                .HasColumnName("tabl");
            entity.Property(e => e.Vib)
                .HasColumnType("character varying")
                .HasColumnName("vib");
        });

        modelBuilder.Entity<ExportXml>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("export_xml_pkey");

            entity.ToTable("export_xml");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Level1)
                .HasColumnType("character varying")
                .HasColumnName("Level_1");
            entity.Property(e => e.Level2)
                .HasColumnType("character varying")
                .HasColumnName("Level_2");
            entity.Property(e => e.Level3)
                .HasColumnType("character varying")
                .HasColumnName("Level_3");
            entity.Property(e => e.Rname1)
                .HasColumnType("character varying")
                .HasColumnName("rname_1");
            entity.Property(e => e.Rname2)
                .HasColumnType("character varying")
                .HasColumnName("rname_2");
            entity.Property(e => e.Rname3)
                .HasColumnType("character varying")
                .HasColumnName("rname_3");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.SubLevel1)
                .HasColumnType("character varying")
                .HasColumnName("SubLevel_1");
            entity.Property(e => e.SubLevel2)
                .HasColumnType("character varying")
                .HasColumnName("SubLevel_2");
            entity.Property(e => e.SubLevel3)
                .HasColumnType("character varying")
                .HasColumnName("SubLevel_3");
        });

        modelBuilder.Entity<Fact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("facts_pkey");

            entity.ToTable("facts");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Dcnode).HasColumnName("DCnode");
            entity.Property(e => e.Izm).HasColumnName("izm");
            entity.Property(e => e.Mode).HasColumnName("mode");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Tref1).HasColumnName("tref1");
            entity.Property(e => e.Tref2).HasColumnName("tref2");
        });

        modelBuilder.Entity<Formcontext>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("formcontext_pkey");

            entity.ToTable("formcontext");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Bind)
                .HasColumnType("character varying")
                .HasColumnName("bind");
            entity.Property(e => e.Defaultappendix).HasColumnName("defaultappendix");
            entity.Property(e => e.Form)
                .HasColumnType("character varying")
                .HasColumnName("form");
            entity.Property(e => e.Linkedform)
                .HasColumnType("character varying")
                .HasColumnName("linkedform");
            entity.Property(e => e.Linkedname)
                .HasColumnType("character varying")
                .HasColumnName("linkedname");
            entity.Property(e => e.TemplateTags).HasColumnType("character varying");
            entity.Property(e => e.Vibork)
                .HasColumnType("character varying")
                .HasColumnName("vibork");
        });

        modelBuilder.Entity<Funcoverk>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("funcoverk_pkey");

            entity.ToTable("funcoverk");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Desc).HasColumnType("character varying");
        });

        modelBuilder.Entity<Generator>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("generator_pkey");

            entity.ToTable("generator");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Adjpq)
                .HasColumnType("character varying")
                .HasColumnName("_adjpq");
            entity.Property(e => e.CosFi).HasColumnName("cosFi");
            entity.Property(e => e.DispNum).HasColumnName("disp_num");
            entity.Property(e => e.IdGenSql).HasColumnName("ID_GenSQL");
            entity.Property(e => e.Kct).HasColumnName("kct");
            entity.Property(e => e.Ke).HasColumnName("ke");
            entity.Property(e => e.Ki).HasColumnName("ki");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Ngou).HasColumnName("ngou");
            entity.Property(e => e.NumPq).HasColumnName("NumPQ");
            entity.Property(e => e.Sg)
                .HasColumnType("character varying")
                .HasColumnName("sg");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.TgA).HasColumnName("tgA");
            entity.Property(e => e.TiP).HasColumnName("ti_P");
            entity.Property(e => e.TiQ).HasColumnName("ti_Q");
            entity.Property(e => e.Xd).HasColumnName("xd");

            entity.HasOne(d => d.NodeNavigation).WithMany(p => p.Generators)
                .HasForeignKey(d => d.Node)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("generator_Node_fkey");
        });

        modelBuilder.Entity<Gou>(entity =>
        {
            entity.HasKey(e => e.Nu).HasName("gou_pkey");

            entity.ToTable("gou");

            entity.Property(e => e.Nu)
                .ValueGeneratedNever()
                .HasColumnName("nu");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.PgMax).HasColumnName("pg_max");
            entity.Property(e => e.PgMin).HasColumnName("pg_min");
            entity.Property(e => e.Vb)
                .HasColumnType("character varying")
                .HasColumnName("_vb");
        });

        modelBuilder.Entity<Graphik2>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("graphik2_pkey");

            entity.ToTable("graphik2");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
        });

        modelBuilder.Entity<Graphikit>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("graphikit_pkey");

            entity.ToTable("graphikit");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<GraphikitSource>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("graphikit_source_pkey");

            entity.ToTable("graphikit_source");

            entity.Property(e => e.Num).ValueGeneratedNever();
        });

        modelBuilder.Entity<Guid>(entity =>
        {
            entity.HasKey(e => new { e.Id1, e.Id2, e.Id3 }).HasName("guids_pkey");

            entity.ToTable("guids");

            entity.Property(e => e.Id1).HasColumnName("id1");
            entity.Property(e => e.Id2).HasColumnName("id2");
            entity.Property(e => e.Id3).HasColumnName("id3");
            entity.Property(e => e.Guid1)
                .HasColumnType("character varying")
                .HasColumnName("guid");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Interarea>(entity =>
        {
            entity.HasKey(e => new { e.BNa, e.ENa }).HasName("interarea_pkey");

            entity.ToTable("interarea");

            entity.Property(e => e.BNa).HasColumnName("b_na");
            entity.Property(e => e.ENa).HasColumnName("e_na");
            entity.Property(e => e.BNa2).HasColumnName("b_na2");
            entity.Property(e => e.BNa3).HasColumnName("b_na3");
            entity.Property(e => e.ENa2).HasColumnName("e_na2");
            entity.Property(e => e.ENa3).HasColumnName("e_na3");
            entity.Property(e => e.NaNa).HasColumnName("_na_na");
            entity.Property(e => e.NaNa2).HasColumnName("_na_na2");
            entity.Property(e => e.NaNa3).HasColumnName("_na_na3");
            entity.Property(e => e.NaName)
                .HasColumnType("character varying")
                .HasColumnName("_na_name");
            entity.Property(e => e.NaName2)
                .HasColumnType("character varying")
                .HasColumnName("_na_name2");
            entity.Property(e => e.NaName3)
                .HasColumnType("character varying")
                .HasColumnName("_na_name3");
            entity.Property(e => e.NaPs).HasColumnName("_na_ps");
            entity.Property(e => e.NaPs2).HasColumnName("_na_ps2");
            entity.Property(e => e.NaPs3).HasColumnName("_na_ps3");
            entity.Property(e => e.Najact)
                .HasColumnType("character varying")
                .HasColumnName("najact");
            entity.Property(e => e.Np).HasColumnName("np");
            entity.Property(e => e.Pb).HasColumnName("pb");
            entity.Property(e => e.Pb2).HasColumnName("pb2");
            entity.Property(e => e.Pb3).HasColumnName("pb3");
            entity.Property(e => e.Pe).HasColumnName("pe");
            entity.Property(e => e.Pe2).HasColumnName("pe2");
            entity.Property(e => e.Pe3).HasColumnName("pe3");
            entity.Property(e => e.Sb).HasColumnName("sb");
            entity.Property(e => e.Se).HasColumnName("se");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Tmpna).HasColumnName("_tmpna");
            entity.Property(e => e.Zbg).HasColumnName("_zbg");
            entity.Property(e => e.Zen).HasColumnName("_zen");
            entity.Property(e => e.Zero).HasColumnName("_zero");

            entity.HasOne(d => d.BNaNavigation).WithMany(p => p.InterareaBNaNavigations)
                .HasForeignKey(d => d.BNa)
                .HasConstraintName("interarea_b_na_fkey");

            entity.HasOne(d => d.ENaNavigation).WithMany(p => p.InterareaENaNavigations)
                .HasForeignKey(d => d.ENa)
                .HasConstraintName("interarea_e_na_fkey");
        });

        modelBuilder.Entity<Island>(entity =>
        {
            entity.HasKey(e => e.Nby).HasName("islands_pkey");

            entity.ToTable("islands");

            entity.Property(e => e.Nby)
                .ValueGeneratedNever()
                .HasColumnName("nby");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.F).HasColumnName("f");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.PgMax).HasColumnName("pg_max");
            entity.Property(e => e.PgMin).HasColumnName("pg_min");
            entity.Property(e => e.Pgr).HasColumnName("pgr");
            entity.Property(e => e.S).HasColumnName("s");
        });

        modelBuilder.Entity<KocBaseRg>(entity =>
        {
            entity.HasKey(e => e.Ny).HasName("koc_base_rg_pkey");

            entity.ToTable("koc_base_rg");

            entity.Property(e => e.Ny)
                .ValueGeneratedNever()
                .HasColumnName("ny");
            entity.Property(e => e.Na).HasColumnName("na");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PBase).HasColumnName("P_base");
            entity.Property(e => e.POc).HasColumnName("P_oc");
            entity.Property(e => e.POcOtkl).HasColumnName("P_oc_otkl");
            entity.Property(e => e.POtkl).HasColumnName("P_otkl");
            entity.Property(e => e.PTek).HasColumnName("P_tek");
            entity.Property(e => e.Pgpti).HasColumnName("pgpti");
            entity.Property(e => e.Pgzb).HasColumnName("pgzb");
            entity.Property(e => e.Pnpti).HasColumnName("pnpti");
            entity.Property(e => e.PnptiKpod).HasColumnName("pnpti_kpod");
            entity.Property(e => e.PnptiVidel).HasColumnName("pnpti_videl");
            entity.Property(e => e.Pnzb).HasColumnName("pnzb");
            entity.Property(e => e.Podsxem).HasColumnName("podsxem");
            entity.Property(e => e.Qgpti).HasColumnName("qgpti");
            entity.Property(e => e.Qgzb).HasColumnName("qgzb");
            entity.Property(e => e.Qnpti).HasColumnName("qnpti");
            entity.Property(e => e.QnptiKpod).HasColumnName("qnpti_kpod");
            entity.Property(e => e.QnptiVidel).HasColumnName("qnpti_videl");
            entity.Property(e => e.Qnzb).HasColumnName("qnzb");
            entity.Property(e => e.SourceP).HasColumnName("sourceP");
            entity.Property(e => e.SourceQ).HasColumnName("sourceQ");
            entity.Property(e => e.Tuz)
                .HasColumnType("character varying")
                .HasColumnName("tuz");
            entity.Property(e => e.Urb).HasColumnName("urb");
            entity.Property(e => e.Uzbr).HasColumnName("uzbr");

            entity.HasOne(d => d.NaNavigation).WithMany(p => p.KocBaseRgs)
                .HasForeignKey(d => d.Na)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("koc_base_rg_na_fkey");
        });

        modelBuilder.Entity<KocConst>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("koc_consts_pkey");

            entity.ToTable("koc_consts");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Alo).HasColumnName("ALO");
            entity.Property(e => e.Bkn).HasColumnName("BKN");
            entity.Property(e => e.Dp).HasColumnName("DP");
            entity.Property(e => e.Ekv).HasColumnName("EKV");
            entity.Property(e => e.Epp).HasColumnName("EPP");
            entity.Property(e => e.Goh).HasColumnName("GOH");
            entity.Property(e => e.Gsn).HasColumnName("GSN");
            entity.Property(e => e.Mit).HasColumnName("MIT");
            entity.Property(e => e.Nbu).HasColumnName("NBU");
            entity.Property(e => e.Ode).HasColumnName("ODE");
            entity.Property(e => e.Pkb).HasColumnName("PKB");
            entity.Property(e => e.Rpz).HasColumnName("RPZ");
            entity.Property(e => e.Sif).HasColumnName("SIF");
            entity.Property(e => e.Skp).HasColumnName("SKP");
            entity.Property(e => e.Sxn).HasColumnName("SXN");
            entity.Property(e => e.Szo).HasColumnName("SZO");
            entity.Property(e => e.Tbo).HasColumnName("TBO");
            entity.Property(e => e.Tsb).HasColumnName("TSB");
            entity.Property(e => e.Tsn).HasColumnName("TSN");
            entity.Property(e => e.Tso).HasColumnName("TSO");
        });

        modelBuilder.Entity<KocKweight>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("koc_kweights_pkey");

            entity.ToTable("koc_kweights");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.KW).HasColumnName("K_W");
            entity.Property(e => e.NumKw).HasColumnName("Num_KW");
        });

        modelBuilder.Entity<KocParam>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("koc_param_pkey");

            entity.ToTable("koc_param");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.KdataAfterOc).HasColumnName("kdata_after_oc");
            entity.Property(e => e.KdataBeforeOc).HasColumnName("kdata_before_oc");
            entity.Property(e => e.OcKakDemidov).HasColumnName("oc_kak_demidov");
            entity.Property(e => e.OcResults).HasColumnName("oc_results");
        });

        modelBuilder.Entity<KocUogr>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("koc_uogr_pkey");

            entity.ToTable("koc_uogr");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.UMaxOn).HasColumnName("u_max_on");
            entity.Property(e => e.UMinOn).HasColumnName("u_min_on");
            entity.Property(e => e.UNomOn).HasColumnName("u_nom_on");
        });

        modelBuilder.Entity<KurConst>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("kur_consts_pkey");

            entity.ToTable("kur_consts");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Alo).HasColumnName("ALO");
            entity.Property(e => e.Chas).HasColumnName("CHAS");
            entity.Property(e => e.DE).HasColumnName("d_E");
            entity.Property(e => e.DP1).HasColumnName("d_P");
            entity.Property(e => e.DQ).HasColumnName("d_Q");
            entity.Property(e => e.Dp).HasColumnName("DP");
            entity.Property(e => e.Dsn).HasColumnName("DSN");
            entity.Property(e => e.Ekg).HasColumnName("EKG");
            entity.Property(e => e.Err).HasColumnName("ERR");
            entity.Property(e => e.Ges).HasColumnName("GES");
            entity.Property(e => e.Kvo).HasColumnName("KVO");
            entity.Property(e => e.Mes).HasColumnName("MES");
            entity.Property(e => e.Mit).HasColumnName("MIT");
            entity.Property(e => e.Mkm).HasColumnName("MKM");
            entity.Property(e => e.Mku).HasColumnName("MKU");
            entity.Property(e => e.Mop).HasColumnName("MOP");
            entity.Property(e => e.Nbu).HasColumnName("NBU");
            entity.Property(e => e.Pom).HasColumnName("POM");
            entity.Property(e => e.Prr).HasColumnName("PRR");
            entity.Property(e => e.Shtr).HasColumnName("SHTR");
            entity.Property(e => e.Sxn).HasColumnName("SXN");
            entity.Property(e => e.Tna).HasColumnName("TNA");
            entity.Property(e => e.Tshn).HasColumnName("TSHN");
            entity.Property(e => e.Tsn).HasColumnName("TSN");
            entity.Property(e => e.Uog).HasColumnName("UOG");
        });

        modelBuilder.Entity<KurParam>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("kur_param_pkey");

            entity.ToTable("kur_param");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.KdataAfterUr).HasColumnName("kdata_after_ur");
            entity.Property(e => e.KurKakR).HasColumnName("kur_kak_r");
            entity.Property(e => e.UrResults)
                .HasColumnType("character varying")
                .HasColumnName("ur_results");
        });

        modelBuilder.Entity<Load>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("load_pkey");

            entity.ToTable("load");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Na).HasColumnName("na");
            entity.Property(e => e.Nam2)
                .HasColumnType("character varying")
                .HasColumnName("nam2");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Npa).HasColumnName("npa");
            entity.Property(e => e.Nsx).HasColumnName("nsx");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Tpo).HasColumnName("TPO");

            entity.HasOne(d => d.NodeNavigation).WithMany(p => p.Loads)
                .HasForeignKey(d => d.Node)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("load_Node_fkey");

            entity.HasOne(d => d.NsxNavigation).WithMany(p => p.Loads)
                .HasForeignKey(d => d.Nsx)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("load_nsx_fkey");
        });

        modelBuilder.Entity<Macrocontext>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("macrocontext_pkey");

            entity.ToTable("macrocontext");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Addstr)
                .HasColumnType("character varying")
                .HasColumnName("addstr");
            entity.Property(e => e.Col)
                .HasColumnType("character varying")
                .HasColumnName("col");
            entity.Property(e => e.Defaultappendix).HasColumnName("defaultappendix");
            entity.Property(e => e.Form)
                .HasColumnType("character varying")
                .HasColumnName("form");
            entity.Property(e => e.Formtype).HasColumnName("formtype");
            entity.Property(e => e.Macrodesc)
                .HasColumnType("character varying")
                .HasColumnName("macrodesc");
            entity.Property(e => e.Macrofile)
                .HasColumnType("character varying")
                .HasColumnName("macrofile");
            entity.Property(e => e.TemplateTags).HasColumnType("character varying");
        });

        modelBuilder.Entity<Meteo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("meteo_pkey");

            entity.ToTable("meteo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MeteoId).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Nagr>(entity =>
        {
            entity.HasKey(e => e.Nng).HasName("nagr_pkey");

            entity.ToTable("nagr");

            entity.Property(e => e.Nng)
                .ValueGeneratedNever()
                .HasColumnName("nng");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Pn).HasColumnName("pn");
            entity.Property(e => e.Pop).HasColumnName("pop");
            entity.Property(e => e.Poq).HasColumnName("poq");
        });

        modelBuilder.Entity<Ngroup>(entity =>
        {
            entity.HasKey(e => e.Nga).HasName("ngroup_pkey");

            entity.ToTable("ngroup");

            entity.Property(e => e.Nga)
                .ValueGeneratedNever()
                .HasColumnName("nga");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.NaNgroup)
                .HasColumnType("character varying")
                .HasColumnName("_na_ngroup");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Navet)
                .HasColumnType("character varying")
                .HasColumnName("_navet");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.Pn).HasColumnName("pn");
            entity.Property(e => e.Pop).HasColumnName("pop");
            entity.Property(e => e.Poq).HasColumnName("poq");
            entity.Property(e => e.Qg).HasColumnName("qg");
            entity.Property(e => e.Qn).HasColumnName("qn");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Vnp).HasColumnName("vnp");
            entity.Property(e => e.Vnq).HasColumnName("vnq");
            entity.Property(e => e.X3)
                .HasColumnType("character varying")
                .HasColumnName("_x3");
        });

        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(e => e.Ny).HasName("node_pkey");

            entity.ToTable("node");

            entity.Property(e => e.Ny)
                .ValueGeneratedNever()
                .HasColumnName("ny");
            entity.Property(e => e.BaseArea).HasColumnName("base_area");
            entity.Property(e => e.BasePriority).HasColumnName("base_priority");
            entity.Property(e => e.Brk).HasColumnName("brk");
            entity.Property(e => e.Bsh).HasColumnName("bsh");
            entity.Property(e => e.Bshr).HasColumnName("bshr");
            entity.Property(e => e.ContrV).HasColumnName("contr_v");
            entity.Property(e => e.Delta).HasColumnName("delta");
            entity.Property(e => e.Dpg).HasColumnName("dpg");
            entity.Property(e => e.Dpn).HasColumnName("dpn");
            entity.Property(e => e.Dqmax).HasColumnName("dqmax");
            entity.Property(e => e.Dqmin).HasColumnName("dqmin");
            entity.Property(e => e.Dqn).HasColumnName("dqn");
            entity.Property(e => e.Ekn).HasColumnName("_ekn");
            entity.Property(e => e.Epg).HasColumnName("_epg");
            entity.Property(e => e.Epn).HasColumnName("_epn");
            entity.Property(e => e.Esh).HasColumnName("_esh");
            entity.Property(e => e.ExistGen).HasColumnName("exist_gen");
            entity.Property(e => e.ExistLoad).HasColumnName("exist_load");
            entity.Property(e => e.Grk).HasColumnName("grk");
            entity.Property(e => e.Gsh).HasColumnName("gsh");
            entity.Property(e => e.Gshr).HasColumnName("gshr");
            entity.Property(e => e.Kct).HasColumnName("kct");
            entity.Property(e => e.Lp).HasColumnName("lp");
            entity.Property(e => e.Lq).HasColumnName("lq");
            entity.Property(e => e.Muskod).HasColumnName("muskod");
            entity.Property(e => e.Na).HasColumnName("na");
            entity.Property(e => e.NaName)
                .HasColumnType("character varying")
                .HasColumnName("na_name");
            entity.Property(e => e.NaNo).HasColumnName("na_no");
            entity.Property(e => e.NaPop).HasColumnName("na_pop");
            entity.Property(e => e.NablP).HasColumnName("nabl_p");
            entity.Property(e => e.NablPFrag).HasColumnName("nabl_p_frag");
            entity.Property(e => e.NablPGen).HasColumnName("nabl_p_gen");
            entity.Property(e => e.NablQ).HasColumnName("nabl_q");
            entity.Property(e => e.NablQFrag).HasColumnName("nabl_q_frag");
            entity.Property(e => e.Nadjgen)
                .HasColumnType("character varying")
                .HasColumnName("_nadjgen");
            entity.Property(e => e.Nadjload)
                .HasColumnType("character varying")
                .HasColumnName("_nadjload");
            entity.Property(e => e.Najact)
                .HasColumnType("character varying")
                .HasColumnName("_najact");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Nebal).HasColumnName("nebal");
            entity.Property(e => e.NebalQ).HasColumnName("nebal_q");
            entity.Property(e => e.Nf).HasColumnName("nf");
            entity.Property(e => e.Nga).HasColumnName("nga");
            entity.Property(e => e.Npa).HasColumnName("npa");
            entity.Property(e => e.Nrk).HasColumnName("nrk");
            entity.Property(e => e.Nsx).HasColumnName("nsx");
            entity.Property(e => e.Numgen).HasColumnName("_numgen");
            entity.Property(e => e.Numload).HasColumnName("_numload");
            entity.Property(e => e.NySubst).HasColumnName("ny_subst");
            entity.Property(e => e.Otv).HasColumnName("otv");
            entity.Property(e => e.Pg).HasColumnName("pg");
            entity.Property(e => e.PgMax).HasColumnName("pg_max");
            entity.Property(e => e.PgMin).HasColumnName("pg_min");
            entity.Property(e => e.PgNom).HasColumnName("pg_nom");
            entity.Property(e => e.Pgr).HasColumnName("pgr");
            entity.Property(e => e.Pn).HasColumnName("pn");
            entity.Property(e => e.PnIzm).HasColumnName("Pn_izm");
            entity.Property(e => e.PnMax).HasColumnName("pn_max");
            entity.Property(e => e.PnMin).HasColumnName("pn_min");
            entity.Property(e => e.Pnr).HasColumnName("pnr");
            entity.Property(e => e.Psh).HasColumnName("psh");
            entity.Property(e => e.Qg).HasColumnName("qg");
            entity.Property(e => e.Qgr).HasColumnName("qgr");
            entity.Property(e => e.Qmax).HasColumnName("qmax");
            entity.Property(e => e.Qmima)
                .HasColumnType("character varying")
                .HasColumnName("qmima");
            entity.Property(e => e.Qmin).HasColumnName("qmin");
            entity.Property(e => e.Qn).HasColumnName("qn");
            entity.Property(e => e.QnIzm).HasColumnName("Qn_izm");
            entity.Property(e => e.QnMax).HasColumnName("qn_max");
            entity.Property(e => e.QnMin).HasColumnName("qn_min");
            entity.Property(e => e.Qnr).HasColumnName("qnr");
            entity.Property(e => e.Qsh).HasColumnName("qsh");
            entity.Property(e => e.Rang).HasColumnName("rang");
            entity.Property(e => e.RegQ).HasColumnName("reg_q");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sg)
                .HasColumnType("character varying")
                .HasColumnName("sg");
            entity.Property(e => e.Sgr)
                .HasColumnType("character varying")
                .HasColumnName("sgr");
            entity.Property(e => e.Sn)
                .HasColumnType("character varying")
                .HasColumnName("sn");
            entity.Property(e => e.Sn1).HasColumnName("%sn");
            entity.Property(e => e.Snr)
                .HasColumnType("character varying")
                .HasColumnName("snr");
            entity.Property(e => e.Ssh)
                .HasColumnType("character varying")
                .HasColumnName("ssh");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.StaR).HasColumnName("sta_r");
            entity.Property(e => e.Supernode).HasColumnName("supernode");
            entity.Property(e => e.TgPhi).HasColumnName("tg_phi");
            entity.Property(e => e.TiBalP).HasColumnName("ti_bal_p");
            entity.Property(e => e.TiBalPq)
                .HasColumnType("character varying")
                .HasColumnName("ti_bal_pq");
            entity.Property(e => e.TiBalQ).HasColumnName("ti_bal_q");
            entity.Property(e => e.TiOcSg)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_sg");
            entity.Property(e => e.TiOcSn)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_sn");
            entity.Property(e => e.TiOcVras)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_vras");
            entity.Property(e => e.TiPg).HasColumnName("ti_pg");
            entity.Property(e => e.TiPgDiff).HasColumnName("_ti_pg_diff");
            entity.Property(e => e.TiPn).HasColumnName("ti_pn");
            entity.Property(e => e.TiPnDiff).HasColumnName("_ti_pn_diff");
            entity.Property(e => e.TiQg).HasColumnName("ti_qg");
            entity.Property(e => e.TiQgDiff).HasColumnName("_ti_qg_diff");
            entity.Property(e => e.TiQn).HasColumnName("ti_qn");
            entity.Property(e => e.TiQnDiff).HasColumnName("_ti_qn_diff");
            entity.Property(e => e.TiSg)
                .HasColumnType("character varying")
                .HasColumnName("ti_sg");
            entity.Property(e => e.TiSgDiff)
                .HasColumnType("character varying")
                .HasColumnName("_ti_sg_diff");
            entity.Property(e => e.TiSn)
                .HasColumnType("character varying")
                .HasColumnName("ti_sn");
            entity.Property(e => e.TiSnDiff)
                .HasColumnType("character varying")
                .HasColumnName("_ti_sn_diff");
            entity.Property(e => e.TiVras).HasColumnName("ti_vras");
            entity.Property(e => e.TiVrasDiff).HasColumnName("_ti_vras_diff");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Tsn).HasColumnName("tsn");
            entity.Property(e => e.UIzm).HasColumnName("U_izm");
            entity.Property(e => e.Uc)
                .HasColumnType("character varying")
                .HasColumnName("uc");
            entity.Property(e => e.Uhom).HasColumnName("uhom");
            entity.Property(e => e.Umax).HasColumnName("umax");
            entity.Property(e => e.Umima)
                .HasColumnType("character varying")
                .HasColumnName("umima");
            entity.Property(e => e.Umin).HasColumnName("umin");
            entity.Property(e => e.Vras).HasColumnName("vras");
            entity.Property(e => e.Vzd).HasColumnName("vzd");
            entity.Property(e => e.Ysh).HasColumnType("character varying");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Customerid)
                .ValueGeneratedNever()
                .HasColumnName("customerid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Customer).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_customerid_fkey");
        });

        modelBuilder.Entity<Polin>(entity =>
        {
            entity.HasKey(e => e.Nsx).HasName("polin_pkey");

            entity.ToTable("polin");

            entity.Property(e => e.Nsx)
                .ValueGeneratedNever()
                .HasColumnName("nsx");
            entity.Property(e => e.Frec).HasColumnName("frec");
            entity.Property(e => e.Frecq).HasColumnName("frecq");
            entity.Property(e => e.P0).HasColumnName("p0");
            entity.Property(e => e.P1).HasColumnName("p1");
            entity.Property(e => e.P2).HasColumnName("p2");
            entity.Property(e => e.P3).HasColumnName("p3");
            entity.Property(e => e.P4).HasColumnName("p4");
            entity.Property(e => e.Q0).HasColumnName("q0");
            entity.Property(e => e.Q1).HasColumnName("q1");
            entity.Property(e => e.Q2).HasColumnName("q2");
            entity.Property(e => e.Q3).HasColumnName("q3");
            entity.Property(e => e.Q4).HasColumnName("q4");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Umin).HasColumnName("umin");
        });

        modelBuilder.Entity<Poteri>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("poteri_pkey");

            entity.ToTable("poteri");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.DpLine).HasColumnName("dp_line");
            entity.Property(e => e.DpSh).HasColumnName("dp_sh");
            entity.Property(e => e.DpTran).HasColumnName("dp_tran");
            entity.Property(e => e.Dpa).HasColumnName("dpa");
            entity.Property(e => e.Dpa2).HasColumnName("dpa2");
            entity.Property(e => e.Dpa3).HasColumnName("dpa3");
            entity.Property(e => e.DpaLine).HasColumnName("dpa_line");
            entity.Property(e => e.DpaLine2).HasColumnName("dpa_line2");
            entity.Property(e => e.DpaLine3).HasColumnName("dpa_line3");
            entity.Property(e => e.DpaTran).HasColumnName("dpa_tran");
            entity.Property(e => e.DpaTran2).HasColumnName("dpa_tran2");
            entity.Property(e => e.DpaTran3).HasColumnName("dpa_tran3");
            entity.Property(e => e.Dpda).HasColumnName("dpda");
            entity.Property(e => e.DpdaLine).HasColumnName("dpda_line");
            entity.Property(e => e.DpdaTran).HasColumnName("dpda_tran");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.DqLine).HasColumnName("dq_line");
            entity.Property(e => e.DqSh).HasColumnName("dq_sh");
            entity.Property(e => e.DqTran).HasColumnName("dq_tran");
            entity.Property(e => e.Dqa).HasColumnName("dqa");
            entity.Property(e => e.Dqa2).HasColumnName("dqa2");
            entity.Property(e => e.Dqa3).HasColumnName("dqa3");
            entity.Property(e => e.DqaLine).HasColumnName("dqa_line");
            entity.Property(e => e.DqaLine2).HasColumnName("dqa_line2");
            entity.Property(e => e.DqaLine3).HasColumnName("dqa_line3");
            entity.Property(e => e.DqaTran).HasColumnName("dqa_tran");
            entity.Property(e => e.DqaTran2).HasColumnName("dqa_tran2");
            entity.Property(e => e.DqaTran3).HasColumnName("dqa_tran3");
            entity.Property(e => e.Dqda).HasColumnName("dqda");
            entity.Property(e => e.DqdaLine).HasColumnName("dqda_line");
            entity.Property(e => e.DqdaTran).HasColumnName("dqda_tran");
            entity.Property(e => e.SpLine).HasColumnName("sp_line");
            entity.Property(e => e.SpTran).HasColumnName("sp_tran");
            entity.Property(e => e.Spa).HasColumnName("spa");
            entity.Property(e => e.Spa2).HasColumnName("spa2");
            entity.Property(e => e.Spa3).HasColumnName("spa3");
            entity.Property(e => e.SpaLine).HasColumnName("spa_line");
            entity.Property(e => e.SpaLine2).HasColumnName("spa_line2");
            entity.Property(e => e.SpaLine3).HasColumnName("spa_line3");
            entity.Property(e => e.SpaTran).HasColumnName("spa_tran");
            entity.Property(e => e.SpaTran2).HasColumnName("spa_tran2");
            entity.Property(e => e.SpaTran3).HasColumnName("spa_tran3");
            entity.Property(e => e.Spda).HasColumnName("spda");
            entity.Property(e => e.SpdaLine).HasColumnName("spda_line");
            entity.Property(e => e.SpdaTran).HasColumnName("spda_tran");
            entity.Property(e => e.SqLine).HasColumnName("sq_line");
            entity.Property(e => e.SqTran).HasColumnName("sq_tran");
            entity.Property(e => e.Sqa).HasColumnName("sqa");
            entity.Property(e => e.Sqa2).HasColumnName("sqa2");
            entity.Property(e => e.Sqa3).HasColumnName("sqa3");
            entity.Property(e => e.SqaLine).HasColumnName("sqa_line");
            entity.Property(e => e.SqaLine2).HasColumnName("sqa_line2");
            entity.Property(e => e.SqaLine3).HasColumnName("sqa_line3");
            entity.Property(e => e.SqaTran).HasColumnName("sqa_tran");
            entity.Property(e => e.SqaTran2).HasColumnName("sqa_tran2");
            entity.Property(e => e.SqaTran3).HasColumnName("sqa_tran3");
            entity.Property(e => e.Sqda).HasColumnName("sqda");
            entity.Property(e => e.SqdaLine).HasColumnName("sqda_line");
            entity.Property(e => e.SqdaTran).HasColumnName("sqda_tran");
            entity.Property(e => e.Tmp).HasColumnName("_tmp");
            entity.Property(e => e.Uhom).HasColumnName("uhom");
        });

        modelBuilder.Entity<Reactor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reactors_pkey");

            entity.ToTable("reactors");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DispNum).HasColumnName("disp_num");
            entity.Property(e => e.IdReacSql).HasColumnName("ID_ReacSQL");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.PrIzm).HasColumnName("Pr_izm");
            entity.Property(e => e.PrVikl).HasColumnName("Pr_vikl");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Tip).HasColumnName("tip");
        });

        modelBuilder.Entity<Sendcommandmainform>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("sendcommandmainforms_pkey");

            entity.ToTable("sendcommandmainforms");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.CommId).HasColumnName("commID");
            entity.Property(e => e.Formname)
                .HasColumnType("character varying")
                .HasColumnName("formname");
            entity.Property(e => e.Menu).HasColumnName("menu");
            entity.Property(e => e.P1)
                .HasColumnType("character varying")
                .HasColumnName("p1");
            entity.Property(e => e.P2)
                .HasColumnType("character varying")
                .HasColumnName("p2");
            entity.Property(e => e.Pp).HasColumnName("pp");
            entity.Property(e => e.Submenu)
                .HasColumnType("character varying")
                .HasColumnName("submenu");
            entity.Property(e => e.Tgtmask).HasColumnName("tgtmask");
        });

        modelBuilder.Entity<SqlSetting>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("sql_settings_pkey");

            entity.ToTable("sql_settings");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.AllowAddCols).HasColumnName("allow_add_cols");
            entity.Property(e => e.AllowAddRows).HasColumnName("allow_add_rows");
            entity.Property(e => e.AllowAddTable).HasColumnName("allow_add_table");
            entity.Property(e => e.AllowDelRows).HasColumnName("allow_del_rows");
            entity.Property(e => e.ConnString)
                .HasColumnType("character varying")
                .HasColumnName("conn_string");
            entity.Property(e => e.ConnStringPg)
                .HasColumnType("character varying")
                .HasColumnName("conn_string_pg");
            entity.Property(e => e.Database)
                .HasColumnType("character varying")
                .HasColumnName("database");
            entity.Property(e => e.ReadNullTable).HasColumnName("read_null_table");
            entity.Property(e => e.SmzuDatabase)
                .HasColumnType("character varying")
                .HasColumnName("smzu_database");
            entity.Property(e => e.SmzuFiles)
                .HasColumnType("character varying")
                .HasColumnName("smzu_files");
            entity.Property(e => e.SmzuServers)
                .HasColumnType("character varying")
                .HasColumnName("smzu_servers");
            entity.Property(e => e.WriteNullTable).HasColumnName("write_null_table");
        });

        modelBuilder.Entity<Ti>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("ti_pkey");

            entity.ToTable("ti");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Cod).HasColumnName("cod");
            entity.Property(e => e.CodDescr)
                .HasColumnType("character varying")
                .HasColumnName("cod_descr");
            entity.Property(e => e.CodKu).HasColumnName("cod_ku");
            entity.Property(e => e.CodOc).HasColumnName("cod_oc");
            entity.Property(e => e.DifOc).HasColumnName("dif_oc");
            entity.Property(e => e.DifRastrKocmocOc).HasColumnName("dif_rastr_kocmoc_oc");
            entity.Property(e => e.Dor)
                .HasColumnType("character varying")
                .HasColumnName("dor");
            entity.Property(e => e.Dt).HasColumnName("dt");
            entity.Property(e => e.Guid)
                .HasColumnType("character varying")
                .HasColumnName("guid");
            entity.Property(e => e.Id1).HasColumnName("id1");
            entity.Property(e => e.Id2).HasColumnName("id2");
            entity.Property(e => e.Id3).HasColumnName("id3");
            entity.Property(e => e.KeyNum).HasColumnName("key_Num");
            entity.Property(e => e.Lagr).HasColumnName("lagr");
            entity.Property(e => e.LagrKrit).HasColumnName("lagr_krit");
            entity.Property(e => e.Mas)
                .HasColumnType("character varying")
                .HasColumnName("mas");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Nb1).HasColumnName("nb1");
            entity.Property(e => e.Nb2).HasColumnName("nb2");
            entity.Property(e => e.NebKu).HasColumnName("neb_ku");
            entity.Property(e => e.PrevVal).HasColumnName("prev_val");
            entity.Property(e => e.Price1).HasColumnName("price1");
            entity.Property(e => e.Price2).HasColumnName("price2");
            entity.Property(e => e.PriceSoft1).HasColumnName("price_soft1");
            entity.Property(e => e.PriceSoft2).HasColumnName("price_soft2");
            entity.Property(e => e.PrvNum).HasColumnName("prv_num");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.TiMax).HasColumnName("ti_max");
            entity.Property(e => e.TiMin).HasColumnName("ti_min");
            entity.Property(e => e.TiOcen).HasColumnName("ti_ocen");
            entity.Property(e => e.TiOcenKocmoc).HasColumnName("ti_ocen_kocmoc");
            entity.Property(e => e.TiOcenRastr).HasColumnName("ti_ocen_rastr");
            entity.Property(e => e.TiSmax).HasColumnName("ti_smax");
            entity.Property(e => e.TiSmin).HasColumnName("ti_smin");
            entity.Property(e => e.TiVal).HasColumnName("ti_val");
            entity.Property(e => e.Time1).HasColumnName("time1");
            entity.Property(e => e.Time2).HasColumnName("time2");
            entity.Property(e => e.TipTi).HasColumnName("tip_ti");
            entity.Property(e => e.TipsTi).HasColumnName("tips_ti");
            entity.Property(e => e.Tso).HasColumnName("tso");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.Wrt).HasColumnName("wrt");
        });

        modelBuilder.Entity<TiBalansP>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("ti_balans_p_pkey");

            entity.ToTable("ti_balans_p");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NtiIp).HasColumnName("nti_ip");
            entity.Property(e => e.NtiIq).HasColumnName("nti_iq");
            entity.Property(e => e.NtiItog).HasColumnName("nti_itog");
            entity.Property(e => e.PgPlIq).HasColumnName("pg_pl_iq");
            entity.Property(e => e.PlshIp).HasColumnName("plsh_ip");
            entity.Property(e => e.PlshIq).HasColumnName("plsh_iq");
            entity.Property(e => e.PnPlIp).HasColumnName("pn_pl_ip");
            entity.Property(e => e.PnodePvet).HasColumnName("pnode_pvet");
            entity.Property(e => e.PnshDpvet).HasColumnName("pnsh_dpvet");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.TiIp).HasColumnName("ti_ip");
            entity.Property(e => e.TiIq).HasColumnName("ti_iq");
            entity.Property(e => e.TiNp).HasColumnName("ti_np");
        });

        modelBuilder.Entity<TiBalansQ>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("ti_balans_q_pkey");

            entity.ToTable("ti_balans_q");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NtiIp).HasColumnName("nti_ip");
            entity.Property(e => e.NtiIq).HasColumnName("nti_iq");
            entity.Property(e => e.NtiItog).HasColumnName("nti_itog");
            entity.Property(e => e.QgQlIq).HasColumnName("qg_ql_iq");
            entity.Property(e => e.QlreacIp).HasColumnName("qlreac_ip");
            entity.Property(e => e.QlreacIq).HasColumnName("qlreac_iq");
            entity.Property(e => e.QlshIp).HasColumnName("qlsh_ip");
            entity.Property(e => e.QlshIq).HasColumnName("qlsh_iq");
            entity.Property(e => e.QnQlIp).HasColumnName("qn_ql_ip");
            entity.Property(e => e.QnodeQvet).HasColumnName("qnode_qvet");
            entity.Property(e => e.QnshDqvet).HasColumnName("qnsh_dqvet");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.TiIp).HasColumnName("ti_ip");
            entity.Property(e => e.TiIq).HasColumnName("ti_iq");
            entity.Property(e => e.TiNp).HasColumnName("ti_np");
        });

        modelBuilder.Entity<TiPrv>(entity =>
        {
            entity.HasKey(e => e.Num).HasName("ti_prv_pkey");

            entity.ToTable("ti_prv");

            entity.Property(e => e.Num).ValueGeneratedNever();
            entity.Property(e => e.Col)
                .HasColumnType("character varying")
                .HasColumnName("col");
            entity.Property(e => e.ColId1)
                .HasColumnType("character varying")
                .HasColumnName("col_id1");
            entity.Property(e => e.ColId2)
                .HasColumnType("character varying")
                .HasColumnName("col_id2");
            entity.Property(e => e.ColId3)
                .HasColumnType("character varying")
                .HasColumnName("col_id3");
            entity.Property(e => e.ColTi)
                .HasColumnType("character varying")
                .HasColumnName("col_ti");
            entity.Property(e => e.Eps).HasColumnName("eps");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.NameSmzu)
                .HasColumnType("character varying")
                .HasColumnName("Name_SMZU");
            entity.Property(e => e.Nves).HasColumnName("NVes");
            entity.Property(e => e.PriceType).HasColumnName("price_type");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.TUpd).HasColumnName("t_upd");
            entity.Property(e => e.Table)
                .HasColumnType("character varying")
                .HasColumnName("table");
            entity.Property(e => e.TiMax).HasColumnName("TI_max");
            entity.Property(e => e.TipIzm).HasColumnName("tip_izm");
        });

        modelBuilder.Entity<TiSetting>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("ti_settings_pkey");

            entity.ToTable("ti_settings");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.CalcStat).HasColumnName("calc_stat");
            entity.Property(e => e.CosRegimsPath)
                .HasColumnType("character varying")
                .HasColumnName("cos_regims_path");
            entity.Property(e => e.CosSaveBd).HasColumnName("cos_save_bd");
            entity.Property(e => e.CosSaveFiles).HasColumnName("cos_save_files");
            entity.Property(e => e.CosShabl)
                .HasColumnType("character varying")
                .HasColumnName("cos_shabl");
            entity.Property(e => e.DelNSrez).HasColumnName("del_n_srez");
            entity.Property(e => e.DeleteCosFiles).HasColumnName("delete_cos_files");
            entity.Property(e => e.LiveHoursCosFiles).HasColumnName("live_hours_cos_files");
            entity.Property(e => e.MaxNSrez).HasColumnName("max_n_srez");
            entity.Property(e => e.OikName)
                .HasColumnType("character varying")
                .HasColumnName("oik_name");
            entity.Property(e => e.PathBd)
                .HasColumnType("character varying")
                .HasColumnName("path_bd");
            entity.Property(e => e.PathGetTm)
                .HasColumnType("character varying")
                .HasColumnName("path_get_tm");
            entity.Property(e => e.PathTmp)
                .HasColumnType("character varying")
                .HasColumnName("path_tmp");
            entity.Property(e => e.Timer).HasColumnName("timer");
        });

        modelBuilder.Entity<Tree>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("trees_pkey");

            entity.ToTable("trees");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.TreeName).HasColumnType("character varying");
        });

        modelBuilder.Entity<Treelevel>(entity =>
        {
            entity.HasKey(e => e.SqlId).HasName("treelevels_pkey");

            entity.ToTable("treelevels");

            entity.Property(e => e.SqlId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("sql_id");
            entity.Property(e => e.Caption).HasColumnType("character varying");
            entity.Property(e => e.ItemName).HasColumnType("character varying");
            entity.Property(e => e.ParentKeys).HasColumnType("character varying");
            entity.Property(e => e.Sel).HasColumnType("character varying");
            entity.Property(e => e.SelfKeys).HasColumnType("character varying");
            entity.Property(e => e.SysTabName).HasColumnType("character varying");
        });

        modelBuilder.Entity<Ushr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ushr_pkey");

            entity.ToTable("ushr");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Dcnode).HasColumnName("DCnode");
            entity.Property(e => e.Izm).HasColumnName("izm");
            entity.Property(e => e.Mode).HasColumnName("mode");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.Tref1).HasColumnName("tref1");
            entity.Property(e => e.Tref2).HasColumnName("tref2");
        });

        modelBuilder.Entity<Vetv>(entity =>
        {
            entity.HasKey(e => new { e.Ip, e.Iq, e.Np }).HasName("vetv_pkey");

            entity.ToTable("vetv");

            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.Iq).HasColumnName("iq");
            entity.Property(e => e.Np).HasColumnName("np");
            entity.Property(e => e.B).HasColumnName("b");
            entity.Property(e => e.BIp).HasColumnName("b_ip");
            entity.Property(e => e.BIq).HasColumnName("b_iq");
            entity.Property(e => e.Bd).HasColumnName("bd");
            entity.Property(e => e.BrIp).HasColumnName("br_ip");
            entity.Property(e => e.BrIq).HasColumnName("br_iq");
            entity.Property(e => e.ContrI).HasColumnName("contr_i");
            entity.Property(e => e.DIp).HasColumnName("d_ip");
            entity.Property(e => e.DIq).HasColumnName("d_iq");
            entity.Property(e => e.Dij).HasColumnName("dij");
            entity.Property(e => e.DijNy).HasColumnName("_dij_ny");
            entity.Property(e => e.Div).HasColumnName("div");
            entity.Property(e => e.Div2).HasColumnName("div2");
            entity.Property(e => e.Div3).HasColumnName("div3");
            entity.Property(e => e.DivNga).HasColumnName("div_nga");
            entity.Property(e => e.Dname)
                .HasColumnType("character varying")
                .HasColumnName("dname");
            entity.Property(e => e.Dp).HasColumnName("dp");
            entity.Property(e => e.Dq).HasColumnName("dq");
            entity.Property(e => e.Dv).HasColumnName("dv");
            entity.Property(e => e.DvNy).HasColumnName("_dv_ny");
            entity.Property(e => e.G).HasColumnName("g");
            entity.Property(e => e.GIp).HasColumnName("g_ip");
            entity.Property(e => e.GIq).HasColumnName("g_iq");
            entity.Property(e => e.GrIp).HasColumnName("gr_ip");
            entity.Property(e => e.GrIq).HasColumnName("gr_iq");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.IDop).HasColumnName("i_dop");
            entity.Property(e => e.IDopAv).HasColumnName("i_dop_av");
            entity.Property(e => e.IDopOb).HasColumnName("i_dop_ob");
            entity.Property(e => e.IDopObAv).HasColumnName("i_dop_ob_av");
            entity.Property(e => e.IDopR).HasColumnName("i_dop_r");
            entity.Property(e => e.IDopRAv).HasColumnName("i_dop_r_av");
            entity.Property(e => e.IMax).HasColumnName("i_max");
            entity.Property(e => e.IMsi).HasColumnName("i_msi");
            entity.Property(e => e.INy).HasColumnName("_i_ny");
            entity.Property(e => e.IZag).HasColumnName("i_zag");
            entity.Property(e => e.Ib).HasColumnName("ib");
            entity.Property(e => e.Ie).HasColumnName("ie");
            entity.Property(e => e.InNy).HasColumnName("_in_ny");
            entity.Property(e => e.IsBreaker).HasColumnName("is_breaker");
            entity.Property(e => e.KiMax).HasColumnName("ki_max");
            entity.Property(e => e.KiMin).HasColumnName("ki_min");
            entity.Property(e => e.KrMax).HasColumnName("kr_max");
            entity.Property(e => e.KrMin).HasColumnName("kr_min");
            entity.Property(e => e.KtB).HasColumnName("kt_b");
            entity.Property(e => e.Kti).HasColumnName("kti");
            entity.Property(e => e.Ktr).HasColumnName("ktr");
            entity.Property(e => e.L).HasColumnName("l");
            entity.Property(e => e.L2).HasColumnName("l2");
            entity.Property(e => e.L3).HasColumnName("l3");
            entity.Property(e => e.Lsum).HasColumnName("lsum");
            entity.Property(e => e.MaxDd).HasColumnName("max_dd");
            entity.Property(e => e.MaxDv).HasColumnName("max_dv");
            entity.Property(e => e.MeteoIdIp).HasColumnName("MeteoId_ip");
            entity.Property(e => e.MeteoIdIq).HasColumnName("MeteoId_iq");
            entity.Property(e => e.Msi).HasColumnName("msi");
            entity.Property(e => e.NAnc).HasColumnName("n_anc");
            entity.Property(e => e.NAncI).HasColumnName("n_anc_i");
            entity.Property(e => e.NIt).HasColumnName("n_it");
            entity.Property(e => e.NItAv).HasColumnName("n_it_av");
            entity.Property(e => e.Na).HasColumnName("na");
            entity.Property(e => e.NaDp).HasColumnName("_na_dp");
            entity.Property(e => e.NaDv).HasColumnName("_na_dv");
            entity.Property(e => e.NaNa).HasColumnName("_na_na");
            entity.Property(e => e.NaName)
                .HasColumnType("character varying")
                .HasColumnName("_na_name");
            entity.Property(e => e.NaNy)
                .HasColumnType("character varying")
                .HasColumnName("_na_ny");
            entity.Property(e => e.NaPl).HasColumnName("_na_pl");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NameNy)
                .HasColumnType("character varying")
                .HasColumnName("_name_ny");
            entity.Property(e => e.Nga).HasColumnName("nga");
            entity.Property(e => e.NgaDp).HasColumnName("_nga_dp");
            entity.Property(e => e.NgaDv).HasColumnName("_nga_dv");
            entity.Property(e => e.NgaNa).HasColumnName("_nga_na");
            entity.Property(e => e.NgaName)
                .HasColumnType("character varying")
                .HasColumnName("_nga_name");
            entity.Property(e => e.NgaNy)
                .HasColumnType("character varying")
                .HasColumnName("_nga_ny");
            entity.Property(e => e.NgaPl).HasColumnName("_nga_pl");
            entity.Property(e => e.Npa).HasColumnName("npa");
            entity.Property(e => e.NpaDp).HasColumnName("_npa_dp");
            entity.Property(e => e.NpaDv).HasColumnName("_npa_dv");
            entity.Property(e => e.NpaNa).HasColumnName("_npa_na");
            entity.Property(e => e.NpaName)
                .HasColumnType("character varying")
                .HasColumnName("_npa_name");
            entity.Property(e => e.NpaNy)
                .HasColumnType("character varying")
                .HasColumnName("_npa_ny");
            entity.Property(e => e.NpaPl).HasColumnName("_npa_pl");
            entity.Property(e => e.NrIp).HasColumnName("nr_ip");
            entity.Property(e => e.NrIq).HasColumnName("nr_iq");
            entity.Property(e => e.PlBal).HasColumnName("pl_bal");
            entity.Property(e => e.PlIp).HasColumnName("pl_ip");
            entity.Property(e => e.PlIpNy).HasColumnName("_pl_ip_ny");
            entity.Property(e => e.PlIq).HasColumnName("pl_iq");
            entity.Property(e => e.PlIqNy).HasColumnName("_pl_iq_ny");
            entity.Property(e => e.PlNy).HasColumnName("_pl_ny");
            entity.Property(e => e.Plmax).HasColumnName("plmax");
            entity.Property(e => e.Psh).HasColumnName("psh");
            entity.Property(e => e.QlIp).HasColumnName("ql_ip");
            entity.Property(e => e.QlIpNy).HasColumnName("_ql_ip_ny");
            entity.Property(e => e.QlIq).HasColumnName("ql_iq");
            entity.Property(e => e.QlIqNy).HasColumnName("_ql_iq_ny");
            entity.Property(e => e.QlNy).HasColumnName("_ql_ny");
            entity.Property(e => e.Qsh).HasColumnName("qsh");
            entity.Property(e => e.R).HasColumnName("r");
            entity.Property(e => e.RegKt).HasColumnName("reg_kt");
            entity.Property(e => e.Sel).HasColumnName("sel");
            entity.Property(e => e.SignP).HasColumnName("signP");
            entity.Property(e => e.SignQip).HasColumnName("signQip");
            entity.Property(e => e.SignQiq).HasColumnName("signQiq");
            entity.Property(e => e.Slb)
                .HasColumnType("character varying")
                .HasColumnName("slb");
            entity.Property(e => e.Sle)
                .HasColumnType("character varying")
                .HasColumnName("sle");
            entity.Property(e => e.Slmax)
                .HasColumnType("character varying")
                .HasColumnName("slmax");
            entity.Property(e => e.SrIp).HasColumnName("sr_ip");
            entity.Property(e => e.SrIq).HasColumnName("sr_iq");
            entity.Property(e => e.Sta).HasColumnName("sta");
            entity.Property(e => e.StrMeteoIdIp)
                .HasColumnType("character varying")
                .HasColumnName("strMeteoId_ip");
            entity.Property(e => e.StrMeteoIdIq)
                .HasColumnType("character varying")
                .HasColumnName("strMeteoId_iq");
            entity.Property(e => e.Sup).HasColumnName("sup");
            entity.Property(e => e.Sup2)
                .HasColumnType("character varying")
                .HasColumnName("_sup2");
            entity.Property(e => e.SupernodeIp).HasColumnName("supernode_ip");
            entity.Property(e => e.SupernodeIq).HasColumnName("supernode_iq");
            entity.Property(e => e.Ta).HasColumnName("ta");
            entity.Property(e => e.TiOcPlIpNy)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_pl_ip_ny");
            entity.Property(e => e.TiOcPlIqNy)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_pl_iq_ny");
            entity.Property(e => e.TiOcQlIpNy)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_ql_ip_ny");
            entity.Property(e => e.TiOcQlIqNy)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_ql_iq_ny");
            entity.Property(e => e.TiOcSlb)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_slb");
            entity.Property(e => e.TiOcSle)
                .HasColumnType("character varying")
                .HasColumnName("_ti_oc_sle");
            entity.Property(e => e.TiPlIp).HasColumnName("ti_pl_ip");
            entity.Property(e => e.TiPlIpNy).HasColumnName("_ti_pl_ip_ny");
            entity.Property(e => e.TiPlIpNyDiff).HasColumnName("_ti_pl_ip_ny_diff");
            entity.Property(e => e.TiPlIq).HasColumnName("ti_pl_iq");
            entity.Property(e => e.TiPlIqNy).HasColumnName("_ti_pl_iq_ny");
            entity.Property(e => e.TiPlIqNyDiff).HasColumnName("_ti_pl_iq_ny_diff");
            entity.Property(e => e.TiQlIp).HasColumnName("ti_ql_ip");
            entity.Property(e => e.TiQlIpNy).HasColumnName("_ti_ql_ip_ny");
            entity.Property(e => e.TiQlIpNyDiff).HasColumnName("_ti_ql_ip_ny_diff");
            entity.Property(e => e.TiQlIq).HasColumnName("ti_ql_iq");
            entity.Property(e => e.TiQlIqNy).HasColumnName("_ti_ql_iq_ny");
            entity.Property(e => e.TiQlIqNyDiff).HasColumnName("_ti_ql_iq_ny_diff");
            entity.Property(e => e.TiSlb)
                .HasColumnType("character varying")
                .HasColumnName("ti_slb");
            entity.Property(e => e.TiSlbDiff)
                .HasColumnType("character varying")
                .HasColumnName("_ti_slb_diff");
            entity.Property(e => e.TiSle)
                .HasColumnType("character varying")
                .HasColumnName("ti_sle");
            entity.Property(e => e.TiSleDiff)
                .HasColumnType("character varying")
                .HasColumnName("_ti_sle_diff");
            entity.Property(e => e.TiV2Ny).HasColumnName("_ti_v2_ny");
            entity.Property(e => e.TiVrasDiff).HasColumnName("_ti_vras_diff");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Tmpny).HasColumnName("_tmpny");
            entity.Property(e => e.V2Ny).HasColumnName("_v2_ny");
            entity.Property(e => e.VIp).HasColumnName("v_ip");
            entity.Property(e => e.VIq).HasColumnName("v_iq");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Z)
                .HasColumnType("character varying")
                .HasColumnName("z");
            entity.Property(e => e.ZagI).HasColumnName("zag_i");
            entity.Property(e => e.ZagIAv).HasColumnName("zag_i_av");
            entity.Property(e => e.ZagIt).HasColumnName("zag_it");
            entity.Property(e => e.ZagItAv).HasColumnName("zag_it_av");
            entity.Property(e => e.Zbg).HasColumnName("_zbg");
            entity.Property(e => e.Zen).HasColumnName("_zen");

            entity.HasOne(d => d.BdNavigation).WithMany(p => p.Vetvs)
                .HasForeignKey(d => d.Bd)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("vetv_bd_fkey");

            entity.HasOne(d => d.IpNavigation).WithMany(p => p.VetvIpNavigations)
                .HasForeignKey(d => d.Ip)
                .HasConstraintName("vetv_ip_fkey");

            entity.HasOne(d => d.IqNavigation).WithMany(p => p.VetvIqNavigations)
                .HasForeignKey(d => d.Iq)
                .HasConstraintName("vetv_iq_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
