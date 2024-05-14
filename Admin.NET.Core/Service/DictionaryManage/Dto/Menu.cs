// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage.Dto;
public class Menu: AggregateRoot<int>
{
    /// <summary>
    /// 主题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 模块
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public string Roles { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int? Ord { get; set; }

    /// <summary>
    /// 父类id
    /// </summary>
    public int? Parentid { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 页面是否缓存
    /// </summary>
    public string NoCache { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public IEnumerable<Menu> SubMenus { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public IEnumerable<Role> MenuRoles { get; set; }

    /// <summary>
    /// 数据状态
    /// </summary>
    public string Status { get; set; }
}
