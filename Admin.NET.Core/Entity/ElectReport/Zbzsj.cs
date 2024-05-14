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
[SugarTable("dat_zbzsj")]
public class Zbzsj
{
    [SugarColumn(IsPrimaryKey =true)]
    public int Id { get; set; }
    /// <summary>
    /// 编组类型 返回的是"集"、"特"、"行"，我们库里保存的是1，2，3，这里需要处理。"合计"、"企"、"国铁"等不需要保存
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public string Yt1Bzlxmc { get; set; }
    /// <summary>
    /// 总重
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public double? Yt1Zz { get; set; }
    /// <summary>
    /// 编组类型
    /// </summary>
    public int? Bzlx { get; set; }
    /// <summary>
    /// 总重
    /// </summary>
    public int? Zz { get; set; }
    /// <summary>
    /// 载重
    /// </summary>
    public int? Zaiz { get; set; }
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
    [SugarColumn(IsIgnore =true)]
    public double? Yt1Hc { get; set; }
    /// <summary>
    /// 换长
    /// </summary>
    public int? Hc { get; set; }
    [SugarColumn(IsIgnore =true)]
    public string Hc10
    {
        get { return Hc == null || Hc <= 0 ? null : ((double.Parse(Hc.ToString()) / 10).ToString()); }
    }
    /// <summary>
    /// 编组数据id
    /// </summary>
    public int? Bzsjid { get; set; }
    /// <summary>
    /// 编组类型名称
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public string Bzlxmc
    {
        get
        {
            if (Bzlx.HasValue)
            {
                switch (Bzlx.Value)
                {
                    case 1:
                        return "集";
                    case 2:
                        return "特";
                    case 3:
                        return "行";
                    default:
                        return "";
                }
            }
            else
            {
                return "";
            }

        }
    }

    /// <summary>
    /// 编组类型名称
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public int? Yt1Bzlx
    {
        get
        {
            switch (Yt1Bzlxmc)
            {
                case "集":
                    return 1;
                case "集装箱公司":
                    return 1;
                case "特":
                    return 2;
                case "特货公司":
                    return 2;
                case "行":
                    return 3;
                case "其中：快运公司":
                    return 3;
                case "合计":
                    return 0;
                case "企":
                    return 4;
                case "国铁":
                    return 5;
                default:
                    return -1;
            }
        }
    }
}
