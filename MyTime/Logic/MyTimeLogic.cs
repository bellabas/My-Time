using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MyTime.Logic
{
    public class MyTimeLogic : IMyTimeLogic
    {
        DateTime dob;
        public TimeSpan MyElapsedTime { get; private set; }
        Timer timer;
        IMessenger messenger;

        public MyTimeLogic(IMessenger messenger)
        {
            this.messenger = messenger;
            this.timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += CalculateNew;
        }

        public void Calculate(DateTime dob)
        {
            this.dob = dob;
            DateTime currentTime = DateTime.Now;

            MyElapsedTime = currentTime - dob;

            messenger.Send(FormatTimeSpan(MyElapsedTime), nameof(MyElapsedTime) + "Status");
            timer.Start();
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
        }

        private void CalculateNew(object sender, ElapsedEventArgs arg)
        {
            MyElapsedTime = MyElapsedTime.Add(new TimeSpan(0, 0, 1));
            messenger.Send(FormatTimeSpan(MyElapsedTime), nameof(MyElapsedTime) + "Status");
        }
    }
}
