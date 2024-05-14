// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using ICSharpCode.SharpZipLib.Zip.Compression;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;
public static class StringSecurity
{
    public static string secretKey = "BeiJing@2023.com";
    public static string FilterSql(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        s = s.Trim();//.ToLower();
        s = s.Replace("=", "");
        // s = s.Replace("'", "");
        // s = s.Replace(";", "");
        s = s.Replace(" or ", "");
        s = s.Replace("select", "");
        s = s.Replace("update", "");
        s = s.Replace("insert", "");
        s = s.Replace("delete", "");
        s = s.Replace("declare", "");
        s = s.Replace("exec", "");
        s = s.Replace("drop", "");
        s = s.Replace("create", "");
        s = s.Replace("%", "");
        s = s.Replace("--", "");
        return s;
    }
    /// <summary>
    /// 返回的字符串格式：json对象，包含 "pageNum": 1,，  "totalSize": 655,"totalPage": 1,  "content": [、、、]
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string UnZipByteToJson(string value)
    {
        string orgStr = "";
        byte[] bytes = null;

        if (value.Equals(string.Empty) && value.Length < 1)
        {
            return null;
        }
        else
        {
            if (IsBase64Formatted(value))
            {
                return ZipByteString(value, out orgStr, out bytes);

            }
            else
            {
                orgStr = value;
            }
        }

        return orgStr;
    }

    private static string ZipByteString(string value, out string orgStr, out byte[] bytes)
    {
        byte[] data = Convert.FromBase64String(value);
        Inflater f = new Inflater();
        f.SetInput(data);
        int count = 0;
        using (MemoryStream o = new MemoryStream(data.Length))
        {
            byte[] buffer = new byte[2048];
            while (!f.IsFinished)
            {
                count = f.Inflate(buffer);
                o.Write(buffer, 0, count);
            }
            bytes = o.ToArray();
            orgStr = Encoding.GetEncoding(65001).GetString(bytes);
            return orgStr;
        }
    }
    private static bool IsBase64Formatted(string input)
    {
        try
        {
            Convert.FromBase64String(input);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// SM4加密
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string Sm4Encrypt(string Text)
    {
        if (!string.IsNullOrEmpty(Text))
        {
            Sm4Helper sm4 = new Sm4Helper();
            return sm4.Encrypt_ECB(secretKey, false, Text);
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// SM4解密
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string Sm4Decrypt(string Text)
    {
        if (!string.IsNullOrEmpty(Text))
        {
            Sm4Helper sm4 = new Sm4Helper();
            return sm4.Decrypt_ECB(secretKey, false, Text);
        }
        else
        {
            return "";
        }
    }
}
