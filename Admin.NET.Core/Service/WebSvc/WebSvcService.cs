﻿// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.DapperEx;
using DotNetCore.CAP.Dashboard.GatewayProxy.Requester;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaBusinessPerformanceBootResponse.Types.Data.Types;

namespace Admin.NET.Core.Service.WebSvc;

[AllowAnonymous]
[ApiDescriptionSettings(Order = 1)]

public class WebTestService : IDynamicApiController,ITransient
{
    private readonly IServiceScopeFactory _scopeFactory;
    public WebTestService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    /// <summary>
    /// 测试Web服务
    /// </summary>
    /// <returns></returns>
    public async Task<string> TestWebCall()
    {
        WebService1SoapClient client = new WebService1SoapClient(WebService1SoapClient.EndpointConfiguration.WebService1Soap);
        var result = await client.GetQsyt1ByCzCcAsync("11","22","33","44","55");
        return result.Body.GetQsyt1ByCzCcResult;
    }
}
