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
/// 出入段地点权限信息管理接口
/// </summary>
[AllowAnonymous]
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class CrdddxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CrdddxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    /// <summary>
    /// 查询全部出入段信息
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [Route("/api/Crdddxx/FindAll")]
    [HttpGet]
    [Authorize(Roles = "crdddxx_base")]
    public dynamic FindAll(string param="")
    {
        //PredicateGroup predicateGroup;
        List<IConditionalModel> predicateGroup;
        IList<ISort> sorts;
        //DapperHelper.GeneratePredicateAndSort<Crddd>(param, out predicateGroup, out sorts);
        SqlSugarHelper.GeneratePredicateAndSort<Crddd>(param, out predicateGroup, out sorts);
        //var Crdddxx = crdddxxService.FindAll(predicateGroup, sorts);
        var query = _db.Queryable<Crddd>();
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
        var Crdddxx = query.ToList();
        return new { success = true, data = new { total = Crdddxx.Count(), items = Crdddxx } };
    }

    /// <summary>
    /// 添加机型
    /// </summary>
    /// <param name="Crddd"></param>
    /// <returns></returns>
    [Route("/api/Crdddxx/Add")]
    [HttpPost]
    [Authorize(Roles = "crdddxx_add")]
    public dynamic Add([FromBody] Crddd Crddd)
    {
        //int result = crdddxxService.Add(Crddd);
        //crdddxxService.Commit();
        _db.Insertable<Crddd>(Crddd).ExecuteCommand();
        Crddd model = _db.Queryable<Crddd>()
                        .Where(x => x.Ddm == Crddd.Ddm
                                    && x.Dm == Crddd.Dm
                                    && x.Mc == Crddd.Mc
                                    && x.Jpm == Crddd.Jpm
                                    && x.Rdcz == Crddd.Rdcz
                                    && x.Rdczmc == Crddd.Rdczmc
                                    && x.Zysj == Crddd.Zysj).Single();
        int result = model == null ? 0 : model.Id;
        return new { success = true, data = result };
    }

    /// <summary>
    /// 修改机型
    /// </summary>
    /// <param name="Crdddxx"></param>
    /// <returns></returns>
    [Route("/api/Crdddxx/Update")]
    [HttpPost]
    [Authorize(Roles = "crdddxx_edit")]
    public dynamic Update([FromBody] Crddd Crdddxx)
    {
        //crdddxxService.Update(Crdddxx);
        //crdddxxService.Commit();
        _db.Updateable(Crdddxx).ExecuteCommand();
        return new { success = true };
    }

    /// <summary>
    /// 删除机型
    /// </summary>
    /// <param name="Crdddxx"></param>
    /// <returns></returns>
    [Route("/api/Crdddxx/Remove")]
    [HttpPost]
    [Authorize(Roles = "crdddxx_delete")]
    public dynamic Remove([FromBody] Crddd Crdddxx)
    {
        //crdddxxService.Remove(Crdddxx);
        //crdddxxService.Commit();
        _db.Deleteable<Crddd>()
            .Where(x => x.Id == Crdddxx.Id).ExecuteCommand();
        return new { success = true };
    }

    /// <summary>
    /// 添加出入段地点权限(更新权限按钮)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Route("/api/Crdddxx/CrdqxAdd")]
    [HttpGet]
    [Authorize(Roles = "crdqx_add")]
    public dynamic CrdqxAdd(string users, string roles)
    {
        try
        {
            //int result = crdddxxService.CrdqxAdd(users, roles);
            //crdddxxService.Commit();
            var userjas = JArray.Parse(users).Values().ToList();
            var rolejas = JArray.Parse(roles).Values().ToList();
            int result = 0;
            _db.Ado.BeginTran();
            foreach (JToken userja in userjas)
            {
                int userid = int.Parse(userja.ToString());
                string delsql = string.Format("delete from {0} where userid = '{1}'", "Dic_user_crddd", userid);
                //先删
                //UserCrdddRepository.Context.ExecuteSqlCommand(delsql);
                _db.Ado.ExecuteCommand(delsql);
                foreach (JToken roleja in rolejas)
                {
                    int ddid = int.Parse(roleja.ToString());
                    //用于判断点击的角色id是否在与角色表中
                    string insql = string.Format(@"insert into {0} values('{1}','{2}')", "Dic_user_crddd", userid, ddid);
                    //result = UserCrdddRepository.Context.ExecuteSqlCommand(insql);
                    result=_db.Ado.ExecuteCommand(insql);
                }
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
    /// 根据当前用户查询出入段地点权限表(下拉框)
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [Route("/api/Crdddxx/FindCrdqx")]
    [HttpGet]
    [Authorize(Roles = "crdqx_base")]
    public dynamic FindCrdqx(string param)
    {
        JObject json = JObject.Parse(param);
        string sql = @"select dic_crddd.dm as DM,dic_crddd.mc as MC from dic_user_crddd left join dic_crddd on dic_user_crddd.ddid=dic_crddd.id where 1=1";
        if (json.ContainsKey("UserId") && !string.IsNullOrEmpty(json["UserId"].ToString()))
        {
            sql = sql + " and UserId=" + json["UserId"] + "";
        }
        //var UserCrddd = UserCrdddRepository.FindAllByCondition(sql);
        var UserCrddd=_db.Ado.SqlQuery<UserCrddd>(sql).ToList();
        return new { success = true, data = new { total = UserCrddd.Count(), items = UserCrddd } };
    }
}
