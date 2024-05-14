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
[SugarTable("yt1_dat_bdsj")]
public class BdsjYt1 : AggregateRoot<int>
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
    /// 列车供电交出量2
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
    /// 车间代码
    /// </summary>
    public string Cjdm { get; set; }


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
    [SugarColumn(IsIgnore =true)]
    public IList<BzsjYt1> Bzsjs { get; set; }
    /// <summary>
    /// 双机固定重联机车数据
    /// </summary>
    [SugarColumn(IsIgnore =true)]

    public IList<ClbdsjYt1> Clbdsjs { get; set; }
    /// <summary>
    /// 双机固定重联机车数据
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public IList<NhsjYt1> Nhsjs { get; set; }
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


    public string SHZT { get; set; }

    public string SHR { get; set; }

    public string SHSJ { get; set; }

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
    public string Psj { get; set; }     //配属局
    public string Psd { get; set; }     //配属段
    public string Eventtime { get; set; }     //运统一数据入库时间(入运统一库)
                                              //public int? Islkj { get; set; }     //是否已经结合LKJ生成电子报单   
                                              //public string Lkjsj { get; set; }     //结合LKJ生成电子报单时间

    /// <summary>
    /// 重联，补机，从控数据表
    /// </summary>
    [SugarColumn(IsIgnore =true)]
    public IList<Yt1Bc> Yt1Bcs { get; set; }

    /// <summary>
    /// 工作方式
    /// </summary>
    public string Gzfs { get; set; }
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
    [SugarColumn(IsIgnore =true)]
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
    [SugarColumn(IsIgnore =true)]
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
    [SugarColumn(IsIgnore =true)]
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

public class ReportYt1
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

    public string TQ { get; set; }
    public string SFCZMC { get; set; }

    public string ZDCZMC { get; set; }

    public string SHSJ { get; set; }

    public string SHSJTXT
    {
        get
        {
            return Convert.ToDateTime(SHSJ).ToString("yyyy-MM-dd");
        }
    }

    public string CJMC { get; set; }
}

