using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timirbaev_Form
{
    internal class TimirbaevSessions
    {
        Process childProcess = null;
        System.Threading.EventWaitHandle stopEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StopEvent");
        System.Threading.EventWaitHandle startEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StartEvent");
        System.Threading.EventWaitHandle confirmEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "ConfirmEvent");
        System.Threading.EventWaitHandle closeEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "CloseEvent");

        public ObservableCollection<string> Sessions { get; set; } = new ObservableCollection<string>();

        private int sessionIndex = 1;
        public int SessionsCount
        {
            get => sessionIndex;
            set => sessionIndex = value;
        }
        private int sNumber = 0;

        public void StartSession()
        {
            if (childProcess == null || childProcess.HasExited)
            {
                childProcess = Process.Start("C:/Users/User/Documents/GitHub/System_programming/Timirbaev_Lab1/x64/Debug/Timirbaev_Console.exe");
                Sessions.Clear();
                Sessions.Add("Главный поток");
                Sessions.Add("Все потоки");
                sNumber = 0;
            }

            if (SessionsCount <= 0)
            {
                return; // Не создаем новые потоки, если значение 0 или меньше
            }

            for (int i = 0; i < SessionsCount; i++)
            {
                startEvent.Set();
                confirmEvent.WaitOne();
                Sessions.Add($"Поток № {++sNumber}");
            }
        }

        public void StopSession()
        {
            if (!(childProcess == null || childProcess.HasExited || (sNumber == 0)))
            {
                stopEvent.Set();
                confirmEvent.WaitOne();
                sNumber -= 1;
                Sessions.RemoveAt(sNumber + 2);
                if (sNumber == 0)
                {
                    Sessions.Clear();
                }
            }
        }

        public void CloseSessions()
        {
            closeEvent.Set();
        }
    }
}