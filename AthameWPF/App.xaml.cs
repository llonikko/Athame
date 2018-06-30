using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Windows;

namespace AthameWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static AppInstance Context { get; set; }

        static App()
        {
            Context = new AppInstance();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Context.Begin();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Context.End();
        }
    }
}
