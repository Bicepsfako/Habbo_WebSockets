using Alchemy;
using Alchemy.Classes;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Wandala
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            WandalaEnvironment.Initialize();
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            //Logger.LogCriticalException("SYSTEM CRITICAL EXCEPTION: " + e);
            WandalaEnvironment.PerformShutDown();
        }

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(CtrlType sig);
    }
}
