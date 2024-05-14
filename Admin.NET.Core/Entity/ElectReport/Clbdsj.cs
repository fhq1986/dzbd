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
[SugarTable("dat_clbdsj")]
public class Clbdsj
{
    public int Id { get; set; }
    /// <summary>
    /// 报单数据id
    /// </summary>
    public int? Bdsjid { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机车号
    /// </summary>
    public string Jch { get; set; }
    /// <summary>
    /// 接收量1
    /// </summary>
    public int? Jsl1 { get; set; }
    /// <summary>
    /// 加入量1
    /// </summary>
    public int? Jrl1 { get; set; }
    /// <summary>
    /// 交出量1
    /// </summary>
    public int? Jcl1 { get; set; }
    /// <summary>
    /// 再生电接收量1
    /// </summary>
    public int? Zsdjsl1 { get; set; }
    /// <summary>
    /// 再生电交出量1
    /// </summary>
    public int? Zsdjcl1 { get; set; }
    /// <summary>
    /// 列车供电接收量1
    /// </summary>
    public int? Lcgdjsl1 { get; set; }
    /// <summary>
    /// 列车供电交出量1
    /// </summary>
    public int? Lcgdjcl1 { get; set; }
    /// <summary>
    /// 接收量2
    /// </summary>
    public int? Jsl2 { get; set; }
    /// <summary>
    /// 加入量2
    /// </summary>
    public int? Jrl2 { get; set; }
    /// <summary>
    /// 交出量2
    /// </summary>
    public int? Jcl2 { get; set; }
    /// <summary>
    /// 再生电接收量2
    /// </summary>
    public int? Zsdjsl2 { get; set; }
    /// <summary>
    /// 再生电交出量2
    /// </summary>
    public int? Zsdjcl2 { get; set; }
    /// <summary>
    /// 列车供电接收量2
    /// </summary>
    public int? Lcgdjsl2 { get; set; }
    /// <summary>
    /// 列车供电交出量2
    /// </summary>
    public int? Lcgdjcl2 { get; set; }
    /// <summary>
    /// 打温时间
    /// </summary>
    public string Dwsj { get; set; }
    /// <summary>
    /// 打温用油量
    /// </summary>
    public int? Dwyl { get; set; }

    /// <summary>
    /// 列车供电接收量1_2
    /// </summary>
    public int? Lcgdjsl1_2 { get; set; }
    /// <summary>
    /// 列车供电交出量1_2
    /// </summary>
    public int? Lcgdjcl1_2 { get; set; }
    /// <summary>
    /// 列车供电接收量2_2
    /// </summary>
    public int? Lcgdjsl2_2 { get; set; }
    /// <summary>
    /// 列车供电交出量2_2
    /// </summary>
    public int? Lcgdjcl2_2 { get; set; }

    /// <summary>
    /// 电表是否损坏(A端)：是，否
    /// </summary>
    public string Dbsfsh1 { get; set; }
    /// <summary>
    /// 电表是否损坏(B端)：是，否
    /// </summary>
    public string Dbsfsh2 { get; set; }
    [SugarColumn(IsIgnore =true)]
    public string Jcxh
    {
        get
        {
            return Jx + "-" + Jch;
        }
    }
}
