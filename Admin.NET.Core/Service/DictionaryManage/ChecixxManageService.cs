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
/// 非固定车次字典信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class ChecixxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public ChecixxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpGet]
    [Authorize(Roles = "checixx_base")]
    [Route("/api/Checixx/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup;
            IList<ISort> sorts;
            //DapperHelper.GeneratePredicateAndSort<Checixx>(param, out predicateGroup, out sorts);
            SqlSugarHelper.GeneratePredicateAndSort<Checixx>(param, out predicateGroup, out sorts);
            //var xls = ChecixxService.FindAll(predicateGroup, sorts);
            var query = _db.Queryable<Checixx>();
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
            var xls = query.ToList();
            return new { success = true, data = new { total = xls.Count(), items = xls } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 添加非图定车次信息
    /// </summary>
    /// <param name="Checixx"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "checixx_add")]
    [Route("/api/Checixx/Add")]
    public dynamic Add([FromBody]Checixx Checixxs)
    {
        try
        {
            //int result = ChecixxService.Add(Checixxs);
            //ChecixxService.Commit();
            //return new { success = true, data = result };
            _db.Insertable(Checixxs).ExecuteCommand();
            Checixx result = _db.Queryable<Checixx>()
                                .Where(x => x.Ddm == Checixxs.Ddm
                                            && x.Cc == Checixxs.Cc
                                            && x.Qudan == Checixxs.Qudan
                                            && x.Ddqy == Checixxs.Ddqy
                                            && x.Sfz == Checixxs.Sfz
                                            && x.Zdz == Checixxs.Zdz
                                            && x.Bdsfz == Checixxs.Bdsfz
                                            && x.Bdzdz == Checixxs.Bdzdz).Single();
            return new { success = true, data = (result==null?0:result.Id) };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 修改非图定车次信息
    /// </summary>
    /// <param name="Checixx"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "checixx_edit")]
    [Route("/api/Checixx/Update")]
    public dynamic Update([FromBody]Checixx Checixxs)
    {
        try
        {
            //ChecixxService.Update(Checixxs);
            //ChecixxService.Commit();
            _db.Updateable(Checixxs).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    /// <summary>
    /// 删除非图定车次信息
    /// </summary>
    /// <param name="Checixx"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "checixx_delete")]
    [Route("/api/Checixx/Remove")]
    public dynamic Remove([FromBody] Checixx Checixxs)
    {
        try
        {
            //ChecixxService.Remove(Checixxs);
            //ChecixxService.Commit();
            _db.Deleteable<Checixx>()
                .Where(x => x.Id == Checixxs.Id).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
