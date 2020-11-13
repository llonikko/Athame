using Athame.Plugin.Api;
using Athame.Plugin.Api.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Athame.Core.Plugin
{
    public static class MediaServiceManager
    {
        private static readonly Dictionary<Uri, IMediaService> services = new Dictionary<Uri, IMediaService>();

        public static void Add(IEnumerable<IPlugin> plugins)
        {
            foreach (IPlugin p in plugins)
            {
                AddService(p.Service);
            }
        }

        public static void AddService(IMediaService service)
        {
            foreach (var uri in service.BaseUri)
            {
                services.Add(uri, service);
            }
        }

        public static IMediaService GetService(Uri baseUri)
            => (from s in services where s.Key.Scheme == baseUri.Scheme && s.Key.Host == baseUri.Host select s.Value)
            .FirstOrDefault();

        public static IEnumerable<IMediaService> Services
            => (from s in services select s.Value)
            .Distinct();
    }
}
