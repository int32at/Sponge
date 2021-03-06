﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.17929.
// 
#pragma warning disable 1591

namespace Sponge.Client.ConfigServiceReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ConfigServiceSoap", Namespace="http://Sponge.WebService.ConfigService")]
    public partial class ConfigService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetCentralOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetRelativeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ConfigService() {
            this.Url = global::Sponge.Client.Properties.Settings.Default.Sponge_Client_ConfigServiceReference_ConfigService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetCentralCompletedEventHandler GetCentralCompleted;
        
        /// <remarks/>
        public event GetRelativeCompletedEventHandler GetRelativeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Sponge.WebService.ConfigService/GetCentral", RequestNamespace="http://Sponge.WebService.ConfigService", ResponseNamespace="http://Sponge.WebService.ConfigService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration", IsNullable=true)]
        public Configuration GetCentral(string appName) {
            object[] results = this.Invoke("GetCentral", new object[] {
                        appName});
            return ((Configuration)(results[0]));
        }
        
        /// <remarks/>
        public void GetCentralAsync(string appName) {
            this.GetCentralAsync(appName, null);
        }
        
        /// <remarks/>
        public void GetCentralAsync(string appName, object userState) {
            if ((this.GetCentralOperationCompleted == null)) {
                this.GetCentralOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCentralOperationCompleted);
            }
            this.InvokeAsync("GetCentral", new object[] {
                        appName}, this.GetCentralOperationCompleted, userState);
        }
        
        private void OnGetCentralOperationCompleted(object arg) {
            if ((this.GetCentralCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCentralCompleted(this, new GetCentralCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Sponge.WebService.ConfigService/GetRelative", RequestNamespace="http://Sponge.WebService.ConfigService", ResponseNamespace="http://Sponge.WebService.ConfigService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Configuration", IsNullable=true)]
        public Configuration GetRelative(string spongeUrl, string appName) {
            object[] results = this.Invoke("GetRelative", new object[] {
                        spongeUrl,
                        appName});
            return ((Configuration)(results[0]));
        }
        
        /// <remarks/>
        public void GetRelativeAsync(string spongeUrl, string appName) {
            this.GetRelativeAsync(spongeUrl, appName, null);
        }
        
        /// <remarks/>
        public void GetRelativeAsync(string spongeUrl, string appName, object userState) {
            if ((this.GetRelativeOperationCompleted == null)) {
                this.GetRelativeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetRelativeOperationCompleted);
            }
            this.InvokeAsync("GetRelative", new object[] {
                        spongeUrl,
                        appName}, this.GetRelativeOperationCompleted, userState);
        }
        
        private void OnGetRelativeOperationCompleted(object arg) {
            if ((this.GetRelativeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetRelativeCompleted(this, new GetRelativeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Sponge.WebService.ConfigService")]
    public partial class Configuration {
        
        private Item[] configurationItemsField;
        
        private string nameField;
        
        private string spongeUrlField;
        
        private bool isOnlineField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ConfigurationItem")]
        public Item[] ConfigurationItems {
            get {
                return this.configurationItemsField;
            }
            set {
                this.configurationItemsField = value;
            }
        }
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string SpongeUrl {
            get {
                return this.spongeUrlField;
            }
            set {
                this.spongeUrlField = value;
            }
        }
        
        /// <remarks/>
        public bool IsOnline {
            get {
                return this.isOnlineField;
            }
            set {
                this.isOnlineField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Sponge.WebService.ConfigService")]
    public partial class Item {
        
        private string keyField;
        
        private object valueField;
        
        /// <remarks/>
        public string Key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        public object Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetCentralCompletedEventHandler(object sender, GetCentralCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCentralCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCentralCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Configuration Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Configuration)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetRelativeCompletedEventHandler(object sender, GetRelativeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetRelativeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetRelativeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Configuration Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Configuration)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591