/// <summary>
/// 编组数据
/// </summary>
[SugarTable("yt1_dat_bzsj")]
public class BzsjYt1 : AggregateRoot<int>
{
    /// <summary>
    /// 车次
    /// </summary>
    public string Cc { get; set; }
    /// <summary>
    /// 客货类型
    /// </summary>
    public string Khlx { get; set; }
    /// <summary>
    /// 客货本补
    /// </summary>
    public string Khbb { get; set; }
    /// <summary>
    /// 总重
    /// </summary>
    public int? Zz { get; set; }
    /// <summary>
    /// 运统1总重
    /// </summary>
    public string Yt1Zz { get; set; }
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
    public int? Hc { get; set; }
    /// <summary>
    /// 运统1换长
    /// </summary>
    public string Yt1Hc { get; set; }
    /// <summary>
    /// 到达
    /// </summary>
    public string Dd { get; set; }
    /// <summary>
    /// 出发
    /// </summary>
    public string Cf { get; set; }
    /// <summary>
    /// 机外停车时间
    /// </summary>
    public string Jwtcsj { get; set; }
    /// <summary>
    /// 区间停车时间
    /// </summary>
    public string Qjtcsj { get; set; }
    /// <summary>
    /// 实际交路号
    /// </summary>
    public string Sjjlh { get; set; }
    /// <summary>
    /// 输入交路号
    /// </summary>
    public string Srjlh { get; set; }
    /// <summary>
    /// 车站号
    /// </summary>
    public string Czh { get; set; }
    /// <summary>
    /// 车站名称
    /// </summary>
    public string Czmc { get; set; }
    /// <summary>
    /// 区间公里
    /// </summary>
    public int? Qjgl { get; set; }
    /// <summary>
    /// 调车时间
    /// </summary>
    public string Dcsj { get; set; }
    /// <summary>
    /// 调车方式
    /// </summary>
    public string Dcfs { get; set; }
    /// <summary>
    /// 入段
    /// </summary>
    public string Rd { get; set; }
    /// <summary>
    /// 出段
    /// </summary>
    public string Cd { get; set; }
    /// <summary>
    /// 监控单位
    /// </summary>
    public string Jkdw { get; set; }
    /// <summary>
    /// 重联机车
    /// </summary>
    public string Cljc { get; set; }
    /// <summary>
    /// 编组次数
    /// </summary>
    public int? Bzcs { get; set; }
    /// <summary>
    /// 运统1报单数据id
    /// </summary>
    public string Yt1bdsjid { get; set; }
    /// <summary>
    /// 运统1id
    /// </summary>
    public string Yt1id { get; set; }
    /// <summary>
    /// 确报数据id
    /// </summary>
    public string Qbsjid { get; set; }
    /// <summary>
    /// 清算运统1数据id
    /// </summary>
    public string Qsyt1id { get; set; }
    /// <summary>
    /// 客编数据id
    /// </summary>
    public string Kbsjid { get; set; }
    /// <summary>
    /// 报单数据id
    /// </summary>
    public int? Bdsjid { get; set; }
    /// <summary>
    /// 统计车站代码
    /// </summary>
    public string Tjczdm { get; set; }
    /// <summary>
    /// 统计车站代码
    /// </summary>
    public string Tjczmc { get; set; }
    /// <summary>
    /// 重联机车摘补
    /// </summary>
    public string Cljczb { get; set; }
    /// <summary>
    /// 子编组数据
    /// </summary>
    public IList<ZbzsjYt1> Zbzsjs { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机号
    /// </summary>
    public string Jh { get; set; }
}
/// <summary>
/// 子编组数据(集、特、行)
/// </summary>
public class ZbzsjYt1 : AggregateRoot<int>
{
    /// <summary>
    /// 编组类型 返回的是"集"、"特"、"行"，我们库里保存的是1，2，3，这里需要处理。"合计"、"企"、"国铁"等不需要保存
    /// </summary>
    public string Yt1Bzlxmc { get; set; }
    /// <summary>
    /// 编组类型
    /// </summary>
    public int? Bzlx { get; set; }
    /// <summary>
    /// 总重
    /// </summary>
    public int? Zz { get; set; }
    /// <summary>
    /// 运统1总重
    /// </summary>
    public string Yt1Zz { get; set; }
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
    public int? Hc { get; set; }
    /// <summary>
    /// 运统1换长
    /// </summary>
    public string Yt1Hc { get; set; }
    /// <summary>
    /// 编组数据id
    /// </summary>
    public int? Bzsjid { get; set; }
    /// <summary>
    /// 编组类型名称
    /// </summary>

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
/// <summary>
/// 双机固定重联机车数据
/// </summary>
public class ClbdsjYt1 : AggregateRoot<int>
{
    /// <summary>
    /// 报单数据id
    /// </summary>
    public int? Bdsjid { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机车号
    /// </summary>
    public string Jch { get; set; }
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

    public string Jcxh
    {
        get
        {
            return Jx + "-" + Jch;
        }
    }
}
public class NhsjYt1 : AggregateRoot<int>
{
    /// <summary>
    /// 报单数据id
    /// </summary>
    public int? Bdsjid { get; set; }
    /// <summary>
    /// 报单数据ID: DAT_BDSJ.ID（关联外键）
    /// </summary>
    public string Zid { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机车号
    /// </summary>
    public string Jch { get; set; }
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
    /// 打温时间
    /// </summary>
    public string Dwsj { get; set; }
    /// <summary>
    /// 打温用油量
    /// </summary>
    public int? Dwyl { get; set; }

    public string Jcxh
    {
        get
        {
            return Jx + "-" + Jch;
        }
    }
}
public class Yt1Bc : AggregateRoot<int>
{
    /// <summary>
    /// 报单数据id
    /// </summary>
    public int? Bdsjid { get; set; }
    /// <summary>
    /// Pk唯一标识
    /// </summary>
    public string Pk { get; set; }
    /// <summary>
    /// 本务作业司机报单号
    /// </summary>
    public string Zid { get; set; }
    /// <summary>
    /// 工作方式
    /// </summary>
    public string Gzfs { get; set; }
    /// <summary>
    /// 机车型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机车号
    /// </summary>
    public string Jh { get; set; }
    /// <summary>
    /// 重联请求结束站
    /// </summary>
    [JsonProperty(PropertyName = "depStn")]
    public string Dep_stn { get; set; }
    /// <summary>
    /// 重联请求开始站
    /// </summary>
    [JsonProperty(PropertyName = "arrStn")]
    public string Arr_stn { get; set; }
    /// <summary>
    /// 到站电报略码
    /// </summary>
    [JsonProperty(PropertyName = "depStncode")]
    public string Dep_stncode { get; set; }
    /// <summary>
    /// 发站电报略码
    /// </summary>
    [JsonProperty(PropertyName = "arrStncode")]
    public string Arr_stncode { get; set; }
    /// <summary>
    /// 重联关联的运统1 PK值（2021年7月3号新添加该字段，之前没有记录数据，后续为空字段代表本务机车未签认运统1）
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 重联机车关联到本务机车时间
    /// </summary>
    public string Eventtime { get; set; }
    /// <summary>
    /// 本务添加重联机车时间
    /// </summary>
    public string Ctime { get; set; }
    /// <summary>
    /// 重联作业司机报单号（为空标识未有重连司机关联）
    /// </summary>
    public string Glsjbdid { get; set; }
    /// <summary>
    /// 重联司机工号
    /// </summary>
    public string Clsjm { get; set; }
    /// <summary>
    /// 重联司机姓名
    /// </summary>
    public string Clsj { get; set; }
    /// <summary>
    /// 本务机车信息（机型-机车号)
    /// </summary>
    public string Bwjc { get; set; }
}
public class RequestForGetYt1Bdsj
{
    public string Ddm { get; set; }

    public bool IsEnd { get; set; }

    public Yt1Bdsj Bdsj { get; set; }
}