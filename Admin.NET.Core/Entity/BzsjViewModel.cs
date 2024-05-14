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
/// <summary>
/// 编组数据
/// </summary>
public class BzsjViewModel
{
    /// <summary>
    /// 车次
    /// </summary>
    public string Cc { get; set; }
    /// <summary>
    /// 客货类型
    /// </summary>
    public string Khlx { get; set; }
    /// <summary>
    /// 客货本补
    /// </summary>
    public string Khbb { get; set; }
    /// <summary>
    /// 总重
    /// </summary>
    public double? Zz { get; set; }
    /// <summary>
    /// 载重
    /// </summary>
    public double? Zaiz { get; set; }
    /// <summary>
    /// 客车
    /// </summary>
    public int? Kc { get; set; }
    /// <summary>
    /// 担当企业
    /// </summary>
    public string Ddqy { get; set; }
    /// <summary>
    /// 其中加挂
    /// </summary>
    public string Qzjg { get; set; }
    /// <summary>
    /// 重车
    /// </summary>
    public int? Zc { get; set; }
    /// <summary>
    /// 空车
    /// </summary>
    public int? Kongc { get; set; }
    /// <summary>
    /// 非运用
    /// </summary>
    public int? Fyyc { get; set; }
    /// <summary>
    /// 代客车
    /// </summary>
    public int? Dkc { get; set; }
    /// <summary>
    /// 其它
    /// </summary>
    public int? Qt { get; set; }
    /// <summary>
    /// 辆数合计
    /// </summary>
    public int? Ls { get; set; }
    /// <summary>
    /// 换长
    /// </summary>
    public int? Hc { get; set; }
    /// <summary>
    /// 到达
    /// </summary>
    public string Dd { get; set; }
    /// <summary>
    /// 出发
    /// </summary>
    public string Cf { get; set; }
    ///// <summary>
    ///// 停车时间
    ///// </summary>
    //public string Tcsj { get; set; }
    /// <summary>
    /// 机外停车时间
    /// </summary>
    public string Jwtcsj { get; set; }
    /// <summary>
    /// 区间停车时间
    /// </summary>
    public string Qjtcsj { get; set; }
    /// <summary>
    /// 实际交路号
    /// </summary>
    public string Sjjlh { get; set; }
    /// <summary>
    /// 输入交路号
    /// </summary>
    public string Srjlh { get; set; }
    /// <summary>
    /// 车站号
    /// </summary>
    public string Czh { get; set; }
    /// <summary>
    /// 车站名称
    /// </summary>
    public string Czmc { get; set; }
    /// <summary>
    /// 区段公里
    /// </summary>
    public int? Qjgl { get; set; }
    /// <summary>
    /// 调车时间
    /// </summary>
    public string Dcsj { get; set; }
    /// <summary>
    /// 调车方式
    /// </summary>
    public string Dcfs { get; set; }
    /// <summary>
    /// 入段
    /// </summary>
    public string Rd { get; set; }
    /// <summary>
    /// 出段
    /// </summary>
    public string Cd { get; set; }
    /// <summary>
    /// 监控单位
    /// </summary>
    public string Jkdw { get; set; }
    /// <summary>
    /// 重联机车
    /// </summary>
    public string Cljc { get; set; }
    /// <summary>
    /// 编组次数
    /// </summary>
    public int? Bzcs { get; set; }
    /// <summary>
    /// 确报数据id
    /// </summary>
    public string Qbsjid { get; set; }
    /// <summary>
    /// 清算运统1数据id
    /// </summary>
    public string Qsyt1id { get; set; }
    /// <summary>
    /// 客编数据id
    /// </summary>
    public string Kbsjid { get; set; }
    /// <summary>
    /// 报单数据id
    /// </summary>
    public int? Bdsjid { get; set; }
    /// <summary>
    /// 子编组数据
    /// </summary>
    public IList<Zbzsj> zbzsjs { get; set; }
}
