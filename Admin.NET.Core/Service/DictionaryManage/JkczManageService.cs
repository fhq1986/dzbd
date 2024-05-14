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
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 监控车站信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class JkczManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public JkczManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "jkcz_base")]
    [Route("/api/Jkcz/FindAll")]
    [HttpGet]
    public dynamic FindAll(string param)
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
            IList<ISort> sorts;
            int pageIndex;
            int pageSize;
            //DapperHelper.GeneratePredicateAndSort<Jkcz>(param, out predicateGroup, out sorts, out pageIndex, out pageSize);
            SqlSugarHelper.GeneratePredicateAndSort<Jkcz>(param, out predicateGroup, out sorts,out pageIndex ,out pageSize);
            //var result = JkczService.FindAll(predicateGroup, sorts, pageIndex, pageSize);
            var query = _db.Queryable<Jkcz>();
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
            var result = query.ToPagedList(pageIndex, pageSize);

            //return new { success = true, data = new { items = result.Data, total = result.TotalRecords, pages = result.TotalPages } };
            return new { success = true, data = new { items = result.Items, total = result.Total, pages = result.TotalPages } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }


    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="jkcz"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "jkcz_add")]
    [Route("/api/Jkcz/Add")]
    public dynamic Add([FromBody] Jkcz jkcz)
    {
        try
        {
            //int result = JkczService.Add(jkcz);
            //JkczService.Commit();
            //return new { success = true, data = result };

            _db.Insertable(jkcz).ExecuteCommand();
            jkcz = _db.Queryable<Jkcz>()
                        .Where(x => x.Jkjldwdm == jkcz.Jkjldwdm
                                    && x.Jkjlh == jkcz.Jkjlh
                                    && x.Jkczh==jkcz.Jkczh
                                    && x.Jkczmc==jkcz.Jkczmc
                                    && x.Ddm == jkcz.Ddm).Single();
            return new { success = true, data = jkcz != null ? jkcz.Id : 0 };
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
    [Authorize(Roles = "jkcz_edit")]
    [Route("/api/Jkcz/Update")]
    public dynamic Update([FromBody]Jkcz Jkcz)
    {
        try
        {
            //JkczService.Update(Jkcz);
            //JkczService.Commit();
            //return new { success = true, data = 1 };

            _db.Updateable(Jkcz).ExecuteCommand();
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
    [Authorize(Roles = "jkcz_delete")]
    [Route("/api/Jkcz/Delete")]
    public dynamic Delete([FromBody] Jkcz Jkcz)
    {
        try
        {
            //JkczService.Remove(Jkcz);
            //JkczService.Commit();
            _db.Deleteable(Jkcz).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [HttpPost]
    [Route("/api/Jkcz/UpdateCzdm")]
    public dynamic UpdateCzdm([FromBody] JObject param)
    {
        try
        {
            //Jkcz jkcz = JkczService.FindByCzh(param);
            var query = _db.Queryable<Jkcz>();
            if (param.ContainsKey("Ddm") && !string.IsNullOrWhiteSpace(param.Value<string>("Ddm")))
            {
                //predList.Add(Predicates.Field<Jkcz>(p => p.Ddm, Operator.Eq, param.Value<string>("Ddm")));
                query = query.Where(x => x.Ddm == param.Value<string>("Ddm"));
            }
            if (param.ContainsKey("Jkjldwdm") && !string.IsNullOrWhiteSpace(param.Value<string>("Jkjldwdm")))
            {
                //predList.Add(Predicates.Field<Jkcz>(p => p.Jkjldwdm, Operator.Eq, param.Value<string>("Jkjldwdm")));
                query = query.Where(x => x.Jkjldwdm == param.Value<string>("Jkjldwdm"));
            }
            if (param.ContainsKey("Jkjlh") && !string.IsNullOrWhiteSpace(param.Value<string>("Jkjlh")))
            {
                //predList.Add(Predicates.Field<Jkcz>(p => p.Jkjlh, Operator.Eq, param.Value<string>("Jkjlh")));
                query = query.Where(x => x.Jkjlh == param.Value<string>("Jkjlh"));
            }
            if (param.ContainsKey("Jkczh") && !string.IsNullOrWhiteSpace(param.Value<string>("Jkczh")))
            {
                //predList.Add(Predicates.Field<Jkcz>(p => p.Jkczh, Operator.Eq, param.Value<string>("Jkczh")));
                query = query.Where(x => x.Jkczh == param.Value<string>("Jkczh"));
            }
            Jkcz jkcz = query.Single();
            if (null != jkcz && param.ContainsKey("Czdm"))
            {
                if (param.Value<string>("Czdm") == ">")
                {
                    jkcz.Zyxbs = "1";
                    jkcz.Czdm = ">";
                }
                else if (param.Value<string>("Czdm") == "<")
                {
                    jkcz.Zyxbs = "0";
                    jkcz.Czdm = "<";
                }
                else
                {
                    jkcz.Czdm = param.Value<string>("Czdm");
                }
            }
            if (null == jkcz)
            {
                return new { success = false, message = "没有找到对应的监控车站记录!" };
            }
            else
            {
                //JkczService.Update(jkcz);
                //JkczService.Commit();
                _db.Updateable(jkcz).ExecuteCommand();
                return new { success = true, data = 1 };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = "更新失败!" };
        }
    }
}
