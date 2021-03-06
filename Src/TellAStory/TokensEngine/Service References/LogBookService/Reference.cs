﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TokensEngine.LogBookService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LogBookService.ILogBookService")]
    public interface ILogBookService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogBookService/CreateLog", ReplyAction="http://tempuri.org/ILogBookService/CreateLogResponse")]
        void CreateLog(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogBookService/WriteLogLine", ReplyAction="http://tempuri.org/ILogBookService/WriteLogLineResponse")]
        void WriteLogLine(string id, string line);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogBookService/ClearLog", ReplyAction="http://tempuri.org/ILogBookService/ClearLogResponse")]
        void ClearLog(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogBookService/DeleteLog", ReplyAction="http://tempuri.org/ILogBookService/DeleteLogResponse")]
        void DeleteLog(string id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILogBookServiceChannel : LogBookService.ILogBookService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LogBookServiceClient : System.ServiceModel.ClientBase<LogBookService.ILogBookService>, LogBookService.ILogBookService {
        
        public LogBookServiceClient() {
        }
        
        public LogBookServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LogBookServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LogBookServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LogBookServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CreateLog(string id) {
            base.Channel.CreateLog(id);
        }
        
        public void WriteLogLine(string id, string line) {
            base.Channel.WriteLogLine(id, line);
        }
        
        public void ClearLog(string id) {
            base.Channel.ClearLog(id);
        }
        
        public void DeleteLog(string id) {
            base.Channel.DeleteLog(id);
        }
    }
}
