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
public class Yt1Bdsj
{
    public string Id { get; set; }
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
    public long? Jsl1 { get; set; }
    /// <summary>
    /// 加入量1
    /// </summary>
    public long? Jrl1 { get; set; }
    /// <summary>
    /// 交出量1
    /// </summary>
    public long? Jcl1 { get; set; }
    /// <summary>
    /// 再生电接收量1
    /// </summary>
    public long? Zsdjsl1 { get; set; }
    /// <summary>
    /// 再生电交出量1
    /// </summary>
    public long? Zsdjcl1 { get; set; }
    /// <summary>
    /// 列车供电接收量1
    /// </summary>
    public long? Lcgdjsl1 { get; set; }
    /// <summary>
    /// 列车供电交出量1
    /// </summary>
    public long? Lcgdjcl1 { get; set; }
    /// <summary>
    /// 接收量2
    /// </summary>
    public long? Jsl2 { get; set; }
    /// <summary>
    /// 加入量2
    /// </summary>
    public long? Jrl2 { get; set; }
    /// <summary>
    /// 交出量2
    /// </summary>
    public long? Jcl2 { get; set; }
    /// <summary>
    /// 再生电接收量2
    /// </summary>
    public long? Zsdjsl2 { get; set; }
    /// <summary>
    /// 再生电交出量2
    /// </summary>
    public long? Zsdjcl2 { get; set; }
    /// <summary>
    /// 列车供电接收量2
    /// </summary>
    public long? Lcgdjsl2 { get; set; }
    /// <summary>
    /// 列车供电交出量2
    /// </summary>
    public long? Lcgdjcl2 { get; set; }
    /// <summary>
    /// 打温时间
    /// </summary>
    public string Dwsj { get; set; }
    /// <summary>
    /// 打温用油量
    /// </summary>
    public long? Dwyl { get; set; }
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
    [JsonProperty(PropertyName = "dat_bzsj")]
    public IList<Yt1Bzsj> Bzsjs { get; set; }
    /// <summary>
    /// 双机固定重联机车数据
    /// </summary>
    [JsonProperty(PropertyName = "dat_clbdsj")]
    public IList<Yt1Nhsj> Nhsjs { get; set; }
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
    public string SHZT { get; set; }     //审核状态
    [JsonProperty(PropertyName = "psddm")]
    public string Psd { get; set; }     //配属段
    public string Eventtime { get; set; }     //运统一数据入库时间(入运统一库)

    /// <summary>
    /// 重联，补机，从控数据表
    /// </summary>
    [JsonProperty(PropertyName = "yt1_driver_listbc")]
    public IList<Yt1Bc> Yt1Bcs { get; set; }
}
public class Yt1Bzsj
{
    public string Id { get; set; }
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
    [JsonProperty(PropertyName = "Zz")]
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
    [JsonProperty(PropertyName = "kongc")]
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
    [JsonProperty(PropertyName = "Hc")]
    public string Yt1Hc { get; set; }
    /// <summary>
    /// 计划到达
    /// </summary>
    public string Jhdd { get; set; }
    /// <summary>
    /// 计划出发
    /// </summary>
    public string Jhcf { get; set; }
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
    /// 附挂机车
    /// </summary>
    public string Fgjc { get; set; }
    /// <summary>
    /// 编组次数
    /// </summary>
    public int? Bzcs { get; set; }
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
    public string Bdsjid { get; set; }
    /// <summary>
    /// 统计车站代码
    /// </summary>
    public string Tjczdm { get; set; }
    /// <summary>
    /// 统计车站代码
    /// </summary>
    public string Tjczmc { get; set; }
    /// <summary>
    /// 运统1id
    /// </summary>
    [JsonProperty(PropertyName = "ytyid")]
    public string Yt1id { get; set; }
    /// <summary>
    /// 子编组数据
    /// </summary>
    [JsonProperty(PropertyName = "dat_zbzsj")]
    public IList<Yt1Zbzsj> Zbzsjs { get; set; }
    /// <summary>
    /// 机型
    /// </summary>
    public string Jx { get; set; }
    /// <summary>
    /// 机号
    /// </summary>
    public string Jh { get; set; }
    /// <summary>
    /// 重联，补机，从控数据表
    /// </summary>
    //[JsonProperty(PropertyName = "yt1_driver_listbc")]
    //public IList<Yt1Bc> Yt1Bc { get; set; }

}
public class Yt1Nhsj
{
    public string Id { get; set; }
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
    public long? Jsl1 { get; set; }
    /// <summary>
    /// 加入量1
    /// </summary>
    public long? Jrl1 { get; set; }
    /// <summary>
    /// 交出量1
    /// </summary>
    public long? Jcl1 { get; set; }
    /// <summary>
    /// 再生电接收量1
    /// </summary>
    public long? Zsdjsl1 { get; set; }
    /// <summary>
    /// 再生电交出量1
    /// </summary>
    public long? Zsdjcl1 { get; set; }
    /// <summary>
    /// 列车供电接收量1
    /// </summary>
    public long? Lcgdjsl1 { get; set; }
    /// <summary>
    /// 列车供电交出量1
    /// </summary>
    public long? Lcgdjcl1 { get; set; }
    /// <summary>
    /// 接收量2
    /// </summary>
    public long? Jsl2 { get; set; }
    /// <summary>
    /// 加入量2
    /// </summary>
    public long? Jrl2 { get; set; }
    /// <summary>
    /// 交出量2
    /// </summary>
    public long? Jcl2 { get; set; }
    /// <summary>
    /// 再生电接收量2
    /// </summary>
    public long? Zsdjsl2 { get; set; }
    /// <summary>
    /// 再生电交出量2
    /// </summary>
    public long? Zsdjcl2 { get; set; }
    /// <summary>
    /// 列车供电接收量2
    /// </summary>
    public long? Lcgdjsl2 { get; set; }
    /// <summary>
    /// 列车供电交出量2
    /// </summary>
    public long? Lcgdjcl2 { get; set; }
    /// <summary>
    /// 打温时间
    /// </summary>
    public string Dwsj { get; set; }
    /// <summary>
    /// 打温用油量
    /// </summary>
    public long? Dwyl { get; set; }

