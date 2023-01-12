using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.Services
{
    //class reference
    //https://stackoverflow.com/questions/32304766/how-can-i-cancel-from-device-starttimer
    //sound plugin https://devblogs.microsoft.com/xamarin/adding-sound-xamarin-forms-app/
    public class SoundService
    {
        ISimpleAudioPlayer player;
        readonly Action _Task;
        readonly List<TaskWrapper> _Tasks = new List<TaskWrapper>();
        readonly TimeSpan _Interval;
        public bool IsRecurring { get; }
        public bool IsRunning => _Tasks.Any(t => t.IsRunning);
        public SoundService()
        {
            player = CrossSimpleAudioPlayer.Current;
            player.Load("ring.wav");
            _Task = player.Play;
            _Interval = new TimeSpan(0, 0, 3);
        }
        public void Start()
        {
            if (IsRunning)
                // Already Running
                return;

            var wrapper = new TaskWrapper(_Task, true, true);
            _Tasks.Add(wrapper);
            //vibrate
            try
            {
                //Vibration.Vibrate();
                Device.StartTimer(_Interval, wrapper.RunTask);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Stop()
        {
            foreach (var task in _Tasks)
                task.IsRunning = false;
            _Tasks.Clear();
            //vibration
            //try
            //{
            //    Vibration.Cancel();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
        }

        class TaskWrapper
        {
            public bool IsRunning { get; set; }
            bool _IsRecurring;
            Action _Task;
            public TaskWrapper(Action task, bool isRecurring, bool isRunning)
            {
                _Task = task;
                _IsRecurring = isRecurring;
                IsRunning = isRunning;
            }

            public bool RunTask()
            {
                if (IsRunning)
                {
                    _Task();
                    if (_IsRecurring)
                        return true;
                }

                // No longer need to recur. Stop
                return IsRunning = false;
            }
        }
    }
}
