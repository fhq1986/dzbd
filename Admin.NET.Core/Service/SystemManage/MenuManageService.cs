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
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore.Metadata;
using Nest;
using OBS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Nest.JoinField;

namespace Admin.NET.Core.Service.SystemManage;

/// <summary>
/// 菜单管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class MenuManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public MenuManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "menu_base")]
    [DisplayName("分页查询")]
    [Route("/api/Menu/FindAll")]
    public dynamic FindAll()
    {
        try
        {
            IList<MenuTreeGrid> trees = new List<MenuTreeGrid>();
            //IList<Menu> menus = MenuRepository.FindAll().OrderBy(p => p.Ord).ToList();
            IList<Menu> menus = _db.Queryable<Menu>().OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < menus.Count; i++)
            {
                Menu menu = menus[i];
                int childnum = menus.Where(p => p.Parentid == menu.Id).Count();
                int depth = menu.Parentid == null ? 1 : 2;
                MenuTreeGrid tree = new MenuTreeGrid()
                {
                    id = menu.Id,
                    Title = menu.Title,
                    Component = menu.Component,
                    Enabled = menu.Enabled,
                    Name = menu.Name,
                    Icon = menu.Icon,
                    Path = menu.Path,
                    Roles = menu.Roles,
                    Ord = menu.Ord,
                    parent_id = menu.Parentid,
                    child_num = childnum,
                    depth = depth
                };
                trees.Add(tree);
            }
            return new { success = true, data = new { total = trees.Count(), items = trees } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }
    /// <summary>
    /// 获取菜单树
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("获取菜单树")]
    [Route("/api/Menu/GetAllMenuTree")]
    public dynamic GetAllMenuTree()
    {
        try
        {
            //IEnumerable<Menu> menus = MenuService.GetAllMenuTree();
            List<Role> roles = _db.Queryable<Role>().ToList();
            IEnumerable<Menu> menus = GetAllMenuTreePrivate(roles);

            return new { success = true, total = menus.Count(), lstResult = menus };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "menu_newlist")]
    [DisplayName("")]
    [Route("/api/Menu/NewList")]
    public dynamic NewList([FromBody]MenuItem menuItem)
    {
        try
        {
            string sql = "";
            var reflag = true;
            var restr = "";
            _db.Ado.BeginTran();
            //MenuService.RefreshList(menuItem, ref reflag, ref restr);
            #region MenuService.RefreshList
            var addMenuList = new List<Menu>();
            var delMenuList = new List<int>();
            var updMenuList = new List<Menu>();
            var addRoleList = new List<Role>();
            var delRoleList = new List<int>();
            var updRoleList = new List<Role>();
            // 菜单
            for (int i = 0; i < menuItem.MenuItems.Count; i++)
            {
                if (menuItem.MenuItems[i].Status == "1")
                {
                    addMenuList.Add(menuItem.MenuItems[i]);
                }
                else if (menuItem.MenuItems[i].Status == "2")
                {
                    delMenuList.Add(menuItem.MenuItems[i].Id);
                }
                else if (menuItem.MenuItems[i].Status == "3")
                {
                    updMenuList.Add(menuItem.MenuItems[i]);
                }
            }

            // 权限
            for (int i = 0; i < menuItem.Roles.Count; i++)
            {
                if (menuItem.Roles[i].Status == "1")
                {
                    addRoleList.Add(menuItem.Roles[i]);
                }
                else if (menuItem.Roles[i].Status == "2")
                {
                    delRoleList.Add(menuItem.Roles[i].Id);
                }
                else if (menuItem.Roles[i].Status == "3")
                {
                    updRoleList.Add(menuItem.Roles[i]);
                }
            }

            //删除权限
            //MenuRepository.BulkRoleDel(delRoleList);
            _db.Deleteable<Role>().Where(x => delRoleList.Contains(x.Id)).ExecuteCommand();

            //删除菜单
            //MenuRepository.BulkDel(delMenuList);
            _db.Deleteable<Menu>().Where(x=>delMenuList.Contains(x.Id)).ExecuteCommand();


            var addMenuList2 = new List<Menu>();
            //保证父菜单先添加
            foreach (Menu menu in addMenuList)
            {
                var menuListTmp = new List<Menu>();
                menuListTmp.Add(menu);
                var menuParent = menu;
                while (menuParent.Parentid.HasValue && menuParent.Parentid > 0)
                {
                    menuParent = addMenuList.FirstOrDefault<Menu>(p => p.Id == menuParent.Parentid);
                    if (menuParent == null)
                    {
                        break;
                    }
                    else
                    {
                        menuListTmp.Add(menuParent);
                    }
                }
                for (int i = menuListTmp.Count - 1; i >= 0; i--)
                {
                    if (addMenuList2.Where(p => p.Id == menuListTmp[i].Id).Count() > 0) continue;
                    addMenuList2.Add(menuListTmp[i]);
                }

            }

            //新增菜单
            foreach (Menu menu in addMenuList2)
            {
                try
                {
                    string enbled = menu.Enabled == true ? "1" : "0";
                    string ord = menu.Ord == null ? "null" : menu.Ord.ToString();
                    var pid = menu.Parentid == null ? "null" : menu.Parentid.ToString();
                    string component = string.IsNullOrWhiteSpace(menu.Component) ? "null" : "'" + menu.Component + "'";
                    string icon = string.IsNullOrWhiteSpace(menu.Icon) ? "null" : "'" + menu.Icon + "'";
                    sql = string.Format($@"INSERT INTO MENUITEM(ID,TITLE,NAME,COMPONENT,ICON,PATH,ORD,PARENTID,ENABLED,NOCACHE) 
                    VALUES({menu.Id},'{menu.Title}','{menu.Name}',{component},{icon},'{menu.Path}',{ord},{pid},{enbled},'{menu.NoCache}')");
                    //var succ = MenuRepository.Context.ExecuteSqlCommand(sql);
                    var succ = _db.Ado.ExecuteCommand(sql);
                }
                catch (Exception e)
                {
                    reflag = false;
                    restr = sql;
                    _db.Ado.RollbackTran();
                    return new { success = false, message = sql };
                }
            }

            //更新菜单
            //MenuRepository.BulkUpdate(updMenuList, ref reflag, ref restr);
            if (updMenuList != null && updMenuList.Count() > 0)
            {
                for (var i = 0; i < updMenuList.Count; i++)
                {
                    var ord = updMenuList[i].Ord;
                    var id = updMenuList[i].Id;
                    var title = updMenuList[i].Title;
                    var name = updMenuList[i].Name;
                    var path = updMenuList[i].Path;
                    var icon = string.IsNullOrWhiteSpace(updMenuList[i].Icon) ? "null" : "'" + updMenuList[i].Icon + "'";
                    var pid = updMenuList[i].Parentid == null ? "null" : updMenuList[i].Parentid.ToString();
                    var enabled = updMenuList[i].Enabled == true ? 1 : 0;
                    var nocache = updMenuList[i].NoCache;
                    string component = string.IsNullOrWhiteSpace(updMenuList[i].Component) ? "null" : "'" + updMenuList[i].Component + "'";
                    sql = $@"update menuitem set Title='{title}',
                    Name='{name}',
                    Component={component},
                    Path='{path}',
                    Icon={icon},
                    Parentid={pid},
                    Enabled='{enabled}',
                    NoCache='{nocache}',
                    Ord ={ord} where id = {id}";
                    //Conntext.Connection.Execute(sql);
                    _db.Ado.ExecuteCommand(sql);
                }
            }

            //更新权限
            //RoleRepository.UpdRoles(updRoleList, ref reflag, ref restr);
            if (updRoleList!=null && updRoleList.Count > 0)
            {
                foreach (var rlsititem in updRoleList)
                {
                    string Dependency = string.IsNullOrWhiteSpace(rlsititem.Dependency) ? "null" : "'" + rlsititem.Dependency + "'";
                    //更新权限
                    sql = sql = string.Format($@"
                        update Roles set name='{rlsititem.Name}'
                        ,displayname='{rlsititem.DisplayName}'
                        ,dependency={Dependency}
                        ,menuid={rlsititem.MenuId}
                        ,ord ={rlsititem.Ord}
                        where id={rlsititem.Id}");
                    //Conntext.Connection.Execute(sql);
                    _db.Ado.ExecuteCommand(sql);
                }
            }

            //新增权限
            foreach (Role role in addRoleList)
            {
                try
                {
                    string Dependency = string.IsNullOrWhiteSpace(role.Dependency) ? "null" : "'" + role.Dependency + "'";
                    sql = string.Format($@"INSERT INTO ROLES(ID,NAME,DISPLAYNAME,DEPENDENCY,MENUID,ORD) 
                    VALUES({role.Id},'{role.Name}','{role.DisplayName}',{Dependency},'{role.MenuId}',{role.Ord})");
                    //var succ = RoleRepository.Context.ExecuteSqlCommand(sql);
                    var succ = _db.Ado.ExecuteCommand(sql);
                }
                catch (Exception e)
                {
                    reflag = false;
                    restr = sql;
                    _db.Ado.RollbackTran();
                    return new { success = false, message = sql };
                }
            }
            #endregion

            if (!reflag)
                return new { success = false, message = restr };
            else
            {
                //MenuService.Commit();
                _db.Ado.CommitTran();
                return new { success = true, data = 1 };
            }
        }
        catch (Exception e)
        {
            //MenuService.Rollback();
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "menu_add")]
    [DisplayName("")]
    [Route("/api/Menu/Add")]
    public dynamic Add(Menu menu)
    {
        try
        {
            //int result = MenuService.Add(menu);
            //MenuService.Commit();
            int result = _db.Insertable(menu).ExecuteCommand();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 更改
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    //[HttpPost]
    //[Authorize(Roles = "menu_edit")]
    //[DisplayName("")]
    //[Route("/api/Menu/Update")]
    //public dynamic Update(Menu menu)
    //{
    //    try
    //    {
    //        //MenuService.Update(menu);
    //        //MenuService.Commit();

    //        MENUITEM model = _db.Queryable<MENUITEM>().Where(x => x.ID == menu.Id).Single();
    //        if (model == null)
    //            return new { success = false, message = "没有找到需要修改的数据" };
    //        model.ID = menu.Id;
    //        model.ICON = menu.Icon;
    //        model.COMPONENT = menu.Component;
    //        model.NAME = menu.Name;
    //        model.ENABLED = menu.Enabled;
    //        model.NOCACHE = menu.NoCache;
    //        model.ORD = menu.Ord;
    //        model.PARENTID = menu.Parentid;
    //        model.PATH = menu.Path;
    //        model.ROLES = menu.Roles;
    //        model.Status = menu.Status;
    //        model.TITLE = menu.Title;
    //        //return new { success = true, data = 1 };
    //        int result = _db.Updateable(model).ExecuteCommand();
    //        if (result > 0)
    //            return new { success = true, data = 1 };
    //        return new { success = false, message = "修改失败，请重试" };
    //    }
    //    catch (Exception e)
    //    {
    //        return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
    //    }
    //}

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    //[HttpPost]
    //[Authorize(Roles = "menu_delete")]
    //[DisplayName("")]
    //[Route("/api/Menu/Delete")]
    //public dynamic Delete(Menu menu)
    //{
    //    try
    //    {
    //        //MenuService.Remove(menu);
    //        //MenuService.Commit();
    //        int result = _db.Deleteable<MENUITEM>().Where(x => x.ID == menu.Id).ExecuteCommand();
    //        if (result > 0)
    //            return new { success = true, data = 1 };
    //        return new { success = false, message = "删除失败，请重试" };
    //    }
    //    catch (Exception e)
    //    {
    //        return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
    //    }
    //}

    private IEnumerable<Menu> GetAllMenuTreePrivate(List<Role> roles=null)
    {
        IEnumerable<Menu> menus = _db.Queryable<Menu>().ToList();
        //获取子菜单函数
        Action<Menu> getSubmenus = null;
        getSubmenus = menu =>
        {
            menu.SubMenus = menus.Where(p => p.Parentid == menu.Id).OrderBy(p => p.Ord).ToList();
            if (null != roles)
            {
                menu.MenuRoles = roles.Where(p => p.MenuId == menu.Id).OrderBy(p => p.Ord).ToList();
            }
            foreach (var subMenu in menu.SubMenus)
            {
                getSubmenus(subMenu);
            }
        };
        IEnumerable<Menu> result = menus.Where(p => p.Parentid.GetValueOrDefault(0) == 0).OrderBy(p => p.Ord).ToList();
        foreach (var menu in result)
        {
            getSubmenus(menu);
        }
        return result;
    }
}
