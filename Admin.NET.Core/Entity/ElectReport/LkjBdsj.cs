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

public class LkjBdsj : AggregateRoot<int>
{
    public string Wjjlsj { get; set; }
    public string Wjjssj { get; set; }
    public string Scwjjlsj { get; set; }
    public string Sccc { get; set; }
    public string Sjh { get; set; }
    public string Sjxm { get; set; }
    public string Fsjh { get; set; }
    public string Fsjxm { get; set; }
    public string Psdh { get; set; }
    public string Jclxbs { get; set; }
    public string Jcxh { get; set; }
    public string Jxmc { get; set; }
    public string Jch { get; set; }
    public string Jcabj { get; set; }
    public string Cc { get; set; }
    public string Khlx { get; set; }
    public string Khbb { get; set; }
    public int Zz { get; set; }
    public int Zaiz { get; set; }
    public int Kc { get; set; }
    public int Zc { get; set; }
    public int Kongc { get; set; }
    public int Fyyc { get; set; }
    public int Dck { get; set; }
    public int Sc { get; set; }
    public int Ls { get; set; }
    public int Hc { get; set; }
    public string Cd { get; set; }
    public string Rd { get; set; }
    public string Sfsjjlh { get; set; }
    public string Sfsrjlh { get; set; }
    public string Sfczh { get; set; }
    public string Sfczmc { get; set; }
    public string Zdsjjlh { get; set; }
    public string Zdsrjlh { get; set; }
    public string Zdczh { get; set; }
    public string Zdczmc { get; set; }
    public int Ickzccs { get; set; }
    public string Ickzcsj { get; set; }
    public string Ickzcsj_min { get; set; }
    public int Zcqzccs { get; set; }
    public string Zcqzcsj { get; set; }
    public string Orgfile { get; set; }
    public string Filename { get; set; }
    public string Jkjldwdm { get; set; }
    public string Bdrksj { get; set; }
    public string Ddm { get; set; }
    public List<LkjBdsjBz> bzs { get; set; }
}
public class RequestForBdsj
{
    public Bdsj bdsj { get; set; }
}