    public string Jcxh
    {
        get
        {
            return Jx + "-" + Jch;
        }
    }
}
public class RequestForGetTqxx
{
    public TqxxModel tqxx { get; set; }
}
public class RequestForGetCqxx
{
    public CqxxModel cqxx { get; set; }
}
public class RequestForGetCtqxx
{
    public string CtqName { get; set; }
}
[SugarTable("dat_tqxxjjm")]
public class TqxxModel : AggregateRoot<int>
{
    /// <summary>
    /// 线路名称
    /// </summary>
    public string Linename { get; set; }
    /// <summary>
    /// 计划车次
    /// </summary>
    public string Traincode { get; set; }
    /// <summary>
    /// 计划开点
    /// </summary>
    public string Deptime { get; set; }
    /// <summary>
    /// 开站
    /// </summary>
    public string Depstation { get; set; }
    /// <summary>
    /// 到点
    /// </summary>
    public string Arrtime { get; set; }
    /// <summary>
    /// 到站
    /// </summary>
    public string Arrstation { get; set; }
    /// <summary>
    /// 计划机车
    /// </summary>
    public string Locomotive { get; set; }
    /// <summary>
    /// 计划出勤时间
    /// </summary>
    public string Planondutytime { get; set; }
    /// <summary>
    /// 人员ID
    /// </summary>
    public string Personid { get; set; }
    /// <summary>
    /// 人员工号
    /// </summary>
    public string Workno { get; set; }
    /// <summary>
    /// 人员姓名
    /// </summary>
    public string Hname { get; set; }
    /// <summary>
    /// 人员排位
    /// </summary>
    public string Workrole { get; set; }
    /// <summary>
    /// 退勤时间
    /// </summary>
    public string Offdutytime { get; set; }
    /// <summary>
    /// 退勤方式0=手工,1=指纹,2=虹魔
    /// </summary>
    public string Offdutymode { get; set; }
    /// <summary>
    /// 退勤地点
    /// </summary>
    public string Place { get; set; }
    /// <summary>
    /// 测酒结果0=未测,1=正常,2=无效,3=饮酒,9=酗酒
    /// </summary>
    public string Drinksign { get; set; }
    /// <summary>
    /// 测酒数值
    /// </summary>
    public string Drinkvalue { get; set; }
}
[SugarTable("dat_cqxxjjm")]
public class CqxxModel : AggregateRoot<int>
{
    /// <summary>
    /// 线路名称
    /// </summary>
    public string Linename { get; set; }
    /// <summary>
    /// 计划车次
    /// </summary>
    public string Traincode { get; set; }
    /// <summary>
    /// 计划开点
    /// </summary>
    public string Deptime { get; set; }
    /// <summary>
    /// 开站
    /// </summary>
    public string Depstation { get; set; }
    /// <summary>
    /// 到点
    /// </summary>
    public string Arrtime { get; set; }
    /// <summary>
    /// 到站
    /// </summary>
    public string Arrstation { get; set; }
    /// <summary>
    /// 计划机车
    /// </summary>
    public string Locomotive { get; set; }
    /// <summary>
    /// 计划出勤时间
    /// </summary>
    public string Planondutytime { get; set; }
    /// <summary>
    /// 人员ID
    /// </summary>
    public string Personid { get; set; }
    /// <summary>
    /// 人员工号
    /// </summary>
    public string Workno { get; set; }
    /// <summary>
    /// 人员姓名
    /// </summary>
    public string Hname { get; set; }
    /// <summary>
    /// 人员排位
    /// </summary>
    public string Workrole { get; set; }
    /// <summary>
    /// 出勤时间
    /// </summary>
    public string Ondutytime { get; set; }
    /// <summary>
    /// 出勤方式0=手工,1=指纹,2=虹魔
    /// </summary>
    public string Ondutymode { get; set; }
    /// <summary>
    /// 出勤地点
    /// </summary>
    public string Place { get; set; }
    /// <summary>
    /// 测酒结果0=未测,1=正常,2=无效,3=饮酒,9=酗酒
    /// </summary>
    public string Drinksign { get; set; }
    /// <summary>
    /// 测酒数值
    /// </summary>
    public string Drinkvalue { get; set; }
}