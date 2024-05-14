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
/// 出退勤作业设置
/// </summary>
[SugarTable("Dic_ChuTuiqinzy")]
public class ChuTuiqinzy
{
    [SqlSugar.SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    /// <summary>
    /// 车站名称 
    /// </summary>
    public string ZhanMingmc { set; get; }
    /// <summary>
    /// 车站代码
    /// </summary>
    public string ZhanMingbm { set; get; }
    /// <summary>
    ///段代码
    /// </summary>
    public string Ddm { set; get; }
    /// <summary>
    ///实际出勤
    /// </summary>
    public int ShiJicq { set; get; }
    /// <summary>
    ///继乘出勤
    /// </summary>
    public int JiChengcq { set; get; }
    /// <summary>
    ///出库出勤
    /// </summary>
    public int ChuKucq { set; get; }
    /// <summary>
    ///实际退勤
    /// </summary>
    public int ShiJitq { set; get; }
    /// <summary>
    ///继乘退勤
    /// </summary>
    public int JiChengtq { set; get; }
    /// <summary>
    ///入库退勤
    /// </summary>
    public int RuKutq { set; get; }
    /// <summary>
    /// 计算方式(出勤)
    /// </summary>
    public string Cqjisuanfs { set; get; }
    /// <summary>
    /// 计算方式(退勤)
    /// </summary>
    public string Tqjisuanfs { set; get; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { set; get; }
}
