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

namespace Admin.NET.Core.WebService.KctjWeb;
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace = "http://smallnest.googlepages.com/FeedBackService", ConfigurationName = "FeedBackServicePortType")]
public interface FeedBackServicePortType
{

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getZdcclbAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getKydDdccAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getLcskbAsync(string in0, string in1, string in2, string in3);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getKybzByLcccAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getDczAsync(string in0);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getClxxValidateAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getZdbzAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getFeedBackAsync(string in0, string in1, string in2);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getKydKpsjAsync(string in0, string in1, string in2);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getReportErrorsAsync(string in0, string in1, string in2, string in3);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getCljbxxAsync(string in0);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getKybzAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> checkKydGetKpsjAsync(string in0, string in1);

    [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
    [return: System.ServiceModel.MessageParameterAttribute(Name = "out")]
    System.Threading.Tasks.Task<string> getSfcclbAsync(string in0, string in1);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface FeedBackServicePortTypeChannel : FeedBackServicePortType, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public partial class FeedBackServicePortTypeClient : System.ServiceModel.ClientBase<FeedBackServicePortType>, FeedBackServicePortType
{

    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

    public FeedBackServicePortTypeClient() :
            base(FeedBackServicePortTypeClient.GetDefaultBinding(), FeedBackServicePortTypeClient.GetDefaultEndpointAddress())
    {
        this.Endpoint.Name = EndpointConfiguration.FeedBackServiceHttpPort.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public FeedBackServicePortTypeClient(EndpointConfiguration endpointConfiguration) :
            base(FeedBackServicePortTypeClient.GetBindingForEndpoint(endpointConfiguration), FeedBackServicePortTypeClient.GetEndpointAddress(endpointConfiguration))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public FeedBackServicePortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
            base(FeedBackServicePortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public FeedBackServicePortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
            base(FeedBackServicePortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public FeedBackServicePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
    {
    }

    public System.Threading.Tasks.Task<string> getZdcclbAsync(string in0, string in1)
    {
        return base.Channel.getZdcclbAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> getKydDdccAsync(string in0, string in1)
    {
        return base.Channel.getKydDdccAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> getLcskbAsync(string in0, string in1, string in2, string in3)
    {
        return base.Channel.getLcskbAsync(in0, in1, in2, in3);
    }

    public System.Threading.Tasks.Task<string> getKybzByLcccAsync(string in0, string in1)
    {
        return base.Channel.getKybzByLcccAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> getDczAsync(string in0)
    {
        return base.Channel.getDczAsync(in0);
    }

    public System.Threading.Tasks.Task<string> getClxxValidateAsync(string in0, string in1)
    {
        return base.Channel.getClxxValidateAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> getZdbzAsync(string in0, string in1)
    {
        return base.Channel.getZdbzAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> getFeedBackAsync(string in0, string in1, string in2)
    {
        return base.Channel.getFeedBackAsync(in0, in1, in2);
    }

    public System.Threading.Tasks.Task<string> getKydKpsjAsync(string in0, string in1, string in2)
    {
        return base.Channel.getKydKpsjAsync(in0, in1, in2);
    }

    public System.Threading.Tasks.Task<string> getReportErrorsAsync(string in0, string in1, string in2, string in3)
    {
        return base.Channel.getReportErrorsAsync(in0, in1, in2, in3);
    }

    public System.Threading.Tasks.Task<string> getCljbxxAsync(string in0)
    {
        return base.Channel.getCljbxxAsync(in0);
    }

    public System.Threading.Tasks.Task<string> getKybzAsync(string in0, string in1)
    {
        return base.Channel.getKybzAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> checkKydGetKpsjAsync(string in0, string in1)
    {
        return base.Channel.checkKydGetKpsjAsync(in0, in1);
    }

    public System.Threading.Tasks.Task<string> getSfcclbAsync(string in0, string in1)
    {
        return base.Channel.getSfcclbAsync(in0, in1);
    }

    public virtual System.Threading.Tasks.Task OpenAsync()
    {
        return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
    }

    private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.FeedBackServiceHttpPort))
        {
            System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            return result;
        }
        throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
    }

    private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.FeedBackServiceHttpPort))
        {
            return new System.ServiceModel.EndpointAddress("http://10.3.102.113/kctj/services/FeedBackService");
        }
        throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
    }

    private static System.ServiceModel.Channels.Binding GetDefaultBinding()
    {
        return FeedBackServicePortTypeClient.GetBindingForEndpoint(EndpointConfiguration.FeedBackServiceHttpPort);
    }

    private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
    {
        return FeedBackServicePortTypeClient.GetEndpointAddress(EndpointConfiguration.FeedBackServiceHttpPort);
    }

    public enum EndpointConfiguration
    {

        FeedBackServiceHttpPort,
    }
}

