using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///用户乘务作业地点
    ///</summary>
    [SugarTable("USERCWZYDD")]
    public partial class USERCWZYDD
    {
           public USERCWZYDD(){


           }
           /// <summary>
           /// Desc:登录名
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string USERNAME {get;set;}

           /// <summary>
           /// Desc:乘务作业地点代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string CWZYDDDM {get;set;}

    }
}
