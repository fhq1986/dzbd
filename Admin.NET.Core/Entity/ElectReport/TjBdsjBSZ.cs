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
[SugarTable("TJ_DAT_BDSJ_BSZ")]
public class TjBdsjBSZ
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string CJDM { get; set; }

    public string XTRQ { get; set; }

    public int BDH { get; set; }

    public string SJRQ { get; set; }

    public string BDRQ { get; set; }


    public string CF { get; set; }

    public string DD { get; set; }

    public string JX { get; set; }

    public string JCH { get; set; }

    public string JCZL { get; set; }

    public string ZPD { get; set; }


    public string PSD { get; set; }

    public string CC { get; set; }

    public string GZM { get; set; }

    public string QDM { get; set; }

    public int? ZZ { get; set; }

    public int? ZIZ { get; set; }

    public int? ZAIZ { get; set; }

    public int? KC1 { get; set; }

    public int? ZC { get; set; }

    public int? KONGC { get; set; }

    public int? FYY { get; set; }

    public int? QT { get; set; }

    public int? HJ { get; set; }

    public double? HC { get; set; }


    public string QDYX { get; set; }

    public string SX { get; set; }

    public string CFZ { get; set; }

    public string DDZ { get; set; }

    public string ZM { get; set; }

    public string FDBZ { get; set; }

    public int BZCS { get; set; }

    public string CFZDBM { get; set; }

    public string DDZDBM { get; set; }
}
