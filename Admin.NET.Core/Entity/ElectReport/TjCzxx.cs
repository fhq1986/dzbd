﻿// 麻省理工学院许可证
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


[SqlSugar.SugarTable("Tj_Dic_Czxx","车站信息")]
public class TjCzxx
{
    [SqlSugar.SugarColumn(IsPrimaryKey =true)]
    public int id { get; set; }
    public string Ddm { get; set; }
    public string Tjczdm { get; set; }
    public string Tjczmc { get; set; }
    public string Czdm { get; set; }  //线路图上的车站代码
    public Int32? Ord { get; set; }
    public string Jpm { get; set; }

    public string Dblm { get; set; }
    [SugarColumn(IsIgnore =true)]
    public string Czmc { get; set; }

    /// <summary>
    /// 继乘站电报略码,多站以逗号隔开
    /// </summary>
    public string Jczdblm { get; set; }

    /// <summary>
    /// 继乘站浮动小时
    /// </summary>
    public int? Fdxs { get; set; }

    public bool Bdbs { get; set; }

    public bool Wdbs { get; set; }

    public string Bdyx { get; set; }

    public string Wdyx { get; set; }
}
