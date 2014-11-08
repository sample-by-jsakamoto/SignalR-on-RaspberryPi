using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using app2;

class Program
{
    static void Main(string[] args)
    {
        var baseAddress = "http://*:80/";
        //var baseAddress = "http://localhost:8081/";
        var httpHost = WebApp.Start<Startup>(url: baseAddress);

        Console.WriteLine();
        for (; ; )
        {
            Console.WriteLine("[M]essage / [Q]uit");
            var key = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();
            Console.WriteLine(key);
            Console.WriteLine();
            switch (key)
            {
                case "M":
                    var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    hubContext.Clients.All.ReceivedMessage("Hey Guys!");
                    break;
                case "Q":
                    Console.WriteLine("Stoping...");
                    httpHost.Dispose();
                    Console.WriteLine("Stoped.");
                    return;
                default: break;
            }
        }
    }
}
