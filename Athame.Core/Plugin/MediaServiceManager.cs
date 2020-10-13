using Athame.Plugin.Api;
using Athame.Plugin.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Core.Plugin
{
    public class MediaServiceManager
    {
        private readonly Dictionary<Uri, IMediaService> services = new Dictionary<Uri, IMediaService>();

        public void Add(IEnumerable<IPlugin> plugins)
        {
            foreach (var p in plugins)
            {
                AddService(p.Service);
            }
        }

        public void AddService(IMediaService service)
        {
            foreach (var uri in service.BaseUri)
            {
                services.Add(uri, service);
            }
        }

        public IMediaService GetService(Uri baseUri)
            => (from s in services where s.Key.Scheme == baseUri.Scheme && s.Key.Host == baseUri.Host select s.Value)
            .FirstOrDefault();

        public IEnumerable<IMediaService> Services
            => (from s in services select s.Value)
            .Distinct();
    }
}
