using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HWService
{
    public partial class Service1 : ServiceBase
    {
        private Task _serviceTask = new Task(() => HWCode.HWServiceTask());

        public Service1()
        {
            InitializeComponent();

            EventLog.Log = "Homework Service";
        }

        protected override void OnStart(string[] args)
        {
            _serviceTask.Start();
            EventLog.WriteEntry("The service was started successfully!\nWorking directory: " + Directory.GetCurrentDirectory(), EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            HWCode.IsServiceStopping = true;
            _serviceTask.Wait();
            EventLog.WriteEntry("The service was stopped successfully!", EventLogEntryType.Information);
        }
    }
}
