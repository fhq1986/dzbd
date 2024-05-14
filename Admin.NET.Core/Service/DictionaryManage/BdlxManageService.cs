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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 报单类型信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class BdlxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public BdlxManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    [HttpGet]
    [Route("/api/Bdlx/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            //给定一个默认的排序属性字段
            //var users = BdlxService.FindAll(param);
            #region BdlxService.FindAll(param)
            JObject jo = null;
            IList<ISort> sorts = new List<ISort>();
            if (!string.IsNullOrEmpty(param))
            {
                jo = JObject.Parse(param);
                sorts.Add(new Sort { PropertyName = jo["sort"].ToString(), Ascending = true });
            }
            if (sorts.Count == 0)
            {
                sorts.Add(new Sort { PropertyName = "Ord", Ascending = true });
            }
            //return BdlxRepository.FindAll(sorts);
            #region BdlxRepository.FindAll(sorts);
            var query = _db.Queryable<Bdlx>();
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
            var users = query.ToList();
            #endregion
            #endregion
            return new { success = true, data = new { total = users.Count(), items = users } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "bdlx_add")]
    [Route("/api/Bdlx/Insert")]
    public dynamic Insert([FromBody]Bdlx bdlx)
    {
        try
        {
            //int result = BdlxService.Add(bdlx);
            //BdlxService.Commit();
            int result = _db.Insertable(bdlx).ExecuteCommand();
            if(result>0)
            {
                bdlx = _db.Queryable<Bdlx>()
                        .Where(x => x.Dm == bdlx.Dm
                                    && x.Mc == bdlx.Mc
                                    && x.Ord == bdlx.Ord).Single();
                result=bdlx==null? 0 : bdlx.Id;
            }
            return new { success = true, data = new { items = result } };
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
    [Authorize(Roles = "bdlx_edit")]
    [Route("/api/Bdlx/Update")]
    public dynamic Update([FromBody] Bdlx bdlx)
    {
        try
        {
            //BdlxService.Update(bdlx);
            //BdlxService.Commit();
            _db.Updateable(bdlx).ExecuteCommand();
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
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "bdlx_delete")]
    [Route("/api/Bdlx/Delete")]
    public dynamic Delete([FromBody] Bdlx bdlx)
    {
        try
        {
            //BdlxService.Remove(bdlx);
            //BdlxService.Commit();
            _db.Deleteable(bdlx).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
