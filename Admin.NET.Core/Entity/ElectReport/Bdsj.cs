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
[SugarTable("dat_bdsj")]
public class Bdsj: AggregateRoot<int>
{
    /// <summary>
    /// 司机代码1
    /// </summary>
    public string Sjdm1 { get; set; }
    /// <summary>
    /// 司机姓名1
    /// </summary>
    public string Sjxm1 { get; set; }
    /// <summary>
    /// 副司机代码1
    /// </summary>
    public string Fsjdm1 { get; set; }
    /// <summary>
    /// 副司机姓名1
    /// </summary>
    public string Fsjxm1 { get; set; }
    /// <summary>
    /// 学员代码1
    /// </summary>
    public string Xydm1 { get; set; }
    /// <summary>
    /// 学员姓名1
    /// </summary>
    public string Xyxm1 { get; set; }
    /// <summary>
    /// 司机代码2
    /// </summary>
    public string Sjdm2 { get; set; }
    /// <summary>
    /// 司机姓名2
    /// </summary>
    public string Sjxm2 { get; set; }
    /// <summary>
    /// 副司机代码2
    /// </summary>
    public string Fsjdm2 { get; set; }
    /// <summary>
    /// 副司机姓名2
    /// </summary>
    public string Fsjxm2 { get; set; }
    /// <summary>
    /// 学员代码2
    /// </summary>
    public string Xydm2 { get; set; }
    /// <summary>
    /// 学员姓名2
    /// </summary>
    public string Xyxm2 { get; set; }
    /// <summary>
    /// 乘务员1职别
    /// </summary>
    public string Cwyzb1 { get; set; }
    /// <summary>
    /// 乘务员2职别
    /// </summary>
    public string Cwyzb2 { get; set; }
    /// <summary>
    /// 乘务员3职别
    /// </summary>
    public string Cwyzb3 { get; set; }
    /// <summary>
    /// 乘务员4职别
    /// </summary>
    public string Cwyzb4 { get; set; }
    /// <summary>
    /// 乘务员5职别
    /// </summary>
    public string Cwyzb5 { get; set; }
    /// <summary>
    /// 乘务员6职别
    /// </summary>
    public string Cwyzb6 { get; set; }
    /// <summary>
    /// 值乘方式代码
    /// </summary>
    public string Zcfsdm { get; set; }
    /// <summary>
    /// 值乘方式名称
    /// </summary>
    public string Zcfsmc { get; set; }
    /// <summary>
    /// 报单类型代码
    /// </summary>
    public string Bdlxdm { get; set; }
    /// <summary>
    /// 报单类型名称
    /// </summary>
    public string Bdlxmc { get; set; }
    /// <summary>
    /// 支配局代码
    /// </summary>
    public string Zpjdm { get; set; }
    /// <summary>
    /// 支配局名称
    /// </summary>
    public string Zpjmc { get; set; }
    /// <summary>
    /// 支配段代码
    /// </summary>
    public string Zpddm { get; set; }
    /// <summary>
    /// 支配段名称
    /// </summary>
    public string Zpdmc { get; set; }
    /// <summary>
    /// 录入局代码
    /// </summary>
    public string Lrjdm { get; set; }
    /// <summary>
    /// 录入局名称
    /// </summary>
    public string Lrjmc { get; set; }
    /// <summary>
    /// 录入段代码
    /// </summary>
    public string Lrddm { get; set; }
    /// <summary>
    /// 录入段名称
    /// </summary>
    public string Lrdmc { get; set; }
    /// <summary>
    /// 机车种类
    /// </summary>
    public string Jczl { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机车号
    /// </summary>
    public string Jch { get; set; }
    /// <summary>
    /// 无人重联机型
    /// </summary>
    public string Jx_Wrcl { get; set; }
    /// <summary>
    /// 无人重联机车号
    /// </summary>
    public string Jch_Wrcl { get; set; }
    /// <summary>
    /// 报单日期
    /// </summary>
    public string Bdrq { get; set; }
    /// <summary>
    /// 接点
    /// </summary>
    public string Jied { get; set; }
    /// <summary>
    /// 出勤
    /// </summary>
    public string Cq { get; set; }
    /// <summary>
    /// 接车
    /// </summary>
    public string Jiec { get; set; }
    /// <summary>
    /// 交车
    /// </summary>
    public string Jiaoc { get; set; }
    /// <summary>
    /// 退勤
    /// </summary>
    public string Tq { get; set; }
    /// <summary>
    /// 出本段
    /// </summary>
    public string Cbd { get; set; }
    /// <summary>
    /// 入外段
    /// </summary>
    public string Rwd { get; set; }
    /// <summary>
    /// 出处段
    /// </summary>
    public string Cwd { get; set; }
    /// <summary>
    /// 入本段
    /// </summary>
    public string Rbd { get; set; }
    /// <summary>
    /// 接收量1
    /// </summary>
    public int? Jsl1 { get; set; }
    /// <summary>
    /// 加入量1
    /// </summary>
    public int? Jrl1 { get; set; }
    /// <summary>
    /// 交出量1
    /// </summary>
    public int? Jcl1 { get; set; }
    /// <summary>
    /// 再生电接收量1
    /// </summary>
    public int? Zsdjsl1 { get; set; }
    /// <summary>
    /// 再生电交出量1
    /// </summary>
    public int? Zsdjcl1 { get; set; }
    /// <summary>
    /// 列车供电接收量1
    /// </summary>
    public int? Lcgdjsl1 { get; set; }
    /// <summary>
    /// 列车供电交出量1
    /// </summary>
    public int? Lcgdjcl1 { get; set; }
    /// <summary>
    /// 列车供电接收量1_2
    /// </summary>
    public int? Lcgdjsl1_2 { get; set; }
    /// <summary>
    /// 列车供电交出量1_2
    /// </summary>
    public int? Lcgdjcl1_2 { get; set; }
    /// <summary>
    /// 接收量2
    /// </summary>
    public int? Jsl2 { get; set; }
    /// <summary>
    /// 加入量2
    /// </summary>
    public int? Jrl2 { get; set; }
    /// <summary>
    /// 交出量2
    /// </summary>
    public int? Jcl2 { get; set; }
    /// <summary>
    /// 再生电接收量2
    /// </summary>
    public int? Zsdjsl2 { get; set; }
    /// <summary>
    /// 再生电交出量2
    /// </summary>
    public int? Zsdjcl2 { get; set; }
    /// <summary>
    /// 列车供电接收量2
    /// </summary>
    public int? Lcgdjsl2 { get; set; }
    /// <summary>
    /// 列车供电交出量2
    /// </summary>
    public int? Lcgdjcl2 { get; set; }
    /// <summary>
    /// 列车供电接收量2_2
    /// </summary>
    public int? Lcgdjsl2_2 { get; set; }
    /// <summary>
    /// 列车供电交出量2_2
    /// </summary>
    public int? Lcgdjcl2_2 { get; set; }
    /// <summary>
    /// 打温时间
    /// </summary>
    public string Dwsj { get; set; }
    /// <summary>
    /// 打温用油量
    /// </summary>
    public int? Dwyl { get; set; }
    /// <summary>
    /// 油脂名称1
    /// </summary>
    public string Yzmc1 { get; set; }
    /// <summary>
    /// 油脂用量1
    /// </summary>
    public double? Yzyl1 { get; set; }
    /// <summary>
    /// 油脂名称2
    /// </summary>
    public string Yzmc2 { get; set; }
    /// <summary>
    /// 油脂用量2
    /// </summary>
    public double? Yzyl2 { get; set; }
    /// <summary>
    /// 油脂名称3
    /// </summary>
    public string Yzmc3 { get; set; }
    /// <summary>
    /// 油脂用量3
    /// </summary>
    public double? Yzyl3 { get; set; }
    /// <summary>
    /// 油脂名称4
    /// </summary>
    public string Yzmc4 { get; set; }
    /// <summary>
    /// 油脂用量4
    /// </summary>
    public double? Yzyl4 { get; set; }
    /// <summary>
    /// 油脂名称5
    /// </summary>
    public string Yzmc5 { get; set; }
    /// <summary>
    /// 油脂用量5
    /// </summary>
    public double? Yzyl5 { get; set; }
    /// <summary>
    /// 油脂名称6
    /// </summary>
    public string Yzmc6 { get; set; }
    /// <summary>
    /// 油脂用量6
    /// </summary>
    public double? Yzyl6 { get; set; }
    /// <summary>
    /// 油脂名称7
    /// </summary>
    public string Yzmc7 { get; set; }
    /// <summary>
    /// 油脂用量7
    /// </summary>
    public double? Yzyl7 { get; set; }
    /// <summary>
    /// 报单二维码
    /// </summary>
    public string Qrcode { get; set; }
    /// <summary>
    /// 纸质报单编号:No
    /// </summary>
    public string Zzbdbh { get; set; }
    /// <summary>
    /// 运统1报单数据id
    /// </summary>
    public string Yt1bdsjid { get; set; }
    /// <summary>
    /// 报单进程：1.发放，2.填写，3.提交，4.回收，5.发送成功，6.发送失败 7.转为归属地回收
    /// </summary>
    public int? Proceeding { get; set; }
    /// <summary>
    /// 乘务退勤地点代码
    /// </summary>
    public string Cwtqdddm { get; set; }
    /// <summary>
    /// 乘务出勤地点代码
    /// </summary>
    public string Cwcqdddm { get; set; }

    /// <summary>
    /// 乘务交路代码
    /// </summary>
    public string Cwjldm { get; set; }

    /// <summary>
    /// 乘务交路名称
    /// </summary>
    public string Cwjlmc { get; set; }

    /// <summary>
    /// 报单提交地点代码
    /// </summary>
    public string Bdtjdddm { get; set; }
    /// <summary>
    /// 报单回收情况:1.正常回收，2.纸质报单回收，6.手动回收
    /// </summary>
    public int? Bdhsqk { get; set; }
    /// <summary>
    /// 司机工资代码1
    /// </summary>
    public string Sjgzdm1 { get; set; }
    /// <summary>
    /// 副司机工资代码1
    /// </summary>
    public string Fsjgzdm1 { get; set; }
    /// <summary>
    /// 学员工资代码1
    /// </summary>
    public string Xygzdm1 { get; set; }
    /// <summary>
    /// 司机工资代码2
    /// </summary>
    public string Sjgzdm2 { get; set; }
    /// <summary>
    /// 副司机工资代码2
    /// </summary>
    public string Fsjgzdm2 { get; set; }
    /// <summary>
    /// 学员工资代码2
    /// </summary>
    public string Xygzdm2 { get; set; }
    /// <summary>
    /// 司机输入代码1
    /// </summary>
    public string Sjsrdm1 { get; set; }
    /// <summary>
    /// 副司机输入代码1
    /// </summary>
    public string Fsjsrdm1 { get; set; }
    /// <summary>
    /// 学员输入代码1
    /// </summary>
    public string Xysrdm1 { get; set; }
    /// <summary>
    ///  司机输入代码2
    /// </summary>
    public string Sjsrdm2 { get; set; }
    /// <summary>
    /// 副司机输入代码2
    /// </summary>
    public string Fsjsrdm2 { get; set; }
    /// <summary>
    /// 学员输入代码2
    /// </summary>
    public string Xysrdm2 { get; set; }
    /// <summary>
    /// 编组数据
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public IList<Bzsj> Bzsjs { get; set; }

    /// <summary>
    /// 双机固定重联机车数据
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public IList<Clbdsj> Clbdsjs { get; set; }
    /// <summary>
    /// 乘务员归属地点代码
    /// </summary>
    public string Cwgsdddm { get; set; }

    /// <summary>
    /// 报单发放人
    /// </summary>
    public string Bdffr { get; set; }

    /// <summary>
    /// 报单回收人
    /// </summary>
    public string Bdhsr { get; set; }
    /// <summary>
    /// 报单修改人
    /// </summary>
    public string Bdxgr { get; set; }

    /// <summary>
    /// 报单发放时间
    /// </summary>
    public string Bdffsj { get; set; }

    /// <summary>
    /// 报单提交时间
    /// </summary>
    public string Bdtjsj { get; set; }
    /// <summary>
    /// 报单回收时间
    /// </summary>
    public string Bdhssj { get; set; }
    /// <summary>
    /// 报单修改时间
    /// </summary>
    public string Bdxgsj { get; set; }
    /// <summary>
    /// 报单作废人
    /// </summary>
    public string Bdzfr { get; set; }
    /// <summary>
    /// 报单作废时间
    /// </summary>
    public string Bdzfsj { get; set; }
    /// <summary>
    /// 报单回退人
    /// </summary>
    public string Bdhtr { get; set; }
    /// <summary>
    /// 报单回退时间
    /// </summary>
    public string Bdhtsj { get; set; }
    /// <summary>
    /// 发往本局 null需要发送 -1不需要发送 0发送成功 1发送不成功,需要重发
    /// </summary>
    public int? Fwbj { get; set; }

    /// <summary>
    /// 发往总公司 null需要发送 -1不需要发送 0发送成功 1发送不成功,需要重发
    /// </summary>
    public int? Fwzgs { get; set; }

    /// <summary>
    /// 发往外段 null需要发送 -1不需要发送 0发送成功 1发送不成功,需要重发
    /// </summary>
    public int? Fwwd { get; set; }

    /// <summary>
    /// 发往外局  null需要发送 -1不需要发送 0发送成功 1发送不成功,需要重发
    /// </summary>
    public int? Fwwj { get; set; }
    
    [SugarColumn(IsIgnore = true)]
    public int? AeiProceeding { get; set; }
    /// <summary>
    /// 监控交路单位代码
    /// </summary>
    public string Jkjldwdm { get; set; }
    public string Sfsjjlh { get; set; }  //始发实际交路号
    public string Sfsrjlh { get; set; }  //始发输入交路号
    public string Sfczh { get; set; }    //始发车站号
    public string Sfczmc { get; set; }   //始发车站名称
    public string Sfcc { get; set; }     //始发车次
    public string Zdsjjlh { get; set; }  //终到实际交路号
    public string Zdsrjlh { get; set; }  //终到输入交路号
    public string Zdczh { get; set; }    //终到车站号
    public string Zdczmc { get; set; }   //终到车站名称
    public string Zdcc { get; set; }     //终到车次
    public string Tjzt { get; set; }     //统计状态
    public string Sjsj { get; set; }     //收集时间
    public string Tjsj { get; set; }     //统计时间
    public string Tjzh { get; set; }     //统计账号
    [SugarColumn(IsIgnore = true)]
    public string Hssfyz { get; set; }     //回收是否验证:1验证 0不验证
    [SugarColumn(IsIgnore =true)]
    public string Eventtime { get; set; }     //运统一数据入库时间(入运统一库)
    [SugarColumn(IsIgnore = true)]
    public int? NhProceeding { get; set; }

    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string Db { get; set; }
    /// <summary>
    /// 标准消耗,指的是机车标准消耗量
    /// </summary>
    public int? Bzxh { get; set; }
    /// <summary>
    /// 能耗卡控值,这个值为实际消耗/标准消耗 或 标准消耗/实际消耗的比例
    /// </summary>
    public double? Nhkk { get; set; }
    /// <summary>
    /// 燃料比例,这个值用于计算实际消耗量
    /// </summary>
    public double? Rlbl { get; set; }
    /// <summary>
    /// 上油方式,这个对内燃机车有用，当为true时，计算能耗时上油不用乘比例。
    /// </summary>
    public bool Syfs { get; set; }

    public string Nhsfyc { get; set; }        //能耗是否异常
    /*内燃实际能耗计算方式
      (Jiey - Jiaoy) * Rlbl + Jiay* (Syfs? Rlbl : 1) 
      电力实际能耗计算方式
      Jiaoy – Jiey*/

    /// <summary>
    /// 机车号是否异常
    /// </summary>
    public string Jchsfyc { get; set; }

    /// <summary>
    /// 机车统计系统是否已录入该报单
    /// </summary>
    public string Sfylr { get; set; }

    /// <summary>
    /// 车间代码
    /// </summary>
    public string Cjdm { get; set; }

    /// <summary>
    /// 车队代码
    /// </summary>
    public string Cddm { get; set; }

    /// <summary>
    /// 报单提交来源(1.后台合成 2.一体机提交 3.台式机提交)
    /// </summary>
    public string Bdtjly { get; set; }

    /// <summary>
    /// 手动转卡(1:是 0或null:否)
    /// </summary>
    public string Sdzk { get; set; }

    /// <summary>
    /// 缺项报单标识：0正常报单 1无运行信息 2无燃料 3无数据
    /// </summary>
    public string Qxbdbs { get; set; }

    /// <summary>
    /// 工作方式
    /// </summary>
    public string Gzfs { get; set; }

    [SugarColumn(IsIgnore =true)]
    public string IsWjch { get; set; } //数据库中是否无机车数据。有机车号为0,无机车号为1。这样设置是为了终端程序中的默认值false,则不处理（太原机务段系统)。

    /// <summary>
    /// 电表是否损坏(A端)：是，否
    /// </summary>
    public string Dbsfsh1 { get; set; }
    /// <summary>
    /// 电表是否损坏(B端)：是，否
    /// </summary>
    public string Dbsfsh2 { get; set; }
    /// <summary>
    /// aei出段
    /// </summary>
    public string Aei_RD { get; set; }
    /// <summary>
    /// aei入段
    /// </summary>
    public string Aei_CD { get; set; }
    /// <summary>
    /// 接收量1
    /// </summary>
    public int? Jsl1_BF { get; set; }
    /// <summary>
    /// 交出量1
    /// </summary>
    public int? Jcl1_BF { get; set; }
    /// <summary>
    /// 再生电接收量1
    /// </summary>
    public int? Zsdjsl1_BF { get; set; }
    /// <summary>
    /// 再生电交出量1
    /// </summary>
    public int? Zsdjcl1_BF { get; set; }
    /// <summary>
    /// 接收量2
    /// </summary>
    public int? Jsl2_BF { get; set; }
    /// <summary>
    /// 交出量2
    /// </summary>
    public int? Jcl2_BF { get; set; }
    /// <summary>
    /// 再生电接收量2
    /// </summary>
    public int? Zsdjsl2_BF { get; set; }
    /// <summary>
    /// 再生电交出量2
    /// </summary>
    public int? Zsdjcl2_BF { get; set; }
    /// <summary>
    /// 监控机车型号代码
    /// </summary>
    public string Jcxh { get; set; }
    /// <summary>
    /// 报单来源:1.自动生成 2.一体机提交 3.台式机提交
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public string Bdlymc
    {
        get
        {
            if (string.IsNullOrEmpty(Bdtjly) || Bdtjly == "2")
                return "一体机提交";
            else if (Bdtjly == "1")
                return "自动合成";
            else if (Bdtjly == "3")
                return "台式机提交";
            return "";
        }
    }

    /// <summary>
    /// 能耗使用量1
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public int? Syl1
    {
        get
        {
            if (Jczl == "D" || Jczl == "1")
            {
                return Jcl1 - Jsl1;
            }
            else
            {
                int? Jrl = null == Jrl1 ? 0 : Jrl1;
                return Jsl1 + Jrl - Jcl1;
            }
        }
    }
    /// <summary>
    /// 能耗使用量2
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int? Syl2
    {
        get
        {
            if (Jczl == "D" || Jczl == "1")
            {
                return Jcl2 - Jsl2;
            }
            else
            {
                int? Jrl = null == Jrl2 ? 0 : Jrl2;
                return Jsl2 + Jrl - Jcl2;
            }
        }
    }

    /// <summary>
    /// 再生电使用量1
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public int? Zsdsyl1
    {
        get
        {
            return Zsdjcl1 - Zsdjsl1;
        }
    }

    /// <summary>
    /// 再生电使用量2
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int? Zsdsyl2
    {
        get
        {
            return Zsdjcl2 - Zsdjsl2;
        }
    }

    /// <summary>
    /// 列车供电使用量1
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public int? Lcgdsyl1
    {
        get
        {
            return Lcgdjcl1 - Lcgdjsl1;
        }
    }

    /// <summary>
    /// 列车供电使用量2
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int? Lcgdsyl2
    {
        get
        {
            return Lcgdjcl2 - Lcgdjsl2;
        }
    }

    /// <summary>
    /// 列车供电使用量1_2
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public int? Lcgdsyl1_2
    {
        get
        {
            return Lcgdjcl1_2 - Lcgdjsl1_2;
        }
    }


    /// <summary>
    /// 列车供电使用量2
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public int? Lcgdsyl2_2
    {
        get
        {
            return Lcgdjcl2_2 - Lcgdjsl2_2;
        }
    }
}

public class BdsjView : Bdsj
{
    public BdsjView(Bdsj bdsj)
    {
        Type type = bdsj.GetType();
        foreach (PropertyInfo p in type.GetProperties())
        {
            if (p.CanWrite)
            {
                p.SetValue(this, p.GetValue(bdsj));
            }

        }
    }

