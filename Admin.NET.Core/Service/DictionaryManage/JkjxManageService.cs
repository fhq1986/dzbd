// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class JkjxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public JkjxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("/api/Jkjx/GetJxByJch")]
    public dynamic GetJxByJch(string jch)
    {
        try
        {
            //var xsxxs = JkjxService.GetJxByJch(jch);
            #region JkjxService.GetJxByJch(jch)
            string sql = @"select jch.jx jxdm,jx.db,jx.jclx from dic_jch jch left join dic_jkjx jx on jch.jx=jx.jxdm where jch = '" + jch + "'";
            //IList<Jkjx> jchs = JkjxRepository.GetGroupByJcxh(sql).ToList();
            var xsxxs = _db.Ado.SqlQuery<Jkjx>(sql).ToList();
            #endregion
            return new { success = true, data = xsxxs };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
