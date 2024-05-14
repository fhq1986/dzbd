// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Org.BouncyCastle.Utilities.Encoders;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Admin.NET.Core;

public class CryptogramUtil
{
    public static readonly string CryptoType = App.GetConfig<string>("Cryptogram:CryptoType"); // 加密类型
    public static readonly string PublicKey = App.GetConfig<string>("Cryptogram:PublicKey"); // 公钥
    public static readonly string PrivateKey = App.GetConfig<string>("Cryptogram:PrivateKey"); // 私钥
    public const string AES_IV = "1234567890000000";
    public static readonly string key = "12345678900000001234567890000000";
    private const int PBKDF2IterCount = 1000;
    private const int PBKDF2SubkeyLength = 32;
    private const int SaltSize = 16;
    public static byte[] HexStringToByteArray(string s)
    {
        s = s.Replace(" ", "");
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
            buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
        return buffer;

    }
    public static string ByteArrayToHexString(byte[] data)
    {
        StringBuilder sb = new StringBuilder(data.Length * 3);
        foreach (byte b in data)
        {
            //16进制数字
            sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            //16进制数字之间以空格隔开
            //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
        }
        return sb.ToString().ToUpper();
    }
    public static string EncryptByAES(string input)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(input);
                    }
                    byte[] bytes = msEncrypt.ToArray();
                    return ByteArrayToHexString(bytes);
                }
            }
        }
    }
    public static string DecryptByAES(string input)
    {
        byte[] inputBytes = HexStringToByteArray(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
        using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                    {
                        return srEncrypt.ReadToEnd();
                    }
                }
            }
        }
    }
    public static string HashPassword(string password)
    {
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        byte[] salt;
        byte[] bytes;
        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
        {
            salt = rfc2898DeriveBytes.Salt;
            bytes = rfc2898DeriveBytes.GetBytes(32);
        }
        byte[] array = new byte[49];
        Buffer.BlockCopy(salt, 0, array, 1, 16);
        Buffer.BlockCopy(bytes, 0, array, 17, 32);
        return Convert.ToBase64String(array);
    }
    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
        if (hashedPassword == null)
        {
            return false;
        }
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        byte[] array = Convert.FromBase64String(hashedPassword);
        if (array.Length != 49 || array[0] != 0)
        {
            return false;
        }
        byte[] array2 = new byte[16];
        Buffer.BlockCopy(array, 1, array2, 0, 16);
        byte[] array3 = new byte[32];
        Buffer.BlockCopy(array, 17, array3, 0, 32);
        byte[] bytes;
        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, 1000))
        {
            bytes = rfc2898DeriveBytes.GetBytes(32);
        }
        return ByteArraysEqual(array3, bytes);
    }
    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (object.ReferenceEquals(a, b))
        {
            return true;
        }
        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }
        bool flag = true;
        for (int i = 0; i < a.Length; i++)
        {
            flag &= (a[i] == b[i]);
        }
        return flag;
    }
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Encrypt(string plainText)
    {
        if (CryptoType == CryptogramEnum.MD5.ToString())
        {
            return MD5Encryption.Encrypt(plainText);
        }
        else if (CryptoType == CryptogramEnum.SM2.ToString())
        {
            return SM2Encrypt(plainText);
        }
        else if (CryptoType == CryptogramEnum.SM4.ToString())
        {
            return SM4EncryptECB(plainText);
        }
        return plainText;
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string Decrypt(string cipherText)
    {
        if (CryptoType == CryptogramEnum.SM2.ToString())
        {
            return SM2Decrypt(cipherText);
        }
        else if (CryptoType == CryptogramEnum.SM4.ToString())
        {
            return SM4DecryptECB(cipherText);
        }
        return cipherText;
    }

    /// <summary>
    /// SM2加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM2Encrypt(string plainText)
    {
        byte[] sourceData = Encoding.Default.GetBytes(plainText);
        return SM2Util.Encrypt(Hex.Decode(PublicKey), sourceData);
    }

    /// <summary>
    /// SM2解密
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM2Decrypt(string cipherText)
    {
        return Encoding.Default.GetString(SM2Util.Decrypt(Hex.Decode(PrivateKey), Hex.Decode(cipherText)));
    }

    /// <summary>
    /// SM4加密（ECB）
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptECB(string plainText)
    {
        var sm4 = new SM4Util();
        return sm4.Encrypt_ECB(plainText);
    }

    /// <summary>
    /// SM4解密（ECB）
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptECB(string cipherText)
    {
        var sm4 = new SM4Util();
        return sm4.Decrypt_ECB(cipherText);
    }

    /// <summary>
    /// SM4加密（CBC）
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptCBC(string plainText)
    {
        var sm4 = new SM4Util();
        return sm4.Encrypt_CBC(plainText);
    }

    /// <summary>
    /// SM4解密（CBC）
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptCBC(string cipherText)
    {
        var sm4 = new SM4Util();
        return sm4.Decrypt_CBC(cipherText);
    }
}