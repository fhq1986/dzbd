// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Microsoft.OpenApi.Models;
using Nest;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;
public class RestConnection : IRestConnection
{
    private RestClient client;
    public RestConnection(string baseUrl)
    {
        client = new RestClient(baseUrl);
        //client.CookieContainer = new CookieContainer();
    }
    public bool Login(string url, string usernameKey, string username, string passwordKey, string password, out string msg)
    {
        msg = "";
        var login = new RestRequest(url, Method.Post);
        login.AddHeader("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
        string encodedBody = string.Format("{0}={1}&{2}={3}&grant_type={4}", usernameKey, username, passwordKey, password, passwordKey);
        login.AddParameter("application/x-www-form-urlencoded", encodedBody, ParameterType.RequestBody);
        //client.Execute<AuthToken>(request);
        RestResponse loginResponse = client.Execute(login);
        if (loginResponse.StatusCode != HttpStatusCode.OK)
        {
            msg = loginResponse.ErrorMessage;
            if (string.IsNullOrEmpty(msg))
            {
                msg = loginResponse.StatusDescription;
            }
            return false;
        }
        /*
        CookieContainer cookieContainer = new CookieContainer();
        foreach (var cookie in loginResponse.Cookies)
        {
            cookieContainer.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
        }
        client.CookieContainer = cookieContainer;
        */
        JObject token = JObject.Parse(loginResponse.Content);
        client.AddDefaultHeader("Authorization", token.Value<string>("token_type") + " " + token.Value<string>("access_token"));
        return true;
    }

    public bool Login(string url, string usernameKey, string username, string passwordKey, string password, out string msg, out string code, out string token)
    {
        msg = "";
        code = "";
        token = "";
        try
        {
            var login = new RestRequest(url, Method.Post);
            login.AlwaysMultipartFormData = true;
            login.AddParameter(usernameKey, username);
            login.AddParameter(passwordKey, password);

            RestResponse loginResponse = client.Execute(login);
            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            else if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                JObject result = JObject.Parse(loginResponse.Content);
                code = result.Value<string>("code");
                msg = result.Value<string>("msg");
                if (code == "200")  //访问成功
                {
                    JObject data = result.Value<JObject>("data");
                    token = data.Value<string>("token");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            return false;
        }
        return false;
    }

    public bool Logout(string url)
    {
        var logoff = new RestRequest(url, Method.Post);
        var logoffResponse = client.Execute(logoff);
        if (logoffResponse.StatusCode != HttpStatusCode.OK) return false;
        return true;
    }
    public JObject Get(string url, Dictionary<string, string> parameters)
    {
        JObject result = null;
        var request = new RestRequest(url, Method.Get);
        if (null != parameters)
        {
            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
        }
        var response = client.Execute(request);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw (new Exception(response.StatusDescription));
        }
        try
        {
            result = JObject.Parse(response.Content);
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }
    public JArray GetJarray(string url, Dictionary<string, string> parameters)
    {
        JArray result = null;
        var request = new RestRequest(url, Method.Get);
        if (null != parameters)
        {
            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
        }
        var response = client.Execute(request);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw (new Exception(response.StatusDescription));
        }
        try
        {
            result = JArray.Parse(response.Content);
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }
    public JObject Post(string url, object body)
    {
        JObject result = null;
        var request = new RestRequest(url, Method.Post);
        //request.RequestFormat = DataFormat.Json;
        //request.AddBody(body);
        request.AddHeader("Accept", "application/json");
        //JsonSerializerSettings settings = new JsonSerializerSettings();
        //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        string sBody = JsonConvert.SerializeObject(body, Formatting.Indented,
        new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        });
        request.AddParameter("application/json", sBody, ParameterType.RequestBody);
        var response = client.Execute(request);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw (new Exception(response.StatusDescription));
        }
        try
        {
            result = JObject.Parse(response.Content);
            if (!result.Value<bool>("success"))
            {
                throw new Exception(result.Value<string>("message"));
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }


    public JObject Post(string url, object body, string token)
    {
        JObject result = null;
        try
        {
            //var client = new RestClient(url);
            //client.Timeout = -1;
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (null != response.StatusDescription)
                    throw (new Exception(response.StatusDescription));
                else
                    throw (new Exception(response.ErrorMessage));
            }

            result = JObject.Parse(response.Content);
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }
    public dynamic PostJson(string url, object body)
    {
        try
        {
            //client.Timeout = -1;
            var request = new RestRequest(url, Method.Post);
            request.AddParameter("application/json", body, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (null != response.StatusDescription)
                    throw (new Exception(response.StatusDescription));
                else
                    throw (new Exception(response.ErrorMessage));
            }

            return response.Content;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public Collection<T> GetPageValues<T>(string url, int page, int start, int limit, Dictionary<string, string> parameters, ref int total)
    {
        Collection<T> result = null;
        if (null == parameters)
        {
            parameters = new Dictionary<string, string>();
        }
        parameters.Add("page", page.ToString());
        parameters.Add("start", start.ToString());
        parameters.Add("limit", limit.ToString());
        JObject jo = Get(url, parameters);
        if (jo.Value<bool>("success"))
        {
            total = jo.Value<int>("total");
            JArray ja = jo.Value<JArray>("data");
            if (null != ja)
            {
                result = ja.ToObject<Collection<T>>();
            }
        }
        else
        {
            throw (new Exception(jo.Value<string>("message")));
        }
        return result;
    }

    public Collection<T> GetValues<T>(string url, Dictionary<string, string> parameters, ref int total)
    {
        Collection<T> result = null;
        if (null == parameters)
        {
            parameters = new Dictionary<string, string>();
        }
        JObject jo = Get(url, parameters);
        if (jo.Value<bool>("success"))
        {
            total = jo.Value<int>("total");
            JArray ja = jo.Value<JArray>("data");
            if (null == ja)
            {
                ja = jo.Value<JArray>("items");
            }
            if (null != ja)
            {
                result = ja.ToObject<Collection<T>>();
            }
        }
        else
        {
            throw (new Exception(jo.Value<string>("message")));
        }
        return result;
    }
}
