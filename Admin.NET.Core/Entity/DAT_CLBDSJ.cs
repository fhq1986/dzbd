using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///重联报单数据表
    ///</summary>
    [SugarTable("DAT_CLBDSJ")]
    public partial class DAT_CLBDSJ
    {
           public DAT_CLBDSJ(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:对应报单数据ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal BDSJID {get;set;}

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
           /// Desc:列车供电接收量2
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LCGDJSL2 {get;set;}

           /// <summary>
           /// Desc:列车供电交出量2
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
           /// Desc:电表是否损坏(A端)：是，否
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DBSFSH1 {get;set;}

           /// <summary>
           /// Desc:电表是否损坏(B端)：是，否
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DBSFSH2 {get;set;}

    }
}
