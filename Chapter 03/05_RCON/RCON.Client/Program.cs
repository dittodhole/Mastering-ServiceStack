﻿using System;
using System.Net;
using RCON.Models;
using ServiceStack;
using ServiceStack.Text;

namespace RCON.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var licensePath = @"~/../../../../license.txt".MapHostAbsolutePath();
            Licensing.RegisterLicenseFromFileIfExists(licensePath);

            const int port = 12345;
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback,
                                            port);
            var client = new ServiceStack.Messaging.Rcon.Client(ipEndPoint);
            client.Connect();

            for (var i = 0;
                 i < 10;
                 i++)
            {
                var hello = new Hello
                            {
                                Name = i.ToString()
                            };
                client.Call(hello,
                            Program.CallbackAfterHello);
            }
            Console.ReadLine();
        }

        private static void CallbackAfterHello(ServiceStack.Messaging.Rcon.Client rconClient,
                                               byte[] response)
        {
            var message = response.ToMessage<HelloResponse>();
            var helloResponse = message.GetBody();
            helloResponse.Result.Print();
        }
    }
}
