// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using AngleSharp.Dom;

namespace Admin.NET.Core;

/// <summary>
/// 系统用户表
/// </summary>
[SugarTable("USERS", "系统用户表")]
[SysTable]
public class SysUserInfo
{
    /// <summary>
    /// User ID
    /// </summary>
    public int Id { get; set; }

    public string UserName { get; set; }

    /// <summary>
    /// User's name
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    ///     Email
    /// </summary>
    public virtual string Email { get; set; }

    /// <summary>
    ///     True if the email is confirmed, default is false
    /// </summary>
    public virtual bool EmailConfirmed { get; set; }

    /// <summary>
    ///     The salted/hashed form of the user password
    /// </summary>
    public virtual string PasswordHash { get; set; }

    /// <summary>
    ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
    /// </summary>
    public virtual string SecurityStamp { get; set; }

    /// <summary>
    ///     PhoneNumber for the user
    /// </summary>
    public virtual string PhoneNumber { get; set; }

    /// <summary>
    ///     True if the phone number is confirmed, default is false
    /// </summary>
    public virtual bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    ///     Is two factor enabled for the user
    /// </summary>
    public virtual bool TwoFactorEnabled { get; set; }

    /// <summary>
    ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
    /// </summary>
    public virtual DateTime? LockoutEndDateUtc { get; set; }

    /// <summary>
    ///     Is lockout enabled for this user
    /// </summary>
    public virtual bool LockoutEnabled { get; set; }

    /// <summary>
    ///     Used to record failures for the purposes of lockout
    /// </summary>
    public virtual int AccessFailedCount { get; set; }

    public virtual string Tljdm { get; set; }

    public virtual string Jwddm { get; set; }

    public virtual string Cjdm { get; set; }

    public virtual string Type { get; set; }
    public virtual string QTDM { get; set; }
}