using System;

namespace Zhaoxi.SmartParking.Client.Common
{
    public class SignalTaskHelper
    {
        private bool _signal = false;
        private bool _terminate = false;

        public bool Signal => _signal;

        public bool IsTerminate => _terminate;

        public void Set() => _signal = true;

        public void Terminate() => _terminate = true;

        public bool Process(Action action, int waitSeconds)
        {
            int sleepSec = 0;
            _terminate = false;
            _signal = false;
            action();
            while (!_signal)
            {
                if (_terminate) return false;
                System.Threading.Thread.Sleep(1000);
                System.Threading.Interlocked.Increment(ref sleepSec);

                if (sleepSec >= waitSeconds)
                {
                    return false;
                }
            }
            return _signal;
        }
    }
}
