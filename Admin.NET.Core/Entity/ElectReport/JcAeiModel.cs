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
[SugarTable("dat_jcaei")]
public class JcAeiModel : AggregateRoot<int>
{

    public string AEI_CJBM { get; set; }
    public string BGRQ { get; set; }
    public string BGSJ { get; set; }
    public string BTXX { get; set; }
    public string BWBBH { get; set; }
    public string BWBZ { get; set; }
    public string BWM { get; set; }
    public string CC { get; set; }
    public string CDRD { get; set; }
    public string CJD_AEI { get; set; }
    public string CJD_DBLM { get; set; }
    public string CJD_TXBH { get; set; }
    public string CJD_ZB { get; set; }
    public string DBLM { get; set; }
    public string DDRQ { get; set; }
    public string DDSJ { get; set; }
    public string DSBZ { get; set; }
    public string FILE_NAME { get; set; }
    public int FIRST_LS { get; set; }
    public string ID { get; set; }
    public DateTime IN_DB_TIME { get; set; }
    public int JCZLS { get; set; }
    public string KH { get; set; }
    public int LAST_LS { get; set; }
    public int MIDDLE_LS { get; set; }
    public string SFR { get; set; }
    public string TGRQ { get; set; }
    public string TGSJ { get; set; }
    public string YL { get; set; }
}
