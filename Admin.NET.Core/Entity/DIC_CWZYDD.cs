using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///乘务作业地点
    ///</summary>
    [SugarTable("DIC_CWZYDD")]
    public partial class DIC_CWZYDD
    {
           public DIC_CWZYDD(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:乘务作业地点代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CWZYDDDM {get;set;}

           /// <summary>
           /// Desc:乘务作业地点名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CWZYDDMC {get;set;}

           /// <summary>
           /// Desc:机务段代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JWDDM {get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

           /// <summary>
           /// Desc:乘务作业地点对应机务段代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ZYDDDYJWDDM {get;set;}

           /// <summary>
           /// Desc:派班室对应的交路信息
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JLDMS {get;set;}

    }
}
