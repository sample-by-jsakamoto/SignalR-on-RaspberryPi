using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace myapp
{
    public class MyHub : Hub
    {
        public GPIOState Enter()
        {
            lock (Program.CurrentState)
                return Program.CurrentState;
        }

        public void Turn(string onOrOff, string target)
        {
            lock (Program.CurrentState)
            {
                var led1 = onOrOff == "on";
                Program.CurrentState.led1 = led1;
                Program.GPIO25_LED1.Value = led1 ? 1 : 0;
                this.Clients.All.StateChange(Program.CurrentState);
            }
        }
    }
}
