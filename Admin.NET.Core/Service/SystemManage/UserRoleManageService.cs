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
/// 用户角色管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class UserRoleManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public UserRoleManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "userrole_base")]
    [DisplayName("获取所有角色")]
    [Route("/api/UserRole/FindAllRoles")]
    public dynamic FindAllRoles()
    {
        try
        {
            //IList<UserRole> defaultChecked = UserRoleService.FindAllUserRole();
            IList<UserRole> defaultChecked = _db.Queryable<UserRole>().ToList();
            //var trees = UserRoleService.FindTree();
            #region MyRegion
            IList<TreeNode> rootTrees = new List<TreeNode>();//跟节点
            //IList<Menu> menus = MenuService.FindAll().OrderBy(p => p.Ord).ToList();
            IList<Menu> menus=_db.Queryable<Menu>().OrderBy(x=>x.Ord).ToList();
            // 给菜单id加上1000使菜单id和角色id不冲突
            foreach (Menu menu in menus)
            {
                menu.Id = menu.Id + 1000;
                if (menu.Parentid != null)
                {
                    menu.Parentid += 1000;
                }
            }
            //IList<Role> roles = RoleService.FindAll().OrderBy(p => p.MenuId).OrderBy(p => p.Ord).ToList();
            IList<Role> roles = _db.Queryable<Role>().OrderBy(x => x.MenuId).OrderBy(x => x.Ord).ToList();
            IList<Menu> parentmenus = menus.Where(p => p.Parentid == null).ToList();
            foreach (Menu menu in parentmenus)
            {
                TreeNode rootTree = new TreeNode() { Id = menu.Id, NodeCode = menu.Name, NodeName = menu.Title };
                IList<TreeNode> childtrees = Recursion(rootTree, menus, roles);
                rootTree.Children = childtrees;
                rootTrees.Add(rootTree);
            }
            #endregion

            return new { success = true, data = new { items = rootTrees, defaultChecked = defaultChecked } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 添加用户权限
    /// </summary>
    /// <param name="users"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "userrole_add")]
    [DisplayName("添加用户权限")]
    [Route("/api/UserRole/Add")]
    public dynamic Add(string users, string roles)
    {
        try
        {
            //int result = UserRoleService.Add(users, roles);
            //UserService.Commit();
            _db.Ado.BeginTran();
            #region result = UserRoleService.Add(users, roles);
            var userjas = JArray.Parse(users).Values().ToList();
            var rolejas = JArray.Parse(roles).Values().ToList();
            int result = 0;
            //IList<Role> judgeRoles = RoleService.FindAll().ToList();
            IList<Role> judgeRoles = _db.Queryable<Role>().ToList();
            foreach (JToken userja in userjas)
            {
                int userid = int.Parse(userja.ToString());
                //string delsql = string.Format("delete from {0} where userid = '{1}'", "userroles", userid);
                ////先删
                //UserRoleRepository.Context.ExecuteSqlCommand(delsql);
                _db.Deleteable<UserRole>().Where(x => x.UserId == userid).ExecuteCommand();
                foreach (JToken roleja in rolejas)
                {
                    int roleid = int.Parse(roleja.ToString());
                    //用于判断点击的角色id是否在与角色表中
                    //string insql = string.Format(@"insert into {0} values('{1}','{2}')", "userroles", userid, roleid);
                    //if (judgeRoles.Where(p => p.Id == roleid).Count() > 0) result = UserRoleRepository.Context.ExecuteSqlCommand(insql);
                    if (judgeRoles.Where(p => p.Id == roleid).Count() > 0)
                        result = _db.Insertable(new UserRole()
                                {
                                    RoleId = roleid,
                                    UserId = userid
                                }).ExecuteCommand();
                }
            }
            #endregion
            _db.Ado.CommitTran();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    // 递归求树
    private IList<TreeNode> Recursion(TreeNode parentTree, IList<Menu> menus, IList<Role> roles)
    {
        IList<TreeNode> childtrees = new List<TreeNode>();
        var childmenus = menus.Where(p => p.Parentid == parentTree.Id);
        if (childmenus == null || childmenus.Count() == 0) return childtrees;
        foreach (Menu childmenu in childmenus)
        {
            TreeNode childTree = new TreeNode() { Id = childmenu.Id, NodeCode = childmenu.Name, NodeName = childmenu.Title, Father = new TreeNode() { Id = parentTree.Id, NodeCode = parentTree.NodeCode, NodeName = parentTree.NodeName } };

            // 在最底层添加role,
            if (menus.Where(p => p.Parentid == childTree.Id).Count() == 0)
            {
                IList<TreeNode> childRoles = new List<TreeNode>();
                foreach (Role role in roles)
                {
                    if ((role.MenuId + 1000) == childTree.Id)
                    {
                        childRoles.Add(new TreeNode() { Id = role.Id, NodeCode = role.Name, NodeName = role.DisplayName, Father = new TreeNode() { Id = childTree.Id, NodeCode = childTree.NodeCode, NodeName = childTree.NodeName } });
                    }
                }
                childTree.Children = childRoles;
            }
            else
            {
                IList<TreeNode> recTrees = Recursion(childTree, menus, roles);
                childTree.Children = recTrees;
            }

            childtrees.Add(childTree);
        }
        return childtrees;
    }
}
