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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;

public class MD5Util
{
    /// <summary>
    /// 字符串MD5加密
    /// </summary>
    /// <param name="rawPass"></param>
    /// <returns></returns>
    private static string MD5Encoding(string rawPass)
    {
        MD5 md5 = MD5.Create();
        byte[] bs = Encoding.UTF8.GetBytes(rawPass);
        byte[] ms = md5.ComputeHash(bs);
        StringBuilder md5Pass = new StringBuilder();
        foreach (byte m in ms)
        {
            md5Pass.Append(m.ToString("x2"));
        }
        return md5Pass.ToString();
    }

    /// <summary>
    /// MD5加盐加密
    /// </summary>
    /// <param name="password">密码源字符串</param>
    /// <param name="salt">盐值</param>
    /// <returns>加密后字符串</returns>
    public static string MD5Encoding(string password, object salt)
    {
        if (salt != null) password += salt.ToString();
        return MD5Encoding(password);
    }
}
