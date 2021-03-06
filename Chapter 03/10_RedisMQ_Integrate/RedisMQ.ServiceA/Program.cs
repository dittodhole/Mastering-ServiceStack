﻿using System;
using RedisMQ.Models;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace RedisMQ.ServiceA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var licensePath = @"~/../../../../license.txt".MapHostAbsolutePath();
            Licensing.RegisterLicenseFromFileIfExists(licensePath);

            var redisClientManager = new BasicRedisClientManager();
            var redisMqServer = new RedisMqServer(redisClientManager);
            var messageQueueClient = redisMqServer.CreateMessageQueueClient();

            for (var i = 0;
                 i < 10;
                 i++)
            {
                var queueName = messageQueueClient.GetTempQueueName();
                var hello = new Hello
                            {
                                Name = i.ToString()
                            };
                var message = new Message<Hello>(hello)
                              {
                                  ReplyTo = queueName
                              };
                messageQueueClient.Publish(message);
                var response = messageQueueClient.Get<HelloResponse>(queueName);
                var helloResponse = response.GetBody();
                helloResponse.Result.Print();
            }

            Console.ReadLine();

            messageQueueClient.Dispose();
            redisMqServer.Dispose();
            redisClientManager.Dispose();
        }
    }
}
