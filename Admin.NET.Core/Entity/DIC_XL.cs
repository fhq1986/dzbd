using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///线路信息
    ///</summary>
    [SugarTable("DIC_XL")]
    public partial class DIC_XL
    {
           public DIC_XL(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:线路代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DM {get;set;}

           /// <summary>
           /// Desc:线路名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? MC {get;set;}

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
