using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using myapp;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;

class Program
{
    public static GPIOState CurrentState { get; set; }
    public static TinyGPIO GPIO24_Sw1 { get; set; }
    public static TinyGPIO GPIO25_LED1 { get; set; }
    private static bool Halt { get; set; }

    static void Main(string[] args)
    {
        InitiGPIO();
        CurrentState = new GPIOState();
        Halt = false;

        var baseAddress = "http://*:80/";
        if (Environment.OSVersion.Platform == PlatformID.Win32NT) baseAddress = "http://localhost:8081/";
        var httpHost = WebApp.Start<Startup>(url: baseAddress);

        var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

        var watcher = new Thread(Watcher);
        watcher.Start();


        Console.WriteLine();
        for (; ; )
        {
            Console.WriteLine("Toggle [S]w1 / Toggle [L]ed1 / [Q]uit");
            var key = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();
            Console.WriteLine(key);
            Console.WriteLine();
            switch (key)
            {
                case "S":
                    CurrentState.sw1 = !CurrentState.sw1;
                    hubContext.Clients.All.StateChange(CurrentState);
                    break;
                case "L":
                    CurrentState.led1 = !CurrentState.led1;
                    hubContext.Clients.All.StateChange(CurrentState);
                    break;
                case "Q":
                    Console.WriteLine("Stoping...");

                    Halt = true;
                    var terminated = watcher.Join(1000);
                    if (terminated == false) watcher.Abort();

                    httpHost.Dispose();

                    Console.WriteLine("Stoped.");
                    return;
                default: break;
            }
        }
    }

    private static void InitiGPIO()
    {
        // init GPIO 24 for Switch1
        GPIO24_Sw1 = TinyGPIO.Export(24);
        GPIO24_Sw1.Direction = GPIODirection.In;

        // init GPIO 25 for LED1
        GPIO25_LED1 = TinyGPIO.Export(25);
        GPIO25_LED1.Direction = GPIODirection.Out;
    }

    private static void Watcher()
    {
        var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
        while (Halt == false)
        {
            var sw1 = GPIO24_Sw1.Value != 0;
            lock (CurrentState)
            {
                if (CurrentState.sw1 != sw1)
                {
                    CurrentState.sw1 = sw1;
                    CurrentState.led1 = GPIO25_LED1.Value != 0;
                    hubContext.Clients.All.StateChange(CurrentState);
                }
            }
            Thread.Sleep(200);
        }
    }
}
