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
using DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;

/// <summary>
/// 监控交路单位信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class JkjldwManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public JkjldwManageService(ISqlSugarClient db)
    {
        _db = db;
    }


    /// <summary>
    /// 分页查询
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "jkjldw_base")]
    [Route("/api/Jkjldw/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
            IList<ISort> sorts;
            //DapperHelper.GeneratePredicateAndSort<Jkjldw>(param, out predicateGroup, out sorts);
            SqlSugarHelper.GeneratePredicateAndSort<Jkjldw>(param, out predicateGroup, out sorts);
            //var jkjldws = JkjldwService.FindAll(predicateGroup, sorts);
            var query = _db.Queryable<Jkjldw>();
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
            var jkjldws = query.ToList();
            return new { success = true, data = new { total = jkjldws.Count(), items = jkjldws } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="jkjldw"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "jkjldw_add")]
    [Route("/api/Jkjldw/Add")]
    public dynamic Add(Jkjldw jkjldw)
    {
        try
        {
            //int result = JkjldwService.Add(jkjldw);
            //JkjldwService.Commit();
            //return new { success = true, data = result };
            _db.Insertable(jkjldw).ExecuteCommand();
            jkjldw = _db.Queryable<Jkjldw>()
                        .Where(x => x.Jkjldwdm == jkjldw.Jkjldwdm
                                    && x.Jkjldwmc == jkjldw.Jkjldwmc
                                    && x.Ddm == jkjldw.Ddm).Single();
            return new { success = true, data = jkjldw != null ? jkjldw.Id : 0 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "jkjldw_edit")]
    [Route("/api/Jkjldw/Update")]
    public dynamic Update(Jkjldw jkjldw)
    {
        try
        {
            //JkjldwService.Update(jkjldw);
            //JkjldwService.Commit();
            //return new { success = true, data = 1 };

            _db.Updateable(jkjldw).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "jkjldw_delete")]
    [Route("/api/Jkjldw/Delete")]
    public dynamic Delete(Jkjldw jkjldw)
    {
        try
        {
            //JkjldwService.Remove(jkjldw);
            //JkjldwService.Commit();

            _db.Deleteable(jkjldw).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
