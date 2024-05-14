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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 轮乘换班地点权限管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class CjlhbddxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CjlhbddxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }


    /// <summary>
    /// 查询全部车间轮乘换班地点信息
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [Route("/api/Cjlhbddxx/FindAll")]
    [HttpGet]
    [Authorize(Roles = "cjlhbddxx_base")]
    public dynamic FindAll(string param="")
    {
        //PredicateGroup predicateGroup;
        List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
        IList<ISort> sorts;
        //DapperHelper.GeneratePredicateAndSort<Cjlhbddxx>(param, out predicateGroup, out sorts);
        SqlSugarHelper.GeneratePredicateAndSort<Cjlhbddxx>(param, out predicateGroup, out sorts);
        //var cjlhbddxx = cjlhbddxxService.FindAll(predicateGroup, sorts);
        var query = _db.Queryable<Cjlhbddxx>();
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
        var cjlhbddxx = query.ToList();
        return new { success = true, data = new { total = cjlhbddxx.Count(), items = cjlhbddxx } };
    }

    /// <summary>
    /// 添加线路区间信息
    /// </summary>
    /// <param name="cjlhbddxx"></param>
    /// <returns></returns>
    [Route("/api/Cjlhbddxx/Add")]
    [HttpPost]
    [Authorize(Roles = "cjlhbddxx_add")]
    public dynamic Add([FromBody] Cjlhbddxx cjlhbddxx)
    {
        //int result = cjlhbddxxService.Add(cjlhbddxx);
        //cjlhbddxxService.Commit();
        //return new { success = true, data = result };
        _db.Insertable<Cjlhbddxx>(cjlhbddxx).ExecuteCommand();
        Cjlhbddxx result = _db.Queryable<Cjlhbddxx>()
                            .Where(x => x.Ddm == cjlhbddxx.Ddm
                                        && x.Dm == cjlhbddxx.Dm
                                        && x.Mc == cjlhbddxx.Mc
                                        && x.Jpm == cjlhbddxx.Jpm).Single();
        return new { success = true, data = (result==null?0:result.Id) };
    }

    /// <summary>
    /// 修改线路区间信息
    /// </summary>
    /// <param name="cjlhbddxx"></param>
    /// <returns></returns>
    [Route("/api/Cjlhbddxx/Update")]
    [HttpPost]
    [Authorize(Roles = "cjlhbddxx_edit")]
    public dynamic Update([FromBody] Cjlhbddxx cjlhbddxx)
    {
        //cjlhbddxxService.Update(cjlhbddxx);
        //cjlhbddxxService.Commit();
        _db.Updateable(cjlhbddxx).ExecuteCommand();
        return new { success = true };
    }

    /// <summary>
    /// 删除线路区间信息
    /// </summary>
    /// <param name="cjlhbddxx"></param>
    /// <returns></returns>
    [Route("/api/Cjlhbddxx/Remove")]
    [HttpPost]
    [Authorize(Roles = "cjlhbddxx_delete")]
    public dynamic Remove([FromBody] Cjlhbddxx cjlhbddxx)
    {
        //cjlhbddxxService.Remove(cjlhbddxx);
        //cjlhbddxxService.Commit();
        _db.Deleteable<Cjlhbddxx>()
            .Where(x => x.Id == cjlhbddxx.Id).ExecuteCommand();
        return new { success = true };
    }

    /// <summary>
    /// 添加轮乘换班地点权限(更新权限按钮)
    /// </summary>
    /// <param name="users"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    [Route("/api/Cjlhbddxx/LchbqxAdd")]
    [HttpGet]
    [Authorize(Roles = "lchbqx_add")]
    public dynamic LchbqxAdd(string users, string roles)
    {
        try
        {
            //int result = cjlhbddxxService.LchbqxAdd(users, roles);
            //cjlhbddxxService.Commit();
            _db.Ado.BeginTran();
            var userjas = JArray.Parse(users).Values().ToList();
            var rolejas = JArray.Parse(roles).Values().ToList();
            int result = 0;
            List<DIC_USER_CJLHBDDXX> lst = new List<DIC_USER_CJLHBDDXX>();
            foreach (JToken userja in userjas)
            {
                lst.Clear();
                int userid = int.Parse(userja.ToString());
                string delsql = string.Format("delete from {0} where userid = '{1}'", "dic_user_cjlhbddxx", userid);
                //先删
                //cjlhbddxxRepository.Context.ExecuteSqlCommand(delsql);
                _db.Ado.ExecuteCommand(delsql);
                foreach (JToken roleja in rolejas)
                {
                    int ddid = int.Parse(roleja.ToString());
                    //用于判断点击的角色id是否在与角色表中
                    //string insql = string.Format(@"insert into {0} values('{1}','{2}')", "dic_user_cjlhbddxx", userid, ddid);
                    //result = cjlhbddxxRepository.Context.ExecuteSqlCommand(insql);
                    lst.Add(new DIC_USER_CJLHBDDXX()
                    {
                        USERID = userid,
                        DDID = ddid
                    });
                }
                result=_db.Insertable(lst).ExecuteCommand();
            }
            _db.Ado.CommitTran();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 根据当前用户查询轮乘换班地点权限表(下拉框)
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [Route("/api/Cjlhbddxx/FindLchbqx")]
    [HttpGet]
    [Authorize(Roles = "lchbqx_base")]
    public dynamic FindLchbqx(string param="")
    {
        string sql = @"select dic_cjlhbddxx.dm as DM,dic_cjlhbddxx.mc as MC from dic_user_cjlhbddxx left join dic_cjlhbddxx on dic_user_cjlhbddxx.ddid=dic_cjlhbddxx.id where 1=1";
        if(!string.IsNullOrWhiteSpace(param))
        {
            JObject json = JObject.Parse(param);
            if (json.ContainsKey("UserId") && !string.IsNullOrEmpty(json["UserId"]+""))
            {
                sql = sql + " and UserId=" + json["UserId"] + "";
            }
        }
        //var UserCrddd = UserCjlhbddxxRepository.FindAllByCondition(sql);
        List<UserCjlhbddxx> UserCrddd=_db.Ado.SqlQuery<UserCjlhbddxx>(sql);
        return new { success = true, data = new { total = UserCrddd.Count(), items = UserCrddd } };
    }
}
