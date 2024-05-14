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
/// 车务站段字典信息
/// </summary>
[SugarTable("dic_zdgl")]
public class Zdgl
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    /// <summary>
    /// 终端mac地址
    /// </summary>
    public string Zddm { get; set; }

    /// <summary>
    /// 终端名称
    /// </summary>
    public string Zdmc { get; set; }

    /// <summary>
    /// 终端IP地址
    /// </summary>
    public string Zdpath { get; set; }

    /// <summary>
    /// 乘务作业地点代码
    /// </summary>
    public string Cwzydddm { get; set; }
    /// <summary>
    /// 乘务作业地点名称
    /// </summary>
    public string Cwzyddmc { get; set; }
    /// <summary>
    /// 活动时间
    /// </summary>
    public string Hdsj { get; set; }
    /// <summary>
    /// 乘务员代码
    /// </summary>
    public string Cwydm { get; set; }
    /// <summary>
    /// 乘务员姓名
    /// </summary>
    public string Cwyxm { get; set; }
    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 段代码
    /// </summary>
    public string Ddm { get; set; }
}
