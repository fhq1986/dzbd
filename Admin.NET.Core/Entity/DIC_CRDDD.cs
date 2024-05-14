using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///出入段地点
    ///</summary>
    [SugarTable("DIC_CRDDD")]
    public partial class DIC_CRDDD
    {
           public DIC_CRDDD(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:机务段代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DDM {get;set;}

           /// <summary>
           /// Desc:地点代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DM {get;set;}

           /// <summary>
           /// Desc:地点名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string MC {get;set;}

           /// <summary>
           /// Desc:简拼码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JPM {get;set;}

           /// <summary>
           /// Desc:入段车站
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? RDCZ {get;set;}

           /// <summary>
           /// Desc:顺序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

           /// <summary>
           /// Desc:入段车站名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? RDCZMC {get;set;}

           /// <summary>
           /// Desc:作业时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZYSJ {get;set;}

    }
}
