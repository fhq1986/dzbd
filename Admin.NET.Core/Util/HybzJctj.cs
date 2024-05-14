// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.WebService.HybzJctjWs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;
public class HybzJctj
{
    public static async Task<JObject> GetQsyt1ByBwm(string zpddm, string dbm)
    {
        JObject jsQsyt1 = null;
        try
        {
            HybzJctjWsPortTypeClient client = new HybzJctjWsPortTypeClient(HybzJctjWsPortTypeClient.EndpointConfiguration.HybzJctjWsHttpSoap11Endpoint);
            var webServiceResult = await client.GetQsyt1ByBwmAsync(zpddm, dbm, "tdb");
            if (webServiceResult != null && !string.IsNullOrWhiteSpace(webServiceResult.@return))
            {
                jsQsyt1 = JObject.Parse(webServiceResult.@return.Trim());
            }
        }
        catch (System.Exception)
        {

            throw;
        }
        return jsQsyt1;
    }
    public static async Task<JArray> GetQsyt1BtByCzCc(string zpddm, string dblm, string cc, string cf, int nSjfd)
    {
        try
        {
            HybzJctjWsPortTypeClient client = new HybzJctjWsPortTypeClient(HybzJctjWsPortTypeClient.EndpointConfiguration.HybzJctjWsHttpSoap11Endpoint);
            JArray jsQsyt1 = null;
            var webServiceResult = await client.GetQsyt1ByCzCcAsync(zpddm, dblm, cc, cf, nSjfd);
            if (webServiceResult != null && !string.IsNullOrWhiteSpace(webServiceResult.@return))
            {
                jsQsyt1 = JArray.Parse(webServiceResult.@return.Trim());
                return jsQsyt1;
            }
        }
        catch (System.Exception)
        {

        }
        return null;
    }
}
