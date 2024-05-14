using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///机型
    ///</summary>
    [SugarTable("DIC_JX")]
    public partial class DIC_JX
    {
           public DIC_JX(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:机种
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JZ {get;set;}

           /// <summary>
           /// Desc:机型
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JX {get;set;}

           /// <summary>
           /// Desc:组号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZH {get;set;}

           /// <summary>
           /// Desc:自重
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ZIZ {get;set;}

           /// <summary>
           /// Desc:功率
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? GL {get;set;}

           /// <summary>
           /// Desc:油电表数
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? YDBS {get;set;}

           /// <summary>
           /// Desc:动车组标识
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool DCZBS {get;set;}

           /// <summary>
           /// Desc:再生电标识
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool ZSDBS {get;set;}

           /// <summary>
           /// Desc:直供电标识
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool ZGDBS {get;set;}

    }
}
