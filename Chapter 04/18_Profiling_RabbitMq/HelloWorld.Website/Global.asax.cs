﻿using System;
using System.Web;
using ServiceStack;
using ServiceStack.MiniProfiler;

namespace HelloWorld.Website
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender,
                                         EventArgs e)
        {
            var licensePath = @"~/../../../license.txt".MapHostAbsolutePath();
            Licensing.RegisterLicenseFromFileIfExists(licensePath);

            new AppHost().Init();
        }

        protected void Application_BeginRequest(object sender,
                                                EventArgs e)
        {
            Profiler.Start();
        }

        protected void Application_EndRequest(object src,
                                              EventArgs e)
        {
            Profiler.Stop();
        }
    }
}
