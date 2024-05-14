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
using DapperExtensions;
using DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;

/// <summary>
/// 机务段信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class JwdxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public JwdxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="param">参数json串</param>
    /// <returns></returns>
    //[HttpGet]
    //[Authorize(Roles = "jwdxx_base")]
    //[DisplayName("分页查询")]
    //[Route("/api/Jwdxx/FindAll")]
    //public dynamic FindAll(string param)
    //{
    //    try
    //    {
    //        //PredicateGroup predicateGroup;
    //        List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
    //        IList<ISort> sorts;
    //        //DapperHelper.GeneratePredicateAndSort<Jwdxx>(param, out predicateGroup, out sorts);
    //        SqlSugarHelper.GeneratePredicateAndSort<Jwdxx>(param, out predicateGroup, out sorts);
    //        //var jwdxxs = JwdxxService.FindAll(predicateGroup, sorts);
    //        var query = _db.Queryable<Jwdxx>();
    //        if (predicateGroup != null
    //            && predicateGroup.Count > 0)
    //        {
    //            query = query.Where(predicateGroup);
    //        }
    //        if (sorts != null
    //            && sorts.Count > 0)
    //        {
    //            foreach (var item in sorts)
    //            {
    //                if (item.Ascending)
    //                    query = query.OrderBy($"{item.PropertyName} asc");
    //                else
    //                    query = query.OrderBy($"{item.PropertyName} desc");
    //            }
    //        }
    //        var jwdxxs = query.ToList();
    //        return new { success = true, data = new { total = jwdxxs.Count(), items = jwdxxs } };
    //    }
    //    catch (Exception e)
    //    {
    //        return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
    //    }
    //}

    /// <summary>
    /// 得到本局机务段信息
    /// </summary>
    /// <param name="param">参数json串</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "jwdxx_base")]
    [DisplayName("得到本局机务段信息")]
    [Route("/api/Jwdxx/GetBjJwdxx")]
    public dynamic GetBjJwdxx()
    {
        try
        {
            //string tljdm = XtcsService.FindByCsdm("tljdm");
            string tljdm = _db.Queryable<Xtcs>().Where(x => x.Csdm == "tljdm").Single().Cs;
            //var jwdxxs = JwdxxService.FindAll(Predicates.Field<Jwdxx>(p => p.Tljdm, Operator.Eq, tljdm));
            var jwdxxs = _db.Queryable<Jwdxx>().Where(x => x.Tljdm == tljdm).ToList();
            return new { success = true, data = new { total = jwdxxs.Count(), items = jwdxxs } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }


    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="jwdxx"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "jwdxx_add")]
    [Route("/api/Jwdxx/Add")]
    public dynamic Add(Jwdxx jwdxx)
    {
        try
        {
            //int result = JwdxxService.Add(jwdxx);
            //JwdxxService.Commit();
            //return new { success = true, data = result };

            _db.Insertable(jwdxx).ExecuteCommand();
            jwdxx = _db.Queryable<Jwdxx>()
                        .Where(x => x.Tljdm == jwdxx.Tljdm
                                    && x.Jwdmc == jwdxx.Jwdmc
                                    && x.Jwbm == jwdxx.Jwbm
                                    && x.Jwddm==jwdxx.Jwddm).Single();
            return new { success = true, data = jwdxx != null ? jwdxx.Id : 0 };
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
    [Authorize(Roles = "jwdxx_edit")]
    [Route("/api/Jwdxx/Update")]
    public dynamic Update(Jwdxx Jwdxx)
    {
        try
        {
            //JwdxxService.Update(Jwdxx);
            //JwdxxService.Commit();

            _db.Updateable(Jwdxx).ExecuteCommand();
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
    [Authorize(Roles = "jwdxx_delete")]
    [Route("/api/Jwdxx/Remove")]
    public dynamic Remove(Jwdxx jwdxx)
    {
        try
        {
            //JwdxxService.Remove(jwdxx);
            //JwdxxService.Commit();
            _db.Deleteable(jwdxx).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
