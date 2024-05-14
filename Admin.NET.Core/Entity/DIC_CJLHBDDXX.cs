using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///轮乘换班地点
    ///</summary>
    [SugarTable("DIC_CJLHBDDXX")]
    public partial class DIC_CJLHBDDXX
    {
           public DIC_CJLHBDDXX(){


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
           /// Desc:顺序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

    }
}
