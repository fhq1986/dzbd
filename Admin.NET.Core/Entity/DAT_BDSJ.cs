using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///报单数据(由于bdsj实体中有很多自定义属性，目前查询有很多找不到，同时有些数据类型和数据库中的不同，但是很多地方又有使用，所以暂时先增加一个DAT_BDSJ数据表)
    ///</summary>
    [SugarTable("DAT_BDSJ")]
    public partial class DAT_BDSJ
    {
        public DAT_BDSJ()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public int ID { get; set; }

        /// <summary>
        /// Desc:司机代码1
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string SJDM1 { get; set; }

        /// <summary>
        /// Desc:司机姓名1
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string SJXM1 { get; set; }

        /// <summary>
        /// Desc:副司机代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJDM1 { get; set; }

        /// <summary>
        /// Desc:副司机姓名1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJXM1 { get; set; }

        /// <summary>
        /// Desc:学员代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYDM1 { get; set; }

        /// <summary>
        /// Desc:学员姓名1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYXM1 { get; set; }

        /// <summary>
        /// Desc:司机代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJDM2 { get; set; }

        /// <summary>
        /// Desc:司机姓名2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJXM2 { get; set; }

        /// <summary>
        /// Desc:副司机代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJDM2 { get; set; }

        /// <summary>
        /// Desc:副司机姓名2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJXM2 { get; set; }

        /// <summary>
        /// Desc:学员代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYDM2 { get; set; }

        /// <summary>
        /// Desc:学员姓名2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYXM2 { get; set; }

        /// <summary>
        /// Desc:值乘方式代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZCFSDM { get; set; }

        /// <summary>
        /// Desc:值乘方式名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZCFSMC { get; set; }

        /// <summary>
        /// Desc:机车种类
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JCZL { get; set; }

        /// <summary>
        /// Desc:机型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JX { get; set; }

        /// <summary>
        /// Desc:机车号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JCH { get; set; }

        /// <summary>
        /// Desc:报单日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDRQ { get; set; }

        /// <summary>
        /// Desc:接点
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JIED { get; set; }

        /// <summary>
        /// Desc:出勤
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CQ { get; set; }

        /// <summary>
        /// Desc:接车
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JIEC { get; set; }

        /// <summary>
        /// Desc:交车
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JIAOC { get; set; }

        /// <summary>
        /// Desc:退勤
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? TQ { get; set; }

        /// <summary>
        /// Desc:出本段
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CBD { get; set; }

        /// <summary>
        /// Desc:入外段
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? RWD { get; set; }

        /// <summary>
        /// Desc:出外段
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWD { get; set; }

        /// <summary>
        /// Desc:入本段
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? RBD { get; set; }

        /// <summary>
        /// Desc:接收量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JSL1 { get; set; }

        /// <summary>
        /// Desc:加入量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JRL1 { get; set; }

        /// <summary>
        /// Desc:交出量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JCL1 { get; set; }

        /// <summary>
        /// Desc:再生电接收量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ZSDJSL1 { get; set; }

        /// <summary>
        /// Desc:再生电交出量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ZSDJCL1 { get; set; }

        /// <summary>
        /// Desc:列车供电接收量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? LCGDJSL1 { get; set; }

        /// <summary>
        /// Desc:列车供电交出量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? LCGDJCL1 { get; set; }

        /// <summary>
        /// Desc:接收量2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JSL2 { get; set; }

        /// <summary>
        /// Desc:加入量2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JRL2 { get; set; }

        /// <summary>
        /// Desc:交出量2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JCL2 { get; set; }

        /// <summary>
        /// Desc:再生电接收量2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ZSDJSL2 { get; set; }

        /// <summary>
        /// Desc:再生电交出量2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ZSDJCL2 { get; set; }

        /// <summary>
        /// Desc:列车供电接收量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? LCGDJSL2 { get; set; }

        /// <summary>
        /// Desc:列车供电交出量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? LCGDJCL2 { get; set; }

        /// <summary>
        /// Desc:打温时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? DWSJ { get; set; }

        /// <summary>
        /// Desc:打温用量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? DWYL { get; set; }

        /// <summary>
        /// Desc:油脂名称1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC1 { get; set; }

        /// <summary>
        /// Desc:油脂用量1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL1 { get; set; }

        /// <summary>
        /// Desc:油脂名称2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC2 { get; set; }

        /// <summary>
        /// Desc:油脂用量2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL2 { get; set; }

        /// <summary>
        /// Desc:油脂名称3
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC3 { get; set; }

        /// <summary>
        /// Desc:油脂用量3
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL3 { get; set; }

        /// <summary>
        /// Desc:油脂名称4
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC4 { get; set; }

        /// <summary>
        /// Desc:油脂用量4
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL4 { get; set; }

        /// <summary>
        /// Desc:油脂名称5
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC5 { get; set; }

        /// <summary>
        /// Desc:油脂用量5
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL5 { get; set; }

        /// <summary>
        /// Desc:油脂名称6
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC6 { get; set; }

        /// <summary>
        /// Desc:油脂用量6
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL6 { get; set; }

        /// <summary>
        /// Desc:油脂名称7
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YZMC7 { get; set; }

        /// <summary>
        /// Desc:油脂用量7
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? YZYL7 { get; set; }

        /// <summary>
        /// Desc:报单二维码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? QRCODE { get; set; }

        /// <summary>
        /// Desc:报单所处进程：1.发放，2.填写，3.提交，4.回收，7.转为归属地回收
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int? PROCEEDING { get; set; }

        /// <summary>
        /// Desc:乘务退勤地点代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWTQDDDM { get; set; }

        /// <summary>
        /// Desc:乘务出勤地点代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWCQDDDM { get; set; }

        /// <summary>
        /// Desc:乘务交路代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWJLDM { get; set; }

        /// <summary>
        /// Desc:乘务交路名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWJLMC { get; set; }

        /// <summary>
        /// Desc:报单提交地点代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDTJDDDM { get; set; }

        /// <summary>
        /// Desc:报单回收情况：1.正常回收，2.纸质报单回收，6.手动回收
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? BDHSQK { get; set; }

        /// <summary>
        /// Desc:司机工资代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJGZDM1 { get; set; }

        /// <summary>
        /// Desc:副司机工资代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJGZDM1 { get; set; }

        /// <summary>
        /// Desc:学员工资代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYGZDM1 { get; set; }

        /// <summary>
        /// Desc:司机工资代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJGZDM2 { get; set; }

        /// <summary>
        /// Desc:副司机工资代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJGZDM2 { get; set; }

        /// <summary>
        /// Desc:学员工资代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYGZDM2 { get; set; }

        /// <summary>
        /// Desc:司机输入代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJSRDM1 { get; set; }

        /// <summary>
        /// Desc:副司机输入代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJSRDM1 { get; set; }

        /// <summary>
        /// Desc:学员输入代码1
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYSRDM1 { get; set; }

        /// <summary>
        /// Desc:司机输入代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJSRDM2 { get; set; }

        /// <summary>
        /// Desc:副司机输入代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? FSJSRDM2 { get; set; }

        /// <summary>
        /// Desc:学员输入代码2
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? XYSRDM2 { get; set; }

        /// <summary>
        /// Desc:乘务归属地点代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWGSDDDM { get; set; }

        /// <summary>
        /// Desc:报单发放人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDFFR { get; set; }

        /// <summary>
        /// Desc:报单回收人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDHSR { get; set; }

        /// <summary>
        /// Desc:发往本局
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? FWBJ { get; set; }

        /// <summary>
        /// Desc:发往总公司
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? FWZGS { get; set; }

        /// <summary>
        /// Desc:发往外段
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? FWWD { get; set; }

        /// <summary>
        /// Desc:发往外局
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? FWWJ { get; set; }

        /// <summary>
        /// Desc:支配局代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZPJDM { get; set; }

        /// <summary>
        /// Desc:支配局名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZPJMC { get; set; }

        /// <summary>
        /// Desc:支配段代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZPDDM { get; set; }

        /// <summary>
        /// Desc:支配段名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZPDMC { get; set; }

        /// <summary>
        /// Desc:录入局代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LRJDM { get; set; }

        /// <summary>
        /// Desc:录入局名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LRJMC { get; set; }

        /// <summary>
        /// Desc:录入段代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LRDDM { get; set; }

        /// <summary>
        /// Desc:录入段名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LRDMC { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWYZB1 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWYZB2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWYZB3 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWYZB4 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWYZB5 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CWYZB6 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDXGR { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDFFSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDTJSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDHSSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDXGSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JKJLDWDM { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SFSJJLH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SFSRJLH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SFCZH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SFCZMC { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SFCC { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZDSJJLH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZDSRJLH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZDCZH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZDCZMC { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZDCC { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? TJZT { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SJSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? TJSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? TJZH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YT1BDSJID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDZFSJ { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDZFR { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LCGDJSL1_2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LCGDJCL1_2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LCGDJSL2_2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LCGDJCL2_2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? EVENTTIME { get; set; }

        /// <summary>
        /// Desc:标准消耗
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? BZXH { get; set; }

        /// <summary>
        /// Desc:能耗卡控值
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? NHKK { get; set; }

        /// <summary>
        /// Desc:燃料比例
        /// Default:
        /// Nullable:True
        /// </summary>           
        public double? RLBL { get; set; }

        /// <summary>
        /// Desc:上油方式
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool SYFS { get; set; }

        /// <summary>
        /// Desc:能耗是否异常
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? NHSFYC { get; set; }

        /// <summary>
        /// Desc:机车号是否异常
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JCHSFYC { get; set; }

        /// <summary>
        /// Desc:无人重联机型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JX_WRCL { get; set; }

        /// <summary>
        /// Desc:无人重联机车号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JCH_WRCL { get; set; }

        /// <summary>
        /// Desc:机车统计程序是否已录入
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SFYLR { get; set; }

        /// <summary>
        /// Desc:车间代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CJDM { get; set; }

        /// <summary>
        /// Desc:工作方式
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? GZFS { get; set; }

        /// <summary>
        /// Desc:报单回退时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDHTSJ { get; set; }

        /// <summary>
        /// Desc:报单回退人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDHTR { get; set; }

        /// <summary>
        /// Desc:缺项报单标识：0完整 1无运行信息 2无燃料 3无数据
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? QXBDBS { get; set; }

        /// <summary>
        /// Desc:报单类型代码：1便乘 2看道 9其它
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDLXDM { get; set; }

        /// <summary>
        /// Desc:报单类型名称：1便乘 2看道 9其它
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDLXMC { get; set; }

        /// <summary>
        /// Desc:纸质报单编号:No
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZZBDBH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? CREATEDATE { get; set; }

        /// <summary>
        /// Desc:车队代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? CDDM { get; set; }

        /// <summary>
        /// Desc:1.自动生成 2.一体机提交 3.台式机提交
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? BDTJLY { get; set; }

        /// <summary>
        /// Desc:手动转卡(1:是 0或null:否)
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? SDZK { get; set; }

        /// <summary>
        /// Desc:电表是否损坏(A端)：是，否
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? DBSFSH1 { get; set; }

        /// <summary>
        /// Desc:电表是否损坏(B端)：是，否
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? DBSFSH2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? JCXH { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ZSDJCL2_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ZSDJSL2_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JCL2_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JSL2_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ZSDJCL1_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? ZSDJSL1_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? JCL1_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? JSL1_BF { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? AEI_RD { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? AEI_CD { get; set; }

    }
}
