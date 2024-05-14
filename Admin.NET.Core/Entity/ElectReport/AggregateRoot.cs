// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Entity.ElectReport;
/// <summary>
/// 表示聚合根类型的基类型。
/// </summary>
public abstract class AggregateRoot<TKey> : IAggregateRoot<TKey>
{
    #region Protected Fields
    protected TKey id;
    #endregion

    #region Public Methods
    /// <summary>
    /// 确定指定的Object是否等于当前的Object。
    /// </summary>
    /// <param name="obj">要与当前对象进行比较的对象。</param>
    /// <returns>如果指定的Object与当前Object相等，则返回true，否则返回false。</returns>
    /// <remarks>有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.equals。
    /// </remarks>
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        IAggregateRoot<TKey> ar = obj as IAggregateRoot<TKey>;
        if (ar == null)
            return false;
        return this.id.Equals(ar.Id);
    }
    /// <summary>
    /// 用作特定类型的哈希函数。
    /// </summary>
    /// <returns>当前Object的哈希代码。</returns>
    /// <remarks>有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.gethashcode。
    /// </remarks>
    public override int GetHashCode()
    {
        return this.id.GetHashCode();
    }
    #endregion

    #region IEntity Members
    /// <summary>
    /// 获取当前领域实体类的全局唯一标识。
    /// </summary>
    //[Column("ID")]
    //[Key]
    public TKey Id
    {
        get { return id; }
        set { id = value; }
    }

    #endregion
}
