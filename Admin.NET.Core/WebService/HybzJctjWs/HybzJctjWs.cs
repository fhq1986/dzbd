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

namespace Admin.NET.Core.WebService.HybzJctjWs;
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://srie.com")]
public partial class Exception1
{

    private Exception exceptionField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
    public Exception Exception
    {
        get
        {
            return this.exceptionField;
        }
        set
        {
            this.exceptionField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://srie.com")]
public partial class Exception
{

    private object exception1Field;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Exception", IsNullable = true, Order = 0)]
    public object Exception1
    {
        get
        {
            return this.exception1Field;
        }
        set
        {
            this.exception1Field = value;
        }
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace = "http://srie.com", ConfigurationName = "HybzJctjWsPortType")]
public interface HybzJctjWsPortType
{

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetJcZxgl", ReplyAction = "urn:GetJcZxglResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetJcZxglException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetJcZxglResponse> GetJcZxglAsync(GetJcZxglRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1ByCzCc", ReplyAction = "urn:GetQsyt1ByCzCcResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1ByCzCcException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1ByCzCcResponse> GetQsyt1ByCzCcAsync(GetQsyt1ByCzCcRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1ByCzZz", ReplyAction = "urn:GetQsyt1ByCzZzResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1ByCzZzException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1ByCzZzResponse> GetQsyt1ByCzZzAsync(GetQsyt1ByCzZzRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1ArrayByCz", ReplyAction = "urn:GetQsyt1ArrayByCzResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1ArrayByCzException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1ArrayByCzResponse> GetQsyt1ArrayByCzAsync(GetQsyt1ArrayByCzRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:PutSjbdUnFound", ReplyAction = "urn:PutSjbdUnFoundResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:PutSjbdUnFoundException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<PutSjbdUnFoundResponse> PutSjbdUnFoundAsync(PutSjbdUnFoundRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:Test", ReplyAction = "urn:TestResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:TestException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<TestResponse> TestAsync(TestRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1ByBwm", ReplyAction = "urn:GetQsyt1ByBwmResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1ByBwmException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1ByBwmResponse> GetQsyt1ByBwmAsync(GetQsyt1ByBwmRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetSjbdUnFound", ReplyAction = "urn:GetSjbdUnFoundResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetSjbdUnFoundException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetSjbdUnFoundResponse> GetSjbdUnFoundAsync(GetSjbdUnFoundRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:PutSjbdUnFoundEx", ReplyAction = "urn:PutSjbdUnFoundExResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:PutSjbdUnFoundExException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<PutSjbdUnFoundExResponse> PutSjbdUnFoundExAsync(PutSjbdUnFoundExRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1BwmByEwm", ReplyAction = "urn:GetQsyt1BwmByEwmResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1BwmByEwmException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1BwmByEwmResponse> GetQsyt1BwmByEwmAsync(GetQsyt1BwmByEwmRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1ArrayByCc", ReplyAction = "urn:GetQsyt1ArrayByCcResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1ArrayByCcException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1ArrayByCcResponse> GetQsyt1ArrayByCcAsync(GetQsyt1ArrayByCcRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:PutSjbdFound", ReplyAction = "urn:PutSjbdFoundResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:PutSjbdFoundException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<PutSjbdFoundResponse> PutSjbdFoundAsync(PutSjbdFoundRequest request);

    [System.ServiceModel.OperationContractAttribute(Action = "urn:GetQsyt1ArrayByCcSjTonsCzList", ReplyAction = "urn:GetQsyt1ArrayByCcSjTonsCzListResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(Exception1), Action = "urn:GetQsyt1ArrayByCcSjTonsCzListException", Name = "Exception")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    System.Threading.Tasks.Task<GetQsyt1ArrayByCcSjTonsCzListResponse> GetQsyt1ArrayByCcSjTonsCzListAsync(GetQsyt1ArrayByCcSjTonsCzListRequest request);
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetJcZxgl", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetJcZxglRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string bdrq;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdcode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jx;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jh;

    public GetJcZxglRequest()
    {
    }

    public GetJcZxglRequest(string bdrq, string jwdcode, string jx, string jh)
    {
        this.bdrq = bdrq;
        this.jwdcode = jwdcode;
        this.jx = jx;
        this.jh = jh;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetJcZxglResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetJcZxglResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetJcZxglResponse()
    {
    }

    public GetJcZxglResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ByCzCc", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ByCzCcRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdCode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string czList;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string cc;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sj;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 4)]
    public int minutes;

    public GetQsyt1ByCzCcRequest()
    {
    }

    public GetQsyt1ByCzCcRequest(string jwdCode, string czList, string cc, string sj, int minutes)
    {
        this.jwdCode = jwdCode;
        this.czList = czList;
        this.cc = cc;
        this.sj = sj;
        this.minutes = minutes;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ByCzCcResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ByCzCcResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1ByCzCcResponse()
    {
    }

    public GetQsyt1ByCzCcResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ByCzZz", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ByCzZzRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdCode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string czList;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    public int zz;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sj;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 4)]
    public int minutes;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 5)]
    public int tons;

    public GetQsyt1ByCzZzRequest()
    {
    }

    public GetQsyt1ByCzZzRequest(string jwdCode, string czList, int zz, string sj, int minutes, int tons)
    {
        this.jwdCode = jwdCode;
        this.czList = czList;
        this.zz = zz;
        this.sj = sj;
        this.minutes = minutes;
        this.tons = tons;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ByCzZzResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ByCzZzResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1ByCzZzResponse()
    {
    }

    public GetQsyt1ByCzZzResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ArrayByCz", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ArrayByCzRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdCode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string cz;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sj;

    public GetQsyt1ArrayByCzRequest()
    {
    }

    public GetQsyt1ArrayByCzRequest(string jwdCode, string cz, string sj)
    {
        this.jwdCode = jwdCode;
        this.cz = cz;
        this.sj = sj;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ArrayByCzResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ArrayByCzResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1ArrayByCzResponse()
    {
    }

    public GetQsyt1ArrayByCzResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "PutSjbdUnFound", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class PutSjbdUnFoundRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string bwInfo;

    public PutSjbdUnFoundRequest()
    {
    }

    public PutSjbdUnFoundRequest(string bwInfo)
    {
        this.bwInfo = bwInfo;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "PutSjbdUnFoundResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class PutSjbdUnFoundResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public PutSjbdUnFoundResponse()
    {
    }

    public PutSjbdUnFoundResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class TestRequest
{

    public TestRequest()
    {
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "TestResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class TestResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public TestResponse()
    {
    }

    public TestResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ByBwm", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ByBwmRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdCode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string bwm;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string bwly;

    public GetQsyt1ByBwmRequest()
    {
    }

    public GetQsyt1ByBwmRequest(string jwdCode, string bwm, string bwly)
    {
        this.jwdCode = jwdCode;
        this.bwm = bwm;
        this.bwly = bwly;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ByBwmResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ByBwmResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1ByBwmResponse()
    {
    }

    public GetQsyt1ByBwmResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetSjbdUnFound", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetSjbdUnFoundRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string czdm;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string qsrq;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string zzrq;

    public GetSjbdUnFoundRequest()
    {
    }

    public GetSjbdUnFoundRequest(string czdm, string qsrq, string zzrq)
    {
        this.czdm = czdm;
        this.qsrq = qsrq;
        this.zzrq = zzrq;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetSjbdUnFoundResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetSjbdUnFoundResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetSjbdUnFoundResponse()
    {
    }

    public GetSjbdUnFoundResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "PutSjbdUnFoundEx", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class PutSjbdUnFoundExRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string bwInfo;

    public PutSjbdUnFoundExRequest()
    {
    }

    public PutSjbdUnFoundExRequest(string bwInfo)
    {
        this.bwInfo = bwInfo;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "PutSjbdUnFoundExResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class PutSjbdUnFoundExResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public PutSjbdUnFoundExResponse()
    {
    }

    public PutSjbdUnFoundExResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1BwmByEwm", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1BwmByEwmRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sEwm;

    public GetQsyt1BwmByEwmRequest()
    {
    }

    public GetQsyt1BwmByEwmRequest(string sEwm)
    {
        this.sEwm = sEwm;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1BwmByEwmResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1BwmByEwmResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1BwmByEwmResponse()
    {
    }

    public GetQsyt1BwmByEwmResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ArrayByCc", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ArrayByCcRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdCode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string cc;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sj;

    public GetQsyt1ArrayByCcRequest()
    {
    }

    public GetQsyt1ArrayByCcRequest(string jwdCode, string cc, string sj)
    {
        this.jwdCode = jwdCode;
        this.cc = cc;
        this.sj = sj;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ArrayByCcResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ArrayByCcResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1ArrayByCcResponse()
    {
    }

    public GetQsyt1ArrayByCcResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "PutSjbdFound", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class PutSjbdFoundRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string bwInfo;

    public PutSjbdFoundRequest()
    {
    }

    public PutSjbdFoundRequest(string bwInfo)
    {
        this.bwInfo = bwInfo;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "PutSjbdFoundResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class PutSjbdFoundResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public PutSjbdFoundResponse()
    {
    }

    public PutSjbdFoundResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ArrayByCcSjTonsCzList", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ArrayByCcSjTonsCzListRequest
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string jwdCode;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 1)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string cc;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 2)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sj_s;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 3)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string sj_e;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 4)]
    public int ton_s;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 5)]
    public int ton_e;

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 6)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string czList;

    public GetQsyt1ArrayByCcSjTonsCzListRequest()
    {
    }

    public GetQsyt1ArrayByCcSjTonsCzListRequest(string jwdCode, string cc, string sj_s, string sj_e, int ton_s, int ton_e, string czList)
    {
        this.jwdCode = jwdCode;
        this.cc = cc;
        this.sj_s = sj_s;
        this.sj_e = sj_e;
        this.ton_s = ton_s;
        this.ton_e = ton_e;
        this.czList = czList;
    }
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
[System.ServiceModel.MessageContractAttribute(WrapperName = "GetQsyt1ArrayByCcSjTonsCzListResponse", WrapperNamespace = "http://srie.com", IsWrapped = true)]
public partial class GetQsyt1ArrayByCcSjTonsCzListResponse
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://srie.com", Order = 0)]
    [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
    public string @return;

    public GetQsyt1ArrayByCcSjTonsCzListResponse()
    {
    }

    public GetQsyt1ArrayByCcSjTonsCzListResponse(string @return)
    {
        this.@return = @return;
    }
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface HybzJctjWsPortTypeChannel : HybzJctjWsPortType, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public partial class HybzJctjWsPortTypeClient : System.ServiceModel.ClientBase<HybzJctjWsPortType>, HybzJctjWsPortType
{

    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

    public HybzJctjWsPortTypeClient(EndpointConfiguration endpointConfiguration) :
            base(HybzJctjWsPortTypeClient.GetBindingForEndpoint(endpointConfiguration), HybzJctjWsPortTypeClient.GetEndpointAddress(endpointConfiguration))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public HybzJctjWsPortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
            base(HybzJctjWsPortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public HybzJctjWsPortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
            base(HybzJctjWsPortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
    {
        this.Endpoint.Name = endpointConfiguration.ToString();
        ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
    }

    public HybzJctjWsPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
    {
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetJcZxglResponse> HybzJctjWsPortType.GetJcZxglAsync(GetJcZxglRequest request)
    {
        return base.Channel.GetJcZxglAsync(request);
    }

    public System.Threading.Tasks.Task<GetJcZxglResponse> GetJcZxglAsync(string bdrq, string jwdcode, string jx, string jh)
    {
        GetJcZxglRequest inValue = new GetJcZxglRequest();
        inValue.bdrq = bdrq;
        inValue.jwdcode = jwdcode;
        inValue.jx = jx;
        inValue.jh = jh;
        return ((HybzJctjWsPortType)(this)).GetJcZxglAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1ByCzCcResponse> HybzJctjWsPortType.GetQsyt1ByCzCcAsync(GetQsyt1ByCzCcRequest request)
    {
        return base.Channel.GetQsyt1ByCzCcAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1ByCzCcResponse> GetQsyt1ByCzCcAsync(string jwdCode, string czList, string cc, string sj, int minutes)
    {
        GetQsyt1ByCzCcRequest inValue = new GetQsyt1ByCzCcRequest();
        inValue.jwdCode = jwdCode;
        inValue.czList = czList;
        inValue.cc = cc;
        inValue.sj = sj;
        inValue.minutes = minutes;
        return ((HybzJctjWsPortType)(this)).GetQsyt1ByCzCcAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1ByCzZzResponse> HybzJctjWsPortType.GetQsyt1ByCzZzAsync(GetQsyt1ByCzZzRequest request)
    {
        return base.Channel.GetQsyt1ByCzZzAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1ByCzZzResponse> GetQsyt1ByCzZzAsync(string jwdCode, string czList, int zz, string sj, int minutes, int tons)
    {
        GetQsyt1ByCzZzRequest inValue = new GetQsyt1ByCzZzRequest();
        inValue.jwdCode = jwdCode;
        inValue.czList = czList;
        inValue.zz = zz;
        inValue.sj = sj;
        inValue.minutes = minutes;
        inValue.tons = tons;
        return ((HybzJctjWsPortType)(this)).GetQsyt1ByCzZzAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1ArrayByCzResponse> HybzJctjWsPortType.GetQsyt1ArrayByCzAsync(GetQsyt1ArrayByCzRequest request)
    {
        return base.Channel.GetQsyt1ArrayByCzAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1ArrayByCzResponse> GetQsyt1ArrayByCzAsync(string jwdCode, string cz, string sj)
    {
        GetQsyt1ArrayByCzRequest inValue = new GetQsyt1ArrayByCzRequest();
        inValue.jwdCode = jwdCode;
        inValue.cz = cz;
        inValue.sj = sj;
        return ((HybzJctjWsPortType)(this)).GetQsyt1ArrayByCzAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<PutSjbdUnFoundResponse> HybzJctjWsPortType.PutSjbdUnFoundAsync(PutSjbdUnFoundRequest request)
    {
        return base.Channel.PutSjbdUnFoundAsync(request);
    }

    public System.Threading.Tasks.Task<PutSjbdUnFoundResponse> PutSjbdUnFoundAsync(string bwInfo)
    {
        PutSjbdUnFoundRequest inValue = new PutSjbdUnFoundRequest();
        inValue.bwInfo = bwInfo;
        return ((HybzJctjWsPortType)(this)).PutSjbdUnFoundAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<TestResponse> HybzJctjWsPortType.TestAsync(TestRequest request)
    {
        return base.Channel.TestAsync(request);
    }

    public System.Threading.Tasks.Task<TestResponse> TestAsync()
    {
        TestRequest inValue = new TestRequest();
        return ((HybzJctjWsPortType)(this)).TestAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1ByBwmResponse> HybzJctjWsPortType.GetQsyt1ByBwmAsync(GetQsyt1ByBwmRequest request)
    {
        return base.Channel.GetQsyt1ByBwmAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1ByBwmResponse> GetQsyt1ByBwmAsync(string jwdCode, string bwm, string bwly)
    {
        GetQsyt1ByBwmRequest inValue = new GetQsyt1ByBwmRequest();
        inValue.jwdCode = jwdCode;
        inValue.bwm = bwm;
        inValue.bwly = bwly;
        return ((HybzJctjWsPortType)(this)).GetQsyt1ByBwmAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetSjbdUnFoundResponse> HybzJctjWsPortType.GetSjbdUnFoundAsync(GetSjbdUnFoundRequest request)
    {
        return base.Channel.GetSjbdUnFoundAsync(request);
    }

    public System.Threading.Tasks.Task<GetSjbdUnFoundResponse> GetSjbdUnFoundAsync(string czdm, string qsrq, string zzrq)
    {
        GetSjbdUnFoundRequest inValue = new GetSjbdUnFoundRequest();
        inValue.czdm = czdm;
        inValue.qsrq = qsrq;
        inValue.zzrq = zzrq;
        return ((HybzJctjWsPortType)(this)).GetSjbdUnFoundAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<PutSjbdUnFoundExResponse> HybzJctjWsPortType.PutSjbdUnFoundExAsync(PutSjbdUnFoundExRequest request)
    {
        return base.Channel.PutSjbdUnFoundExAsync(request);
    }

    public System.Threading.Tasks.Task<PutSjbdUnFoundExResponse> PutSjbdUnFoundExAsync(string bwInfo)
    {
        PutSjbdUnFoundExRequest inValue = new PutSjbdUnFoundExRequest();
        inValue.bwInfo = bwInfo;
        return ((HybzJctjWsPortType)(this)).PutSjbdUnFoundExAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1BwmByEwmResponse> HybzJctjWsPortType.GetQsyt1BwmByEwmAsync(GetQsyt1BwmByEwmRequest request)
    {
        return base.Channel.GetQsyt1BwmByEwmAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1BwmByEwmResponse> GetQsyt1BwmByEwmAsync(string sEwm)
    {
        GetQsyt1BwmByEwmRequest inValue = new GetQsyt1BwmByEwmRequest();
        inValue.sEwm = sEwm;
        return ((HybzJctjWsPortType)(this)).GetQsyt1BwmByEwmAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1ArrayByCcResponse> HybzJctjWsPortType.GetQsyt1ArrayByCcAsync(GetQsyt1ArrayByCcRequest request)
    {
        return base.Channel.GetQsyt1ArrayByCcAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1ArrayByCcResponse> GetQsyt1ArrayByCcAsync(string jwdCode, string cc, string sj)
    {
        GetQsyt1ArrayByCcRequest inValue = new GetQsyt1ArrayByCcRequest();
        inValue.jwdCode = jwdCode;
        inValue.cc = cc;
        inValue.sj = sj;
        return ((HybzJctjWsPortType)(this)).GetQsyt1ArrayByCcAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<PutSjbdFoundResponse> HybzJctjWsPortType.PutSjbdFoundAsync(PutSjbdFoundRequest request)
    {
        return base.Channel.PutSjbdFoundAsync(request);
    }

    public System.Threading.Tasks.Task<PutSjbdFoundResponse> PutSjbdFoundAsync(string bwInfo)
    {
        PutSjbdFoundRequest inValue = new PutSjbdFoundRequest();
        inValue.bwInfo = bwInfo;
        return ((HybzJctjWsPortType)(this)).PutSjbdFoundAsync(inValue);
    }

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    System.Threading.Tasks.Task<GetQsyt1ArrayByCcSjTonsCzListResponse> HybzJctjWsPortType.GetQsyt1ArrayByCcSjTonsCzListAsync(GetQsyt1ArrayByCcSjTonsCzListRequest request)
    {
        return base.Channel.GetQsyt1ArrayByCcSjTonsCzListAsync(request);
    }

    public System.Threading.Tasks.Task<GetQsyt1ArrayByCcSjTonsCzListResponse> GetQsyt1ArrayByCcSjTonsCzListAsync(string jwdCode, string cc, string sj_s, string sj_e, int ton_s, int ton_e, string czList)
    {
        GetQsyt1ArrayByCcSjTonsCzListRequest inValue = new GetQsyt1ArrayByCcSjTonsCzListRequest();
        inValue.jwdCode = jwdCode;
        inValue.cc = cc;
        inValue.sj_s = sj_s;
        inValue.sj_e = sj_e;
        inValue.ton_s = ton_s;
        inValue.ton_e = ton_e;
        inValue.czList = czList;
        return ((HybzJctjWsPortType)(this)).GetQsyt1ArrayByCcSjTonsCzListAsync(inValue);
    }

    public virtual System.Threading.Tasks.Task OpenAsync()
    {
        return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
    }

    private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.HybzJctjWsHttpSoap11Endpoint))
        {
            System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            return result;
        }
        if ((endpointConfiguration == EndpointConfiguration.HybzJctjWsHttpSoap12Endpoint))
        {
            System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
            System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
            textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
            result.Elements.Add(textBindingElement);
            System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
            httpBindingElement.AllowCookies = true;
            httpBindingElement.MaxBufferSize = int.MaxValue;
            httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
            result.Elements.Add(httpBindingElement);
            return result;
        }
        throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
    }

    private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
    {
        if ((endpointConfiguration == EndpointConfiguration.HybzJctjWsHttpSoap11Endpoint))
        {
            return new System.ServiceModel.EndpointAddress("http://10.1.190.31/HybzJctj/services/HybzJctjWs.HybzJctjWsHttpSoap11Endpoint/");
        }
        if ((endpointConfiguration == EndpointConfiguration.HybzJctjWsHttpSoap12Endpoint))
        {
            return new System.ServiceModel.EndpointAddress("http://10.1.190.31/HybzJctj/services/HybzJctjWs.HybzJctjWsHttpSoap12Endpoint/");
        }
        throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
    }

    public enum EndpointConfiguration
    {

        HybzJctjWsHttpSoap11Endpoint,

        HybzJctjWsHttpSoap12Endpoint,
    }
}

