﻿using System;

namespace RabbitMQ.ComponentB
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start("http://*:1337/");

            Console.ReadLine();

            appHost.Dispose();
        }
    }
}
