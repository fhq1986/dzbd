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
[SugarTable("Dic_cwyxx")]
public class Cwyxx
{
    [SugarColumn(IsPrimaryKey =true)]
    public int Id { get; set; }
    /// <summary>
    /// 车间代码
    /// </summary>
    public string Cjdm { get; set; }

    /// <summary>
    /// 乘务员代码
    /// </summary>
    public string Cwydm { get; set; }

    /// <summary>
    /// 工资代码
    /// </summary>
    public string Gzdm { get; set; }

    /// <summary>
    /// 乘务员名称
    /// </summary>
    public string Cwyxm { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 职别代码
    /// </summary>
    public string Zbdm { get; set; }

    /// <summary>
    /// 交路代码
    /// </summary>
    public string Jldm { get; set; }

    /// <summary>
    /// 简拼码
    /// </summary>
    public string Jpm { get; set; }

    /// <summary>
    /// 乘务员登录密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 乘务员归属地点代码
    /// </summary>
    public string Cwzydddm { get; set; }

    /// <summary>
    /// 机务段代码
    /// </summary>
    public string Ddm { get; set; }

    /// <summary>
    /// 车队代码
    /// </summary>
    public string CheDuiCode { get; set; }
    /// <summary>
    /// 岗位代码
    /// </summary>
    public string GangWeiCode { get; set; }
    /// <summary>
    /// 职位代码
    /// </summary>
    public string ZhiWeiCode { get; set; }
    /// <summary>
    /// 运输种别
    /// </summary>
    public string Gzzb { get; set; }
    /// <summary>
    /// 入职日期
    /// </summary>
    public string Rzrq { get; set; }
    /// <summary>
    /// 是否计件
    /// </summary>
    public string Isjj { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remarks { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 职别名称
    /// </summary>
    public string Zbmc { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 岗位名称
    /// </summary>
    public string Gwmc { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 车队名称
    /// </summary>
    public string Cdmc { get; set; }

    /// <summary>
    /// 乘务区段代码
    /// </summary>
    public string Cwqddm { get; set; }

    /// <summary>
    /// 动车组标识
    /// </summary>
    public string Dczbs { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 计工代码是否启用
    /// </summary>
    public string Jgdm { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 是否获取运统1数据：1是 0否
    /// </summary>
    public string Yt1bs { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 终端标识：1是 0否
    /// </summary>
    public string Zdbs { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 打印标识：1是 0否
    /// </summary>
    public string Dybs { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// IC卡转储标识:1启用 0或null不启用
    /// </summary>
    public string Ickbs { get; set; }
    [SugarColumn(IsIgnore = true)]
    /// <summary>
    /// 报单发放是否启用
    /// </summary>
    public string Bdff { get; set; }
    /// <summary>
    /// 列车供电是否启用:1启用 0或null不启用
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string Lcgd { get; set; }

    /// <summary>
    /// 纸张尺寸：默认A4，可选择B5
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string PageSize { get; set; }
}
