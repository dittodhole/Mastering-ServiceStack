﻿using RedisMQ.Models;
using ServiceStack;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;

namespace RedisMQ.ServiceA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var licensePath = @"~/../../../../license.txt".MapHostAbsolutePath();
            Licensing.RegisterLicenseFromFileIfExists(licensePath);

            AuthenticateResponse authenticateResponse;
            using (var jsonServiceClient = new JsonServiceClient(Constants.UrlBase))
            {
                authenticateResponse = jsonServiceClient.Post(new Authenticate
                                                              {
                                                                  RememberMe = true,
                                                                  UserName = "johndoe",
                                                                  Password = "password"
                                                              });

                // you can now assign the sessionId directly to the client
                // and objects, implementing IHasSessionId, will get automatically
                // assigned the session upon request. unfortunately MQ clients
                // do not offer this behaviour, so we need to do populating
                // ourselfs.
                //jsonServiceClient.SessionId = authenticateResponse.SessionId;
            }

            var redisClientManager = new BasicRedisClientManager();
            var redisMqServer = new RedisMqServer(redisClientManager);
            var messageQueueClient = redisMqServer.CreateMessageQueueClient();

            var hello = new Hello
                        {
                            SessionId = authenticateResponse.SessionId
                        };
            messageQueueClient.Publish(hello);

            messageQueueClient.Dispose();
            redisMqServer.Dispose();
            redisClientManager.Dispose();
        }
    }
}
