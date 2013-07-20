using System;
using System.Collections.Generic;
using System.Xml;
using Sponge.Common.Configuration.ServiceReference;

namespace Sponge.Common.Utilities
{
    public class Config : IDisposable
    {
        private ConfigService _svc;

        public Config(string url)
        {
            if (!url.EndsWith("/"))
                url += "/";

            url += "_layouts/Sponge/ConfigService.asmx";

            _svc = new ConfigService();
            _svc.Url = url;
            _svc.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        }

        public T Get<T>(string app, string key)
        {
            object result = _svc.Get(app, key);
            return (T)result;
        }

        public object Get(string app, string key)
        {
            return _svc.Get(app, key);
        }

        public Dictionary<string, string> GetAll(string app)
        {
            return FromXml(_svc.GetAll(app));
        }

        public void Set(string app, string key, string value)
        {
            _svc.Set(app, key, value);
        }

        public void CreateApplication(string appName)
        {
            _svc.CreateApplication(appName);
        }

        public bool ApplicationExists(string appName)
        {
            return _svc.ApplicationExists(appName);
        }

        public void Dispose()
        {
            if (_svc != null)
                _svc.Dispose();
        }

        private Dictionary<string, string> FromXml(XmlNode doc)
        {
            var dict = new Dictionary<string, string>();
            foreach (XmlNode el in doc.ChildNodes)
                dict.Add(el.Name, el.Value);

            return dict;
        }
    }
}
