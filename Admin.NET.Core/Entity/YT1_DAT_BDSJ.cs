using System;
using System.Linq;
using System.Text;
using Admin.NET.Core.Entity.ElectReport;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///运统一报单数据（之前的实体BdsjYt1中的属性类型和字段的类型不同导致无法使用，所以增加该实体）
    ///</summary>
    [SugarTable("YT1_DAT_BDSJ")]
    public partial class YT1_DAT_BDSJ
    {
           public YT1_DAT_BDSJ(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:司机代码1
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string SJDM1 {get;set;}

           /// <summary>
           /// Desc:司机姓名1
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string SJXM1 {get;set;}

           /// <summary>
           /// Desc:副司机代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJDM1 {get;set;}

           /// <summary>
           /// Desc:副司机姓名1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJXM1 {get;set;}

           /// <summary>
           /// Desc:学员代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYDM1 {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYXM1 {get;set;}

           /// <summary>
           /// Desc:司机代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJDM2 {get;set;}

           /// <summary>
           /// Desc:司机姓名2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJXM2 {get;set;}

           /// <summary>
           /// Desc:副司机代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJDM2 {get;set;}

           /// <summary>
           /// Desc:副司机姓名2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJXM2 {get;set;}

           /// <summary>
           /// Desc:学员代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYDM2 {get;set;}

           /// <summary>
           /// Desc:学员姓名2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYXM2 {get;set;}

           /// <summary>
           /// Desc:值乘方式代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZCFSDM {get;set;}

           /// <summary>
           /// Desc:值乘方式名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZCFSMC {get;set;}

           /// <summary>
           /// Desc:机车种类
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JCZL {get;set;}

           /// <summary>
           /// Desc:机型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JX {get;set;}

           /// <summary>
           /// Desc:机车号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JCH {get;set;}

           /// <summary>
           /// Desc:报单日期
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDRQ {get;set;}

           /// <summary>
           /// Desc:接点
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JIED {get;set;}

           /// <summary>
           /// Desc:出勤
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CQ {get;set;}

           /// <summary>
           /// Desc:接车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JIEC {get;set;}

           /// <summary>
           /// Desc:交车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JIAOC {get;set;}

           /// <summary>
           /// Desc:退勤
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TQ {get;set;}

           /// <summary>
           /// Desc:出本段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CBD {get;set;}

           /// <summary>
           /// Desc:入外段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? RWD {get;set;}

           /// <summary>
           /// Desc:出外段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWD {get;set;}

           /// <summary>
           /// Desc:入本段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? RBD {get;set;}

           /// <summary>
           /// Desc:接收量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? JSL1 {get;set;}

           /// <summary>
           /// Desc:加入量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? JRL1 {get;set;}

           /// <summary>
           /// Desc:交出量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? JCL1 {get;set;}

           /// <summary>
           /// Desc:再生电接收量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZSDJSL1 {get;set;}

           /// <summary>
           /// Desc:再生电交出量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZSDJCL1 {get;set;}

           /// <summary>
           /// Desc:列车供电接收量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LCGDJSL1 {get;set;}

           /// <summary>
           /// Desc:列车供电交出量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LCGDJCL1 {get;set;}

           /// <summary>
           /// Desc:接收量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? JSL2 {get;set;}

           /// <summary>
           /// Desc:加入量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? JRL2 {get;set;}

           /// <summary>
           /// Desc:交出量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? JCL2 {get;set;}

           /// <summary>
           /// Desc:再生电接收量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZSDJSL2 {get;set;}

           /// <summary>
           /// Desc:再生电交出量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZSDJCL2 {get;set;}

           /// <summary>
           /// Desc:列车供电接收量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LCGDJSL2 {get;set;}

           /// <summary>
           /// Desc:列车供电交出量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LCGDJCL2 {get;set;}

           /// <summary>
           /// Desc:打温时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DWSJ {get;set;}

           /// <summary>
           /// Desc:打温用量
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? DWYL {get;set;}

           /// <summary>
           /// Desc:油脂名称1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC1 {get;set;}

           /// <summary>
           /// Desc:油脂用量1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL1 {get;set;}

           /// <summary>
           /// Desc:油脂名称2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC2 {get;set;}

           /// <summary>
           /// Desc:油脂用量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL2 {get;set;}

           /// <summary>
           /// Desc:油脂名称3
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC3 {get;set;}

           /// <summary>
           /// Desc:油脂用量3
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL3 {get;set;}

           /// <summary>
           /// Desc:油脂名称4
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC4 {get;set;}

           /// <summary>
           /// Desc:油脂用量4
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL4 {get;set;}

           /// <summary>
           /// Desc:油脂名称5
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC5 {get;set;}

           /// <summary>
           /// Desc:油脂用量5
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL5 {get;set;}

           /// <summary>
           /// Desc:油脂名称6
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC6 {get;set;}

           /// <summary>
           /// Desc:油脂用量6
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL6 {get;set;}

           /// <summary>
           /// Desc:油脂名称7
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YZMC7 {get;set;}

           /// <summary>
           /// Desc:油脂用量7
           /// Default:
           /// Nullable:True
           /// </summary>           
           public double? YZYL7 {get;set;}

           /// <summary>
           /// Desc:报单二维码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QRCODE {get;set;}

           /// <summary>
           /// Desc:报单所处进程：1.发放，2.填写，3.提交，4.回收，7.转为归属地回收
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal PROCEEDING {get;set;}

           /// <summary>
           /// Desc:乘务退勤地点代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWTQDDDM {get;set;}

           /// <summary>
           /// Desc:乘务出勤地点代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWCQDDDM {get;set;}

           /// <summary>
           /// Desc:乘务交路代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWJLDM {get;set;}

           /// <summary>
           /// Desc:乘务交路名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWJLMC {get;set;}

           /// <summary>
           /// Desc:报单提交地点代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDTJDDDM {get;set;}

           /// <summary>
           /// Desc:报单回收情况：1.正常回收，2.纸质报单回收，6.手动回收
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? BDHSQK {get;set;}

           /// <summary>
           /// Desc:司机工资代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJGZDM1 {get;set;}

           /// <summary>
           /// Desc:副司机工资代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJGZDM1 {get;set;}

           /// <summary>
           /// Desc:学员工资代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYGZDM1 {get;set;}

           /// <summary>
           /// Desc:司机工资代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJGZDM2 {get;set;}

           /// <summary>
           /// Desc:副司机工资代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJGZDM2 {get;set;}

           /// <summary>
           /// Desc:学员工资代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYGZDM2 {get;set;}

           /// <summary>
           /// Desc:司机输入代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJSRDM1 {get;set;}

           /// <summary>
           /// Desc:副司机输入代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJSRDM1 {get;set;}

           /// <summary>
           /// Desc:学员输入代码1
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYSRDM1 {get;set;}

           /// <summary>
           /// Desc:司机输入代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJSRDM2 {get;set;}

           /// <summary>
           /// Desc:副司机输入代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FSJSRDM2 {get;set;}

           /// <summary>
           /// Desc:学员输入代码2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XYSRDM2 {get;set;}

           /// <summary>
           /// Desc:乘务归属地点代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWGSDDDM {get;set;}

           /// <summary>
           /// Desc:报单发放人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDFFR {get;set;}

           /// <summary>
           /// Desc:报单回收人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDHSR {get;set;}

           /// <summary>
           /// Desc:发往本局
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? FWBJ {get;set;}

           /// <summary>
           /// Desc:发往总公司
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? FWZGS {get;set;}

           /// <summary>
           /// Desc:发往外段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? FWWD {get;set;}

           /// <summary>
           /// Desc:发往外局
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? FWWJ {get;set;}

           /// <summary>
           /// Desc:支配局代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZPJDM {get;set;}

           /// <summary>
           /// Desc:支配局名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZPJMC {get;set;}

           /// <summary>
           /// Desc:支配段代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZPDDM {get;set;}

           /// <summary>
           /// Desc:支配段名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZPDMC {get;set;}

           /// <summary>
           /// Desc:录入局代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LRJDM {get;set;}

           /// <summary>
           /// Desc:录入局名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LRJMC {get;set;}

           /// <summary>
           /// Desc:录入段代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LRDDM {get;set;}

           /// <summary>
           /// Desc:录入段名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LRDMC {get;set;}

           /// <summary>
           /// Desc:乘务员1职别字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWYZB1 {get;set;}

           /// <summary>
           /// Desc:乘务员2职别字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWYZB2 {get;set;}

           /// <summary>
           /// Desc:乘务员3职别字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWYZB3 {get;set;}

           /// <summary>
           /// Desc:乘务员4职别字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWYZB4 {get;set;}

           /// <summary>
           /// Desc:乘务员5职别字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWYZB5 {get;set;}

           /// <summary>
           /// Desc:乘务员6职别字段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CWYZB6 {get;set;}

           /// <summary>
           /// Desc:报单修改人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDXGR {get;set;}

           /// <summary>
           /// Desc:报单发放时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDFFSJ {get;set;}

           /// <summary>
           /// Desc:报单提交时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDTJSJ {get;set;}

           /// <summary>
           /// Desc:报单回收时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDHSSJ {get;set;}

           /// <summary>
           /// Desc:报单修改时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDXGSJ {get;set;}

           /// <summary>
           /// Desc:监控交路单位代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JKJLDWDM {get;set;}

           /// <summary>
           /// Desc:始发数据交路号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SFSJJLH {get;set;}

           /// <summary>
           /// Desc:始发输入交路号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SFSRJLH {get;set;}

           /// <summary>
           /// Desc:始发车站号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SFCZH {get;set;}

           /// <summary>
           /// Desc:始发车站名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SFCZMC {get;set;}

           /// <summary>
           /// Desc:始发车次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SFCC {get;set;}

           /// <summary>
           /// Desc:终到数据交路号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZDSJJLH {get;set;}

           /// <summary>
           /// Desc:终到输入交路号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZDSRJLH {get;set;}

           /// <summary>
           /// Desc:终到车站号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZDCZH {get;set;}

           /// <summary>
           /// Desc:终到车站名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZDCZMC {get;set;}

           /// <summary>
           /// Desc:终到车次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZDCC {get;set;}

           /// <summary>
           /// Desc:统计状态:1.收集 2.统计 3. 暂缓统计 4.手工处理 null:未处理
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TJZT {get;set;}

           /// <summary>
           /// Desc:收集时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJSJ {get;set;}

           /// <summary>
           /// Desc:统计时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TJSJ {get;set;}

           /// <summary>
           /// Desc:统计账号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TJZH {get;set;}

           /// <summary>
           /// Desc:报单作废人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDZFR {get;set;}

           /// <summary>
           /// Desc:报单作废时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDZFSJ {get;set;}

           /// <summary>
           /// Desc:报单回退时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDHTSJ {get;set;}

           /// <summary>
           /// Desc:报单回退人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? BDHTR {get;set;}

           /// <summary>
           /// Desc:配属段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? PSD {get;set;}

           /// <summary>
           /// Desc:配属局
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? PSJ {get;set;}

           /// <summary>
           /// Desc:列车供电接收量1_2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LCGDJSL1_2 {get;set;}

           /// <summary>
           /// Desc:列车供电交出量1_2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LCGDJCL1_2 {get;set;}

           /// <summary>
           /// Desc:列车供电接收量2_2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LCGDJSL2_2 {get;set;}

           /// <summary>
           /// Desc:列车供电交出量2_2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? LCGDJCL2_2 {get;set;}

           /// <summary>
           /// Desc:运统1报单数据id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YT1BDSJID {get;set;}

           /// <summary>
           /// Desc:运统一数据入库时间(入运统一库)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? EVENTTIME {get;set;}

           /// <summary>
           /// Desc:审核状态:1通过0不通过
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SHZT {get;set;}

           /// <summary>
           /// Desc:审核人
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SHR {get;set;}

           /// <summary>
           /// Desc:审核时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SHSJ {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CREATEDATE {get;set;}

           /// <summary>
           /// Desc:工作方式
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? GZFS {get;set;}

           /// <summary>
           /// Desc:车间代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CJDM {get;set;}

    }
}
