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
/// 出勤信息
/// </summary>
[SugarTable("dat_cqxx")]
public class Cqxx
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    /// <summary>
    /// 乘务员代码1
    /// </summary>
    public string Cwydm1 { get; set; }
    /// <summary>
    /// 乘务员姓名1
    /// </summary>
    public string Cwyxm1 { get; set; }
    /// <summary>
    /// 职别代码1
    /// </summary>
    public string Zbdm1 { get; set; }
    /// <summary>
    /// 职别名称1
    /// </summary>
    public string Zbmc1 { get; set; }
    /// <summary>
    /// 乘务员代码2
    /// </summary>
    public string Cwydm2 { get; set; }
    /// <summary>
    /// 乘务员姓名2
    /// </summary>
    public string Cwyxm2 { get; set; }
    /// <summary>
    /// 职别代码2
    /// </summary>
    public string Zbdm2 { get; set; }
    /// <summary>
    /// 职别名称2
    /// </summary>
    public string Zbmc2 { get; set; }
    /// <summary>
    /// 乘务员代码3
    /// </summary>
    public string Cwydm3 { get; set; }
    /// <summary>
    /// 乘务员姓名3
    /// </summary>
    public string Cwyxm3 { get; set; }
    /// <summary>
    /// 职别代码3
    /// </summary>
    public string Zbdm3 { get; set; }
    /// <summary>
    /// 职别名称3
    /// </summary>
    public string Zbmc3 { get; set; }
    /// <summary>
    /// 乘务员代码4
    /// </summary>
    public string Cwydm4 { get; set; }
    /// <summary>
    /// 乘务员姓名4
    /// </summary>
    public string Cwyxm4 { get; set; }
    /// <summary>
    /// 职别代码4
    /// </summary>
    public string Zbdm4 { get; set; }
    /// <summary>
    /// 职别名称4
    /// </summary>
    public string Zbmc4 { get; set; }
    /// <summary>
    /// 乘务员代码5
    /// </summary>
    public string Cwydm5 { get; set; }
    /// <summary>
    /// 乘务员姓名5
    /// </summary>
    public string Cwyxm5 { get; set; }
    /// <summary>
    /// 职别代码5
    /// </summary>
    public string Zbdm5 { get; set; }
    /// <summary>
    /// 职别名称5
    /// </summary>
    public string Zbmc5 { get; set; }
    /// <summary>
    /// 乘务员代码6
    /// </summary>
    public string Cwydm6 { get; set; }
    /// <summary>
    /// 乘务员姓名6
    /// </summary>
    public string Cwyxm6 { get; set; }
    /// <summary>
    /// 职别代码6
    /// </summary>
    public string Zbdm6 { get; set; }
    /// <summary>
    /// 职别名称6
    /// </summary>
    public string Zbmc6 { get; set; }
    /// <summary>
    /// 出勤时间
    /// </summary>
    public string Cqsj { get; set; }
    /// <summary>
    /// 乘务作业点代码
    /// </summary>
    public string Cwzydddm { get; set; }
    /// <summary>
    /// 乘务作业点名称
    /// </summary>
    public string Cwzyddmc { get; set; }
    /// <summary>
    /// 乘务交路代码
    /// </summary>
    public string Cwjldm { get; set; }
    /// <summary>
    /// 乘务交路名称
    /// </summary>
    public string Cwjlmc { get; set; }
    /// <summary>
    /// 车次
    /// </summary>
    public string Cc { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机车号
    /// </summary>
    public string Jch { get; set; }
    /// <summary>
    /// 车间代码
    /// </summary>
    public string Cjdm { get; set; }
    /// <summary>
    /// 车间名称
    /// </summary>
    public string Cjmc { get; set; }
    /// <summary>
    /// 值乘方式代码
    /// </summary>
    public string Zcfsdm { get; set; }
    /// <summary>
    /// 值乘方式名称
    /// </summary>
    public string Zcfsmc { get; set; }
    /// <summary>
    /// 报单二维码
    /// </summary>
    public string Qrcode { get; set; }
}
