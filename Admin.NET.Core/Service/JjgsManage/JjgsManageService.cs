// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.DapperEx;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abp.Domain.Uow.AbpDataFilters;

namespace Admin.NET.Core.Service.JjgsManage;

/// <summary>
/// 报单类型信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class JjgsManageService : IDynamicApiController, ITransient
{
    ISqlSugarClient _db;
    public JjgsManageService(SqlSugar.ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 从总公司查询运安系统推送的出勤信息
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Jjgs/GetGztjCx")]
    [Authorize(Roles = "jjgs_base")]
    public dynamic GetGztjCx(string param)
    {
        try
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("sWhere", param);
            //return JjgsService.GetToJjgs(dic, "api/Gztjcx/GetGztjCx");
            string action = "api/Gztjcx/GetGztjCx";
            #region JjgsService.GetToJjgs(dic, "api/Gztjcx/GetGztjCx");
            string msg;
            //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("jjgsurl")).FirstOrDefault().Cs;
            Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jjgsurl").Single();
            string url = modelXtcs==null?"":modelXtcs.Cs;
            IRestConnection restConnection = new RestConnection(url);
            try
            {
                JArray reslut = restConnection.GetJarray(action, dic);

                IList<JjgsXx> jjgss = reslut.ToObject<IList<JjgsXx>>();
                // 断开连接
                restConnection.Logout(url);
                return new { success = true, data = jjgss };
            }
            catch (Exception ex)
            {
                // 断开连接
                restConnection.Logout(url);
                return new { success = false, message = "获取计件工资失败" };
            }
            #endregion
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
