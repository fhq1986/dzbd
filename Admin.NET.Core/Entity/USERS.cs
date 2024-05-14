using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///用户
    ///</summary>
    [SugarTable("USERS")]
    public partial class USERS
    {
           public USERS(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:电邮地址
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? EMAIL {get;set;}

           /// <summary>
           /// Desc:确认电邮地址
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool EMAILCONFIRMED {get;set;}

           /// <summary>
           /// Desc:密码散列
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? PASSWORDHASH {get;set;}

           /// <summary>
           /// Desc:安全戳值
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? SECURITYSTAMP {get;set;}

           /// <summary>
           /// Desc:电话号码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? PHONENUMBER {get;set;}

           /// <summary>
           /// Desc:确认电话号码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool PHONENUMBERCONFIRMED {get;set;}

           /// <summary>
           /// Desc:启用双重认证
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool TWOFACTORENABLED {get;set;}

           /// <summary>
           /// Desc:锁定结束日期
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? LOCKOUTENDDATEUTC {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool LOCKOUTENABLED {get;set;}

           /// <summary>
           /// Desc:访问失败次数
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal ACCESSFAILEDCOUNT {get;set;}

           /// <summary>
           /// Desc:登录名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string USERNAME {get;set;}

           /// <summary>
           /// Desc:姓名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DISPLAYNAME {get;set;}

           /// <summary>
           /// Desc:铁路局
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TLJDM {get;set;}

           /// <summary>
           /// Desc:机务段
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? JWDDM {get;set;}

           /// <summary>
           /// Desc:车间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? CJDM {get;set;}

           /// <summary>
           /// Desc:0:国铁集团 1:铁路局 2:机务段 3:车间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? TYPE {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QTDM {get;set;}

    }
}
