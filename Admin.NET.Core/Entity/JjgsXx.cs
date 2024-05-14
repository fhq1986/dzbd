// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Entity;
public class JjgsXx : AggregateRoot<int>
{
    /// <summary>
    /// 司机代码
    /// </summary>

    public string GongHao { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>

    public string ChWuYuName { get; set; }

    /// <summary>
    /// 车间
    /// </summary>

    public string CheJian { get; set; }

    /// <summary>
    /// 车队
    /// </summary>

    public string CheDui { get; set; }

    /// <summary>
    /// 职别
    /// </summary>

    public string ZhiBie { get; set; }

    /// <summary>
    /// 担当职称
    /// </summary>

    public string GangWei { get; set; }

    /// <summary>
    /// 统计开始时间
    /// </summary>

    public string TongJiKssj { get; set; }

    /// <summary>
    /// 统计结束时间
    /// </summary>

    public string TongJiJssj { get; set; }

    /// <summary>
    /// 车次
    /// </summary>
    public string CheCi { get; set; }

    /// <summary>
    /// 总工时
    /// </summary>

    public decimal GongShi { get; set; }

    /// <summary>
    /// 趟数
    /// </summary>
    public decimal TangShu { get; set; }

    /// <summary>
    /// 应发工资
    /// </summary>
    public decimal YingFaGZ { get; set; }

    /// <summary>
    /// 统计月份
    /// </summary>
    public string Cxyf { get; set; }
}