    /// <summary>
    /// 再生电空车重车（用于再生电卡控）
    /// </summary>
    public string ZsdKcZc
    {
        get
        {
            if (this.Bzsjs != null && this.Bzsjs.Count > 0)
            {
                var zaiz = this.Bzsjs.Max(x => x.Zaiz.HasValue ? x.Zaiz.Value : 0);
                return zaiz > 0 ? "2" : "1";
            }

            return "0";
        }

    }
}

public class ReportBdsj
{
    public string SHZT { get; set; }

    public int XH { get; set; }

    public string SJDM1 { get; set; }

    public string SJXM1 { get; set; }

    public string JCH { get; set; }

    public string BDRQ { get; set; }

    public string QRCODE { get; set; }

    public string DISPLAYNAME { get; set; }

    public string FSJDM1 { get; set; }

    public string FSJXM1 { get; set; }

    public string CQ { get; set; }

    public string JIEC { get; set; }
    public string JIAOC { get; set; }

    public string TQ { get; set; }
    public string CK { get; set; }
    public string RK { get; set; }
    public string TJMC { get; set; }
    public string XYDM1 { get; set; }
    public string XYXM1 { get; set; }
    public string SFCZMC { get; set; }

    public string ZDCZMC { get; set; }

    public string BDHSSJ { get; set; }

    public string BDHSSJTXT
    {
        get
        {
            return Convert.ToDateTime(BDHSSJ).ToString("MM-dd HH:mm");
        }
    }

    public string CJMC { get; set; }
}
public class RequestForGetYt1BdsjDetail
{
    public string Ddm { get; set; }

    public BdsjYt1 Bdsj { get; set; }
}
public class RequestForGetYt1BdsjList
{
    public string Ddm { get; set; }
}