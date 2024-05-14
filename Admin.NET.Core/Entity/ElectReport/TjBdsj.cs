// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Entity.ElectReport;
[SugarTable("TJ_DAT_BDSJ")]
public class TjBdsj
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    //[Column("DDM")]
    public string Ddm { get; set; }
    //[Column("CJDM")]
    public string Cjdm { get; set; }
    //[Column("XTRQ")]
    public string Xtrq { get; set; }
    //[Column("BDH")]
    public int Bdh { get; set; }
    //[Column("BDRQ")]
    public string Bdrq { get; set; }
    //[Column("JX")]
    public string Jx { get; set; }
    //[Column("JCH")]
    public string Jch { get; set; }
    //[Column("JCZL")]
    public string Jczl { get; set; }
    //[Column("ZPD")]
    public string Zpd { get; set; }
    //[Column("PSD")]
    public string Psd { get; set; }
    //[Column("SJ1")]
    public string Sj1 { get; set; }
    //[Column("FSJ1")]
    public string Fsj1 { get; set; }
    //[Column("XY1")]
    public string Xy1 { get; set; }
    //[Column("SJ2")]
    public string Sj2 { get; set; }
    //[Column("FSJ2")]
    public string Fsj2 { get; set; }
    //[Column("XY2")]
    public string Xy2 { get; set; }
    //[Column("JIAY1")]
    public int? Jiay1 { get; set; }
    //[Column("JIEY1")]
    public int? Jiey1 { get; set; }
    //[Column("JIAOY1")]
    public int? Jiaoy1 { get; set; }
    //[Column("ZCD1")]
    public int? Zcd1 { get; set; }
    //[Column("JIAY2")]
    public int? Jiay2 { get; set; }
    //[Column("JIEY2")]
    public int? Jiey2 { get; set; }
    //[Column("JIAOY2")]
    public int? Jiaoy2 { get; set; }
    //[Column("ZCD2")]
    public int? Zcd2 { get; set; }
    //[Column("RLBL1")]
    public double? Rlbl1 { get; set; }
    //[Column("RLBL2")]
    public double? Rlbl2 { get; set; }
    //[Column("Cq")]
    public string Cq { get; set; }
    //[Column("TQ")]
    public string Tq { get; set; }
    //[Column("JIAOC")]
    public string Jiaoc { get; set; }
    //[Column("JIEC")]
    public string Jiec { get; set; }
    //[Column("CBD")]
    public string Cbd { get; set; }
    //[Column("RWD")]
    public string Rwd { get; set; }
    //[Column("CWD")]
    public string Cwd { get; set; }
    //[Column("RBD")]
    public string Rbd { get; set; }
    //[Column("BDFYY")]
    public string Bdfyy { get; set; }
    //[Column("WDFYY")]
    public string Wdfyy { get; set; }
    //[Column("JWTC")]
    public int? Jwtc { get; set; }
    //[Column("JWTS")]
    public int? Jwts { get; set; }
    //[Column("CZC")]
    public int? Czc { get; set; }
    //[Column("CZT")]
    public int? Czt { get; set; }
    //[Column("CZGL")]
    public int? Czgl { get; set; }
    //[Column("QZC")]
    public int? Qzc { get; set; }
    //[Column("QZT")]
    public int? Qzt { get; set; }
    //[Column("QZGL")]
    public int? Qzgl { get; set; }
    //[Column("ZZCS")]
    public double? Zzcs { get; set; }
    //[Column("CC")]
    public string Cc { get; set; }
    //[Column("GZM")]
    public string Gzm { get; set; }
    //[Column("QDM")]
    public string Qdm { get; set; }
    //[Column("QDGL")]
    public int? Qdgl { get; set; }
    //[Column("JLDM")]
    public string Jldm { get; set; }
    //[Column("CF")]
    public string Cf { get; set; }
    //[Column("DD")]
    public string Dd { get; set; }
    //[Column("ZJZ")]
    public string Zjz { get; set; }
    //[Column("DCDD")]
    public string Dcdd { get; set; }
    //[Column("DC")]
    public string Dc { get; set; }
    //[Column("ZXGL")]
    public int? Zxgl { get; set; }
    //[Column("ZZ")]
    public int? Zz { get; set; }
    //[Column("ZIZ")]
    public int? Ziz { get; set; }
    //[Column("ZAIZ")]
    public int? Zaiz { get; set; }
    //[Column("KC1")]
    public int? Kc1 { get; set; }
    //[Column("SSJ1")]
    public string Ssj1 { get; set; }
    //[Column("KC2")]
    public string Kc2 { get; set; }
    //[Column("SSJ2")]
    public string Ssj2 { get; set; }
    //[Column("ZC")]
    public int? Zc { get; set; }
    //[Column("KONGC")]
    public int? Kongc { get; set; }
    //[Column("FYY")]
    public int? Fyy { get; set; }
    //[Column("QT")]
    public int? Qt { get; set; }
    //[Column("HJ")]
    public int? Hj { get; set; }
    //[Column("HC")]
    public double? Hc { get; set; }
    //[Column("RLBZ")]
    public int? Rlbz { get; set; }
    //[Column("RLYL")]
    public int? Rlyl { get; set; }
    //[Column("MHSJ")]
    public string Mhsj { get; set; }
    //[Column("MHYL")]
    public int? Mhyl { get; set; }
    //[Column("MHBZ")]
    public int? Mhbz { get; set; }
    //[Column("CBBS")]
    public string Cbbs { get; set; }
    //[Column("CBGL")]
    public int? Cbgl { get; set; }
    //[Column("JIED")]
    public string Jied { get; set; }
    //[Column("Y_01")]
    public double? Y_01 { get; set; }
    //[Column("Y_02")]
    public double? Y_02 { get; set; }
    //[Column("Y_03")]
    public double? Y_03 { get; set; }
    //[Column("Y_04")]
    public double? Y_04 { get; set; }
    //[Column("Y_05")]
    public double? Y_05 { get; set; }
    //[Column("Y_06")]
    public double? Y_06 { get; set; }
    //[Column("Y_07")]
    public double? Y_07 { get; set; }
    //[Column("Y_08")]
    public double? Y_08 { get; set; }
    //[Column("Y_09")]
    public double? Y_09 { get; set; }
    //[Column("Y_10")]
    public double? Y_10 { get; set; }
    //[Column("Y_11")]
    public double? Y_11 { get; set; }
    //[Column("Y_12")]
    public double? Y_12 { get; set; }
    //[Column("Y_13")]
    public double? Y_13 { get; set; }
    //[Column("Y_14")]
    public double? Y_14 { get; set; }
    //[Column("Y_15")]
    public double? Y_15 { get; set; }
    //[Column("Y_16")]
    public double? Y_16 { get; set; }
    //[Column("QDYX")]
    public string Qdyx { get; set; }
    //[Column("JSJL")]
    public string Jsjl { get; set; }
    //[Column("SX")]
    public string Sx { get; set; }
    //[Column("BZCS")]
    public int Bzcs { get; set; }
    //[Column("CBJC")]
    public string Cbjc { get; set; }
    //[Column("DWYYL")]
    public int? Dwyyl { get; set; }
    //[Column("LCGD")]
    public int? Lcgd { get; set; }
    //[Column("DQDM")]
    public string Dqdm { get; set; }
    //[Column("QSYT1BWM")]
    public string Qsyt1bwm { get; set; }
    //[Column("CFZ")]
    public string Cfz { get; set; }
    //[Column("DDZ")]
    public string Ddz { get; set; }
    //[Column("CZDM")]
    public string Czdm { get; set; }
    //[Column("YQDM")]
    public string Yqdm { get; set; }
    //[Column("PPQDBS")]
    public int? Ppqdbs { get; set; }
    //[Column("ZH")]
    public string Zh { get; set; }
    //[Column("ZHBH")]
    public int? Zhbh { get; set; }
    //[Column("SJXM1")]
    public string Sjxm1 { get; set; }
    //[Column("FSJXM1")]
    public string Fsjxm1 { get; set; }
    //[Column("XYXM1")]
    public string Xyxm1 { get; set; }
    //[Column("SJXM2")]
    public string Sjxm2 { get; set; }
    //[Column("FSJXM2")]
    public string Fsjxm2 { get; set; }
    //[Column("XYXM2")]
    public string Xyxm2 { get; set; }
    //[Column("CFZDBM")]
    public string Cfzdbm { get; set; }
    //[Column("DDZDBM")]
    public string Ddzdbm { get; set; }
    //[Column("BZDM")]
    public string Bzdm { get; set; }
    //[Column("SFLS")]
    public int? Sfls { get; set; }
    //[Column("FQBS")]
    public DateTime Fqbs { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_cjdm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_cjdm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_cjdm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_cjdm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_cjdm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_cjdm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Tljdm { get; set; }
}
