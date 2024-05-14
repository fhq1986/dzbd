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

namespace Admin.NET.Core.Service.SystemManage;
/// <summary>
/// 系统参数管理
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class SystemConfigManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public SystemConfigManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 添加系统参数
    /// </summary>
    /// <param name="xtcs"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "xtcs_add")]
    [DisplayName("添加系统参数")]
    [Route("/api/Xtcs/Add")]
    public dynamic Add(Xtcs xtcs)
    {
        try
        {
            //int result = XtcsService.Add(xtcs);
            //XtcsService.Commit();
            _db.Ado.BeginTran();
            int result = _db.Insertable(xtcs).ExecuteCommand();
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
    /// 修改系统参数
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "xtcs_edit")]
    [DisplayName("修改系统参数")]
    [Route("/api/Xtcs/Update")]
    public dynamic Update(string param)
    {
        //return XtcsService.Update(param);
        if (string.IsNullOrEmpty(param) || param == null) return null;
        // 集合从前台转向后台
        var xtcses = JArray.Parse(param).Children();
        try
        {
            _db.Ado.BeginTran();
            foreach (var xtcs in xtcses)
            {
                Xtcs xtcsob = xtcs.ToObject<Xtcs>();
                //Update(xtcsob);
                _db.Updateable(xtcsob).ExecuteCommand();
            }
            _db.Ado.CommitTran();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 获取所有系统参数
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "xtcs_base")]
    [DisplayName("获取所有系统参数")]
    [Route("/api/Xtcs/FindAll")]
    public dynamic FindAll()
    {
        try
        {
            //var xtcses = XtcsService.FindAll().OrderBy(p => p.Ord);
            var xtcses = _db.Queryable<Xtcs>().OrderBy(x => SqlFunc.ToInt32(x.Ord)).ToList();
            return new { success = true, data = xtcses };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

}
