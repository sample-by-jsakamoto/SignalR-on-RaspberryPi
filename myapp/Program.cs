using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using myapp;

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
        var isWin = Environment.OSVersion.Platform == PlatformID.Win32NT;
        if (isWin) baseAddress = "http://localhost:8081/";
        var httpHost = WebApp.Start<Startup>(url: baseAddress);

        var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

        var watcher = new Thread(Watcher);
        watcher.Start();

        var urlOfThisApp = baseAddress.Replace("*", "127.0.0.1");

        Console.WriteLine();
        for (; ; )
        {
            Console.WriteLine("URL: " + urlOfThisApp);
            Console.WriteLine("[O]pen browser / [Q]uit" + (isWin ? " / Toggle [S]w1" : ""));
            var key = Console.ReadKey(intercept: true).KeyChar.ToString().ToUpper();
            Console.WriteLine(key);
            Console.WriteLine();
            switch (key)
            {
                case "O":
                    Process.Start(urlOfThisApp);
                    break;
                case "S":
                    if (isWin)
                    {
                        var gpio24valuePath = Path.Combine(TinyGPIO.Debug.DebugPath, @"sys\class\gpio\gpio24\value");
                        var gpio24value = File.ReadAllLines(gpio24valuePath).First();
                        gpio24value = gpio24value == "0" ? "1" : "0";
                        File.WriteAllLines(gpio24valuePath, new[] { gpio24value });
                    }
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
