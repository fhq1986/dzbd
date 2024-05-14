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
public class Bdxxtj : AggregateRoot<int>
{
    /// <summary>
    /// 报单日期
    /// </summary>
    public string Bdrq { get; set; }

    /// <summary>
    /// 报单总数
    /// </summary>
    public int? Bdzs { get; set; }

    /// <summary>
    /// 未生成报单
    /// </summary>
    public int? Wsc { get; set; }

    /// <summary>
    /// 已生成报单
    /// </summary>
    public int? Ysc { get; set; }

    /// <summary>
    /// 正常报单
    /// </summary>
    public int? Zcbd { get; set; }

    // <summary>
    // 无运行信息
    // </summary>
    public int? Wyx { get; set; }

    /// <summary>
    /// 无燃料
    /// </summary>
    public int? Wrl { get; set; }

    /// <summary>
    /// 无数据
    /// </summary>
    public int? Wsj { get; set; }

    /// <summary>
    /// 未提交
    /// </summary>
    public int? Wtj { get; set; }

    /// <summary>
    /// 未回收
    /// </summary>
    public int? Whs { get; set; }

    /// <summary>
    /// 已统计
    /// </summary>
    public int? Ytongji { get; set; }

    /// <summary>
    /// 未统计
    /// </summary>
    public int? Wtongji { get; set; }

    /// <summary>
    /// 回收率
    /// </summary>
    public int? Recovery { get; set; }

}
