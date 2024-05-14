// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Furion.InstantMessaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Hub.Bdgl;
[MapHub("BdshHub")]
public class BdshHub : Hub<IBdffHub>
{
    // 保存连接的客户端id集合
    public static HashSet<string> Users = new HashSet<string>();
    private readonly IHubContext<BdshHub, IBdffHub> _bdffHubContext;

    public BdshHub(IHubContext<BdshHub, IBdffHub> bdffHubContext)
    {
        _bdffHubContext= bdffHubContext;
    }
    /// <summary>
    /// 向所有连接的客户端发送信息
    /// </summary>
    /// <param name="message"></param>
    [HubMethodName("send")]
    public void Send()
    {
        _bdffHubContext.Clients.All.QueryBdsh();
    }

    /// <summary>
    /// 向固定连接的 客户端发送信息
    /// </summary>
    /// <param name="toUserId"></param>
    /// <param name="message"></param>
    [HubMethodName("sendOne")]
    public void Send(string clientId, string message)
    {
        // 得到当前访问的客户端，并调用updateUserName方法
        _bdffHubContext.Clients.Client(clientId).UpdateUserName(clientId);
    }

    /// <summary>
    /// 当客户端连接时调用
    /// </summary>
    /// <returns></returns>
    public override Task OnConnectedAsync()
    {
        string clientId = GetClientId();
        Users.Add(clientId);
        return base.OnConnectedAsync();
    }

    /// <summary>
    /// 当客户端断开连接时调用，只有当客户端整个关闭的时候才会调用，注销不能
    /// </summary>
    /// <param name="stopCalled"></param>
    /// <returns></returns>
    public override Task OnDisconnectedAsync(Exception ex)
    {
        string clientId = GetClientId();
        Users.Remove(clientId);
        return base.OnDisconnectedAsync(ex);
    }
    // 得到客户端id
    private string GetClientId()
    {
        string clientId = "";
        // clientId passed from application 
        if (Context.GetHttpContext().Request.Query["clientId"].Any())
        {
            clientId = Context.GetHttpContext().Request.Query["clientId"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(clientId))
        {
            clientId = Context.ConnectionId;
        }
        return clientId;
    }
}
