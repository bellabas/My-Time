using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            timer.Elapsed += CalculateEvent;
            LoadSave();
        }

        public void Calculate(DateTime dob)
        {
            this.dob = dob;
            SaveState();
            MyElapsedTime = DateTime.Now - dob;
            messenger.Send(FormatTimeSpan(MyElapsedTime), nameof(MyElapsedTime) + "Status");
            timer.Start();
        }

        private void Calculate()
        {
            MyElapsedTime = DateTime.Now - dob;
            messenger.Send(FormatTimeSpan(MyElapsedTime), nameof(MyElapsedTime) + "Status");
            timer.Start();
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{((int)timeSpan.TotalHours).ToString("N0").Replace(",", " ")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }

        private void CalculateEvent(object sender, ElapsedEventArgs arg)
        {
            MyElapsedTime = MyElapsedTime.Add(new TimeSpan(0, 0, 1));
            messenger.Send(FormatTimeSpan(MyElapsedTime), nameof(MyElapsedTime) + "Status");
            messenger.Send((object)this.dob, "DOBStatus");
        }

        private void SaveState()
        {
            string fileName = "save.json";
            string jsonString = JsonConvert.SerializeObject(this.dob);
            File.WriteAllText(fileName, jsonString);
        }

        private void LoadSave()
        {
            if (File.Exists("save.json"))
            {
                string jsonContent = File.ReadAllText("save.json");
                this.dob = JsonConvert.DeserializeObject<DateTime>(jsonContent);
                messenger.Send((object)this.dob, "DOBStatus");
                Calculate();
            }
        }

    }
}
