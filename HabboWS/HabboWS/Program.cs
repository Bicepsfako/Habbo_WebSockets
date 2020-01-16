using log4net;
using log4net.Config;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace HabboWS
{
    class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        private static readonly ILog log = LogManager.GetLogger("HabboWS.Program");

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            HabboEnvironment.Initialize();
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("SYSTEM CRITICAL EXCEPTION: " + e);
            HabboEnvironment.PerformShutDown(sender, args);
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
