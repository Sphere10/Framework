//-----------------------------------------------------------------------
// <copyright file="ScheduleTestForm.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sphere10.Framework;
using Sphere10.Framework.Scheduler;
using Sphere10.Framework.Windows.Forms;

namespace Sphere10.FrameworkTester {
    public partial class ScheduleTestForm : Form {
        public ScheduleTestForm() {
            InitializeComponent();
        }


        private void _job1Button_Click(object sender, EventArgs e) {
            var textWriter = new TextBoxWriter(textBox1);
            var logger = new MulticastLogger(
                new TextWriterLogger(textWriter),
                new ConsoleLogger()
                );

            var job1 =
                JobBuilder.For(() => logger.Info("Executed Job 1"))
                    .Called("Job1")
                    .RunOnce(DateTime.Now)
                    .RunAsyncronously()
                    .Repeat
                    .OnInterval(TimeSpan.FromSeconds(1))
                    .Build();

            var job2 =
                JobBuilder.For(() => logger.Info("Executed Job 2"))
                    .Called("Job2")
                    .RunOnce(DateTime.Now)
                    .RunAsyncronously()
                    .Repeat
                    .OnInterval(TimeSpan.FromSeconds(2))
                    .Build();

            Sphere10.Framework.Scheduler.Scheduler.Global.JobStatusChanged += 
                (job, fromStatus, toStatus) => textWriter.WriteLine("{0}: {1} -> {2}", job.Name, fromStatus, toStatus);
            Sphere10.Framework.Scheduler.Scheduler.Global.AddJob(job1);
            Sphere10.Framework.Scheduler.Scheduler.Global.AddJob(job2);


        }
    }
}
