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
/// <summary>
/// 统计报单信息
/// </summary>
[SugarTable("TJ_DAT_BDXX")]
public class TjBdxx
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string Ddm { get; set; }
    public string Cjdm { get; set; }
    public string Xtrq { get; set; }
    public int Bdh { get; set; }
    public string Bdrq { get; set; }
    public string Jx { get; set; }
    public string Jch { get; set; }
    public string Jczl { get; set; }
    public string Zpd { get; set; }
    public string Psd { get; set; }
    public string Sj1 { get; set; }
    public string Fsj1 { get; set; }
    public string Xy1 { get; set; }
    public string Sj2 { get; set; }
    public string Fsj2 { get; set; }
    public string Xy2 { get; set; }
    public int? Jiay1 { get; set; }
    public int? Jiey1 { get; set; }
    public int? Jiaoy1 { get; set; }
    public int? Zcd1 { get; set; }
    public int? Jiay2 { get; set; }
    public int? Jiey2 { get; set; }
    public int? Jiaoy2 { get; set; }
    public int? Zcd2 { get; set; }
    public double? Rlbl1 { get; set; }
    public double? Rlbl2 { get; set; }
    public string Cq { get; set; }
    public string Tq { get; set; }
    public string Jiaoc { get; set; }
    public string Jiec { get; set; }
    public string Cbd { get; set; }
    public string Rwd { get; set; }
    public string Cwd { get; set; }
    public string Rbd { get; set; }
    public string Bdfyy { get; set; }
    public string Wdfyy { get; set; }
    public int? Jwtc { get; set; }
    public int? Jwts { get; set; }
    public int? Czc { get; set; }
    public int? Czt { get; set; }
    public int? Czgl { get; set; }
    public int? Qzc { get; set; }
    public int? Qzt { get; set; }
    public int? Qzgl { get; set; }
    public double? Zzcs { get; set; }
    public string Cc { get; set; }
    public string Gzm { get; set; }
    public string Qdm { get; set; }
    public int? Qdgl { get; set; }
    public string Jldm { get; set; }
    public string Cf { get; set; }
    public string Dd { get; set; }
    public string Zjz { get; set; }
    public string Dcdd { get; set; }
    public string Dc { get; set; }
    public int? Zxgl { get; set; }
    public int? Zz { get; set; }
    public int? Zzdgl { get; set; }
    public int? Zzdglfp { get; set; }
    public int? Ziz { get; set; }
    public int? Zizdgl { get; set; }
    public int? Zaiz { get; set; }
    public int? Zaizdgl { get; set; }
    public int? Zaizdglfp { get; set; }
    public int? Kc1 { get; set; }
    public int? Kc1lgl { get; set; }
    public string Ssj1 { get; set; }
    public string Kc2 { get; set; }
    public string Ssj2 { get; set; }
    public int? Zc { get; set; }
    public int? Zclgl { get; set; }
    public int? Kongc { get; set; }
    public int? Kongclgl { get; set; }
    public int? Fyy { get; set; }
    public int? Fyylgl { get; set; }
    public int? Qt { get; set; }
    public int? Qtlgl { get; set; }
    public int? Hj { get; set; }
    public int? Hjlgl { get; set; }
    public double? Hc { get; set; }
    public int? Rlbz { get; set; }
    public int? Rlyl { get; set; }
    public string Cbbs { get; set; }
    public int? Cbgl { get; set; }
    public string Jied { get; set; }
    public double? Y_01 { get; set; }
    public double? Y_02 { get; set; }
    public double? Y_03 { get; set; }
    public double? Y_04 { get; set; }
    public double? Y_05 { get; set; }
    public double? Y_06 { get; set; }
    public double? Y_07 { get; set; }
    public double? Y_08 { get; set; }
    public double? Y_09 { get; set; }
    public double? Y_10 { get; set; }
    public double? Y_11 { get; set; }
    public double? Y_12 { get; set; }
    public double? Y_13 { get; set; }
    public double? Y_14 { get; set; }
    public double? Y_15 { get; set; }
    public string Jsjl { get; set; }
    public string Qdyx { get; set; }
    public string Sx { get; set; }
    public int Bzcs { get; set; }
    public double? Qzzsj { get; set; }
    public double? Cyzsj { get; set; }
    public double? Zjtsj { get; set; }
    public double? Bdtsj { get; set; }
    public double? Bztsj { get; set; }
    public double? Wdtsj { get; set; }
    public double? Wztsj { get; set; }
    public decimal? Dcsj { get; set; } //20220516  改为decimal
    public int? Dcgl { get; set; }
    public int? Yhgl { get; set; }
    public int? Jcgl { get; set; }
    public string Snbz { get; set; }
    public string Cyfs { get; set; }
    public int? Djc { get; set; }
    public int? Djgl { get; set; }
    public int? Ls { get; set; }
    public int? Fjzzftlcs { get; set; }
    public double? Mhsj { get; set; }
    public int? Mhyl { get; set; }
    public int? Mhbz { get; set; }
    public string Cbjc { get; set; }
    public int? Dwyyl { get; set; }
    public int? Lcgd { get; set; }
    public string Dqdm { get; set; }
    public int? Qzc_ztzd { get; set; }
    public int? Qzt_ztzd { get; set; }
    public int? Qzgl_ztzd { get; set; }
    public string Ktsx { get; set; }
    public int? Yzc { get; set; }
    public int? Yzclgl { get; set; }
    public int? Rzc { get; set; }
    public int? Rzclgl { get; set; }
    public int? Ywc { get; set; }
    public int? Ywclgl { get; set; }
    public int? Rwc { get; set; }
    public int? Rwclgl { get; set; }
    public int? Xlc { get; set; }
    public int? Xlclgl { get; set; }
    public int? Uzc { get; set; }
    public int? Uzclgl { get; set; }
    public int? Cac { get; set; }
    public int? Caclgl { get; set; }
    public int? Kqt { get; set; }
    public int? Kqtlgl { get; set; }
    public int? Huc { get; set; }
    public int? Huclgl { get; set; }
    public int? Hqt { get; set; }
    public int? Hqtlgl { get; set; }
    public int? Ztky_xlc { get; set; }
    public int? Ztky_xlclgl { get; set; }
    public int? Ztky_xlczz { get; set; }
    public int? Ztky_xlczzdgl { get; set; }
    public int? Tlj_xlc { get; set; }
    public int? Tlj_xlclgl { get; set; }
    public int? Tlj_xlczz { get; set; }
    public int? Tlj_xlczzdgl { get; set; }
    public int? Qt_xlc { get; set; }
    public int? Qt_xlclgl { get; set; }
    public int? Qt_xlczz { get; set; }
    public int? Qt_xlczzdgl { get; set; }
    public double? Cqzjcsj { get; set; }
    public double? Jczkcsj { get; set; }
    public double? Jczbdsj { get; set; }
    public double? Jczwdsj { get; set; }
    public double? Bdzkcsj { get; set; }
    public double? Wdzkcsj { get; set; }
    public double? Ddzbdsj { get; set; }
    public double? Ddzwdsj { get; set; }
    public double? Bdzjcsj { get; set; }
    public double? Wdzjcsj { get; set; }
    public double? Ddzjcsj { get; set; }
    public double? Jcztqsj { get; set; }
    public string Rzd { get; set; }
    public double? Jczjdsj { get; set; }
    public int? Zyys_zizdgl { get; set; }
    public int? Zyys_zzrlyl { get; set; }
    public int? Zyys_zizrlyl { get; set; }
    public int? Zyys_zzdglfp { get; set; }
    public string Zzpd { get; set; }
    public string Qsyt1bwm { get; set; }
    public string Yqdm { get; set; }
    public string Zh { get; set; }
    public int? Zhbh { get; set; }
    public string Sjxm1 { get; set; }
    public string Fsjxm1 { get; set; }
    public string Xyxm1 { get; set; }
    public string Sjxm2 { get; set; }
    public string Fsjxm2 { get; set; }
    public string Xyxm2 { get; set; }
    public int? Yzcd { get; set; }
    public int? Yzcdgl { get; set; }
    public int? Rzcd { get; set; }
    public int? Rzcdgl { get; set; }
    public int? Ywcd { get; set; }
    public int? Ywcdgl { get; set; }
    public int? Rwcd { get; set; }
    public int? Rwcdgl { get; set; }
    public int? Xlcd { get; set; }
    public int? Xlcdgl { get; set; }
    public int? Uzcd { get; set; }
    public int? Uzcdgl { get; set; }
    public int? Cacd { get; set; }
    public int? Cacdgl { get; set; }
    public int? Kqtd { get; set; }
    public int? Kqtdgl { get; set; }
    public int? Hucd { get; set; }
    public int? Hucdgl { get; set; }
    public int? Hqtd { get; set; }
    public int? Hqtdgl { get; set; }
    public int? Uzcd1 { get; set; }
    public int? Uzcdgl1 { get; set; }
    public int? Uzcd2 { get; set; }
    public int? Uzcdgl2 { get; set; }
    public int? Uzcd3 { get; set; }
    public int? Uzcdgl3 { get; set; }
    public int? Uzcd4 { get; set; }
    public int? Uzcdgl4 { get; set; }
    public double? Yjxs { get; set; }
    public string Bzdm { get; set; }
    public int? Sfls { get; set; }
    public int? Zcdfp { get; set; }
    public int? Lcgdfp { get; set; }
    public string Sfrq { get; set; }
    public string Sfcc { get; set; }
    public string Kybz { get; set; }
    public int? Xlc1 { get; set; }
    public int? Xlclgl1 { get; set; }
    public int? Xlc2 { get; set; }
    public int? Xlclgl2 { get; set; }
    public int? Xlc3 { get; set; }
    public int? Xlclgl3 { get; set; }
    public int? Xlc4 { get; set; }
    public int? Xlclgl4 { get; set; }
    public DateTime Fqbs { get; set; }

    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj1_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj1_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy1_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Sj2_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Fsj2_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_cwqddm { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_cheduicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xy2_gangweicode { get; set; }
    [SqlSugar.SugarColumn(IsIgnore = true)]
    public string Xldm { get; set; }
}
