using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace  HMI_Master_KX7500.Globals
{
    public static class Event
    {

        public delegate void UpdateEventHandler();
        public static event UpdateEventHandler UpdateEvent;
        public static void RaiseUpdateEvent()
        {
            UpdateEvent?.Invoke();
        }
    }
}
