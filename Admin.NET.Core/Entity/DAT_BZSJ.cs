using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///编组数据
    ///</summary>
    [SugarTable("DAT_BZSJ")]
    public partial class DAT_BZSJ
    {
           public DAT_BZSJ(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:车次
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CC {get;set;}

           /// <summary>
           /// Desc:客货类型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? KHLX {get;set;}

           /// <summary>
           /// Desc:客货本补
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? KHBB {get;set;}

           /// <summary>
           /// Desc:总重
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ZZ {get;set;}

           /// <summary>
           /// Desc:载重
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ZAIZ {get;set;}

           /// <summary>
           /// Desc:客车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? KC {get;set;}

           /// <summary>
           /// Desc:担当企业
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DDQY {get;set;}

           /// <summary>
           /// Desc:其中加挂
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QZJG {get;set;}

           /// <summary>
           /// Desc:重车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ZC {get;set;}

           /// <summary>
           /// Desc:空车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? KONGC {get;set;}

           /// <summary>
           /// Desc:非运用
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? FYYC {get;set;}

           /// <summary>
           /// Desc:代客车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? DKC {get;set;}

           /// <summary>
           /// Desc:其它
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? QT {get;set;}

           /// <summary>
           /// Desc:辆数合计
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? LS {get;set;}

           /// <summary>
           /// Desc:换长
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? HC {get;set;}

           /// <summary>
           /// Desc:到达
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DD {get;set;}

           /// <summary>
           /// Desc:出发
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CF {get;set;}

           /// <summary>
           /// Desc:机外停车时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JWTCSJ {get;set;}

           /// <summary>
           /// Desc:区间停车时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QJTCSJ {get;set;}

           /// <summary>
           /// Desc:实际交路号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SJJLH {get;set;}

           /// <summary>
           /// Desc:输入交路号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SRJLH {get;set;}

           /// <summary>
           /// Desc:车站号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CZH {get;set;}

           /// <summary>
           /// Desc:车站名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CZMC {get;set;}

           /// <summary>
           /// Desc:区间公里
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? QJGL {get;set;}

           /// <summary>
           /// Desc:调车时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DCSJ {get;set;}

           /// <summary>
           /// Desc:调车方式
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DCFS {get;set;}

           /// <summary>
           /// Desc:入段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? RD {get;set;}

           /// <summary>
           /// Desc:出段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CD {get;set;}

           /// <summary>
           /// Desc:监控单位
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JKDW {get;set;}

           /// <summary>
           /// Desc:重联机车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CLJC {get;set;}

           /// <summary>
           /// Desc:编组次数
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? BZCS {get;set;}

           /// <summary>
           /// Desc:确报数据ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QBSJID {get;set;}

           /// <summary>
           /// Desc:确报数据ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QSYT1ID {get;set;}

           /// <summary>
           /// Desc:客编数据ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? KBSJID {get;set;}

           /// <summary>
           /// Desc:报单数据ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int BDSJID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TJCZDM {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TJCZMC {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YT1BDSJID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? YT1ID {get;set;}

           /// <summary>
           /// Desc:附挂机车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FGJC {get;set;}

    }
}
