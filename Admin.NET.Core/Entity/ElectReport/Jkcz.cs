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
[SugarTable("dic_jkcz")]
public class Jkcz
{

    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    /// <summary>
    /// 监控交路单位代码
    /// </summary>
    public string Jkjldwdm { get; set; }

    /// <summary>
    /// 监控交路号
    /// </summary>
    public string Jkjlh { get; set; }

    /// <summary>
    /// 监控车站号
    /// </summary>
    public string Jkczh { get; set; }

    /// <summary>
    /// 监控车站名称
    /// </summary>
    public string Jkczmc { get; set; }

    /// <summary>
    /// 车站代码
    /// </summary>
    public string Czdm { get; set; }

    /// <summary>
    /// 统计车站名称
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string Tjczmc { get; set; }

    /// <summary>
    /// 电报略码
    /// </summary>
    public string Dblm { get; set; }

    /// <summary>
    /// 机务段代码
    /// </summary>
    public string Ddm { get; set; }

    /// <summary>   
    /// 专用线标识 1专用线 ， 0国有线
    /// </summary>
    public string Zyxbs { get; set; }

    /// <summary>
    /// 继乘站电报略码,多站以逗号隔开
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string Jczdblm { get; set; }

    /// <summary>
    /// 继乘站浮动小时
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public string Fdxs { get; set; }
    [SugarColumn(IsIgnore = true)]
    public bool Bdbs { get; set; }
    [SugarColumn(IsIgnore = true)]
    public bool Wdbs { get; set; }
    [SugarColumn(IsIgnore = true)]
    public string Bdyx { get; set; }
    [SugarColumn(IsIgnore = true)]
    public string Wdyx { get; set; }
}
