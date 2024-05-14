using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///车间信息
    ///</summary>
    [SugarTable("DIC_CJXX")]
    public partial class DIC_CJXX
    {
           public DIC_CJXX(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:车间代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CJDM {get;set;}

           /// <summary>
           /// Desc:车间名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string CJMC {get;set;}

           /// <summary>
           /// Desc:段代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DDM {get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal ORD {get;set;}

           /// <summary>
           /// Desc:交路代码列表
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JLDMS {get;set;}

           /// <summary>
           /// Desc:大车间代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DCJDM {get;set;}

           /// <summary>
           /// Desc:车间类型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CJLX {get;set;}

           /// <summary>
           /// Desc:车间性质
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XZ {get;set;}

    }
}
