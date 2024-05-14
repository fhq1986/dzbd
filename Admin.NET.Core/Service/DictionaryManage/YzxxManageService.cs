// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Util;
using DapperExtensions.Predicate;
using DzbdJwd.Domain.Models.Zdxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 油脂信息管理接口
/// </summary>
[AllowAnonymous]
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class YzxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public YzxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpGet]
    [Authorize(Roles = "yzxx_base")]
    [Route("/api/Yzxx/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
            IList<ISort> sorts;
            //DapperHelper.GeneratePredicateAndSort<Yzxx>(param, out predicateGroup, out sorts);
            //var Yzxxs = YzxxService.FindAll(predicateGroup, sorts).OrderBy(p => p.Ord).ToList();
            SqlSugarHelper.GeneratePredicateAndSort<Yzxx>(param, out predicateGroup, out sorts);
            var query = _db.Queryable<Yzxx>();
            if (predicateGroup != null
                && predicateGroup.Count > 0)
            {
                query = query.Where(predicateGroup);
            }
            if (sorts != null
                && sorts.Count > 0)
            {
                foreach (var item in sorts)
                {
                    if (item.Ascending)
                        query = query.OrderBy($"{item.PropertyName} asc");
                    else
                        query = query.OrderBy($"{item.PropertyName} desc");
                }
            }
            var Yzxxs = query.ToList();
            return new { success = true, data = new { total = Yzxxs.Count(), items = Yzxxs } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
