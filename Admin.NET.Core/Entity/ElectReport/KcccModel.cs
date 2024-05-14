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
[SugarTable("DIC_KCCC")]
public class KcccModel
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string Lrcc { get; set; }
    public string Kccc { get; set; }
    public string Gzm { get; set; }
    public string Ssj { get; set; }
    public Int32 Zz { get; set; }
    public Int32 Kc { get; set; }
    public Int32 Xlzz { get; set; }
    public double? Hc { get; set; }
    public string Qdm { get; set; }
    public string Cfz { get; set; }
    public string Ddz { get; set; }
    public string Tjzbm { get; set; }
    public string Jlrq { get; set; }
    public string Sfcc { get; set; }
    public string Sfsc { get; set; }
    public string Ddsc { get; set; }
    public string Kccc2 { get; set; }
    public string Ktsx { get; set; }

}
