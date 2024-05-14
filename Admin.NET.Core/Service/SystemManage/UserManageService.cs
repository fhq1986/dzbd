// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Service.SystemManage.Dto;
using Admin.NET.Core.Util;
using DapperExtensions;
using DapperExtensions.Predicate;
using Nest;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace Admin.NET.Core.Service.SystemManage;

/// <summary>
/// 用户管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class UserManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public UserManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    /// <summary>
    /// 用户密码重置
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "user_resetpassword")]
    [DisplayName("用户密码重置")]
    [Route("/api/User/ResetPassword")]
    public dynamic ResetPassword(User user)
    {
        try
        {
            //UserService.GetPasswordHash(user);
            //UserService.Update(user);
            //UserService.Commit();
            User model = _db.Queryable<User>()
                            .Where(x => x.Id == user.Id).Single();
            if(model==null)
                return new { success = false, message ="没有找到用户信息" };
            model.PasswordHash = CryptogramUtil.EncryptByAES(user.PasswordHash);
            model.SecurityStamp = model.PasswordHash;
            _db.Updateable(model).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 添加用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "user_add")]
    [DisplayName("添加用户")]
    [Route("/api/User/Add")]
    public dynamic Add(User user)
    {
        try
        {
            //string name = User.Identity.Name;
            string name = App.User?.FindFirst(ClaimConst.Account)?.Value;
            // 用户密码加密
            //UserService.GetPasswordHash(user);
            user.PasswordHash = CryptogramUtil.EncryptByAES(user.PasswordHash);
            //int result = UserService.Add(user);
            int result = _db.Insertable(user).ExecuteCommand();
            if (result > 0)
            {
                result = _db.Queryable<User>()
                            .Where(x => x.UserName == user.UserName)
                            .Single().Id;
            }
            //UserService.Commit();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "user_edit")]
    [DisplayName("更改用户")]
    [Route("/api/User/Update")]
    public dynamic Update(User user)
    {
        try
        {
            //UserService.Update(user);
            //UserService.Commit();
            User model = _db.Queryable<User>()
                            .Where(x => x.Id == user.Id).Single();
            if (model == null)
                return new { success = false, message = "没有找到用户信息" };
            model.AccessFailedCount = user.AccessFailedCount;
            model.UserName = user.UserName;
            model.LockoutEnabled = user.LockoutEnabled;
            model.LockoutEndDateUtc = user.LockoutEndDateUtc;
            model.PhoneNumber = user.PhoneNumber;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            model.PasswordHash = user.PasswordHash;
            model.SecurityStamp = user.SecurityStamp;
            model.Cjdm = user.Cjdm;
            model.EmailConfirmed = user.EmailConfirmed;
            model.TwoFactorEnabled = user.TwoFactorEnabled;
            model.DisplayName = user.DisplayName;
            model.Email = user.Email;
            model.Type = user.Type;
            model.Jwddm = user.Jwddm;
            model.QTDM = user.QTDM;
            model.Tljdm = user.Tljdm;
            if (_db.Updateable(model).ExecuteCommand() <= 0)
                return new { success = false };
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "user_delete")]
    [DisplayName("删除用户")]
    [Route("/api/User/Delete")]
    public dynamic Delete(User user)
    {
        try
        {
            //UserService.Remove(user);
            //UserService.Commit();
            if(_db.Deleteable<User>().Where(x=>x.Id == user.Id).ExecuteCommand()<=0)
                return new { success = false };
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [HttpGet]
    [Authorize(Roles = "user_querycwzydd")]
    [DisplayName("")]
    [Route("/api/User/FindAllCwzyddsByname")]
    public dynamic FindAllCwzyddsByname()
    {
        try
        {
            //IList<Dmmc> dmmcs = UserService.FindAllUserCwzyddByname(User.Identity.Name);
            string userName = App.User?.FindFirst(ClaimConst.RealName).Value;
            string sql = string.Format(@"select u.cwzydddm dm,dd.cwzyddmc mc from usercwzydd u 
                                                left join dic_cwzydd dd on u.cwzydddm=dd.cwzydddm where u.username='{0}'", userName);
            IList<Dmmc> dmmcs = _db.Ado.SqlQuery<Dmmc>(sql);
            return new { success = true, data = dmmcs };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [HttpGet]
    [DisplayName("")]
    [Route("/api/User/FindName")]
    public dynamic FindName(string userName)
    {
        try
        {
            //var user = UserService.FindByName(userName);
            User user = _db.Queryable<User>()
                                .Where(x => x.UserName == userName).Single();
            return new { success = true, user = user };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
