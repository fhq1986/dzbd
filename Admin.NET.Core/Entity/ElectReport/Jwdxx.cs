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
[SugarTable("dic_jwd")]
public class Jwdxx
{

    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    /// <summary>
    /// 机务段代码
    /// </summary>
    public string Jwddm { get; set; }

    /// <summary>
    ///  机务段名称
    /// </summary>
    public string Jwdmc { get; set; }

    /// <summary>
    /// 机务编码
    /// </summary>
    public string Jwbm { get; set; }

    /// <summary>
    /// 
    /// 铁路局代码
    /// </summary>
    public string Tljdm { get; set; }

    /// <summary>
    /// 监控对应的配属段号
    /// </summary>
    public string JK_PSDH { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public string Ord { get; set; }

    /// <summary>
    /// 企业代码
    /// </summary>
    public string Qydm { get; set; }

    /// <summary>
    /// 电子报单是否走终端:1是 0否
    /// </summary>
    public string Zdbs { get; set; }

    /// <summary>
    /// 打印标识：1是 0否
    /// </summary>
    public string Dybs { get; set; }

    /// <summary>
    /// IC卡转储标识:1启用 0或null不启用
    /// </summary>
    public string Ickbs { get; set; }

    /// <summary>
    /// 是否获取运统1数据：1是 0否
    /// </summary>
    public string Yt1bs { get; set; }

    /// <summary>
    /// 列车供电是否启用:1启用 0或null不启用
    /// </summary>
    public string Lcgd { get; set; }

    /// <summary>
    /// 是否获取AEI（出入段）数据：1是 0否
    /// </summary>
    public string Aeibs { get; set; }

    /// <summary>
    /// 是否获取博飞能耗数据：1是 0否
    /// </summary>
    public string Bfnhbs { get; set; }
}
