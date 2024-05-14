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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abp.Domain.Uow.AbpDataFilters;

namespace Admin.NET.Core.Service.BdglManage;

/// <summary>
/// 
/// </summary>
[NonUnify]
[AllowAnonymous]
[ApiDescriptionSettings(Order = 390)]
public class BdffManage : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public BdffManage(ISqlSugarClient db)
    {
        _db = db;
    }
    /// <summary>
    /// 根据乘务员代码查姓名
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/Bdff/FindCwyxmByDm")]
    public dynamic FindCwyxmByDm(object param)
    {
        //return CwyxxService.FindCwyxmByDm(param.ToString());
        #region CwyxxService.FindCwyxmByDm(param.ToString())
        try
        {
            if (string.IsNullOrEmpty(param + "") || param == null) return null;
            JObject jo = JObject.Parse(param.ToString());
            string cwydm = jo["cwydm"].ToString();
            #region 
            //if (XtcsService.FindByCsdm("jwbm").Equals(cwydm.Substring(0, 2)))
            //{
            //    IPredicate jwdpredicate = Predicates.Field<Cwyxx>(p => p.Cwydm, Operator.Eq, cwydm);
            //    Cwyxx cwyxx = FindAll(jwdpredicate).FirstOrDefault();
            //    //string cwyxm = cwyxx == null ? "" : cwyxx.Cwyxm;
            //    return new { success = true, data = cwyxx, isBd = true }; //如果是本务乘务员，终端则可以修改密码
            //}
            //else
            //{
            //    Dictionary<string, string> paramters = new Dictionary<string, string>();
            //    paramters.Add("param", cwydm);
            //    return ZgsService.GetToZgs(paramters, "api/Bdff/FindCwyxmByDm");
            //}
            Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jwbm").Single();
            if (modelXtcs != null && modelXtcs.Cs.Equals(cwydm.Substring(0, 2)))
            {
                //IPredicate jwdpredicate = Predicates.Field<Cwyxx>(p => p.Cwydm, Operator.Eq, cwydm);
                //Cwyxx cwyxx = FindAll(jwdpredicate).FirstOrDefault();
                Cwyxx cwyxx = _db.Queryable<Cwyxx>()
                                .Where(x => x.Cwydm == cwydm).Single();
                return new { success = true, data = cwyxx, isBd = true }; //如果是本务乘务员，终端则可以修改密码
            }
            else
            {
                Dictionary<string, string> paramters = new Dictionary<string, string>();
                paramters.Add("param", cwydm);
                //return ZgsService.GetToZgs(paramters, "api/Bdff/FindCwyxmByDm");
                return GetToZgs(paramters, "api/Bdff/FindCwyxmByDm");
            }
            #endregion

        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
        #endregion
    }

    private dynamic GetToZgs(Dictionary<string, string> parameters, string method)
    {
        string msg;
        //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("zgsurl")).FirstOrDefault().Cs;
        Xtcs modelXtcs = _db.Queryable<Xtcs>()
                            .Where(x => x.Csdm == "zgsurl").Single();
        if(modelXtcs==null)
            return new { success = false, data = "没有找到相关系统配置" };
        string url = modelXtcs.Cs;
        IRestConnection restConnection = new RestConnection(url);
        if (restConnection.Login(@"/Token", "username", "cf", "password", "123", out msg))
        {
            JObject reslut = restConnection.Get(method, parameters);
            // 断开连接
            restConnection.Logout(url);
            if (reslut.Value<bool>("success"))
            {
                return new { success = true, data = reslut["data"] };
            }
            else
            {
                return new { success = false, data = reslut["message"] };
            }
        }
        else
        {
            return new { success = false, message = "总公司连接失败！" };
        }
    }
}
