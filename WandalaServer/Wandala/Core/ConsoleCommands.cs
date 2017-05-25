using System;
using log4net;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Core
{
    public class ConsoleCommands
    {
        private static readonly ILog log = LogManager.GetLogger("Wandala.Core.ConsoleCommands");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    #region stop
                    case "stop":
                    case "shutdown":
                        {
                            log.Warn("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!");
                            WandalaEnvironment.PerformShutDown();
                            break;
                        }
                    #endregion

                    #region alert
                    case "alert":
                        {
                            string Notice = inputData.Substring(6);
                            WandalaEnvironment.GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(WandalaEnvironment.GetLanguageManager().TryGetValue("server.console.alert") + "\n\n" + Notice));
                            log.Info("Alert successfully sent.");
                            break;
                        }
                    #endregion

                    default:
                        {
                            log.Error(parameters[0].ToLower() + " is an unknown or unsupported command. Type help for more information");
                            break;
                        }
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Error in command [" + inputData + "]: " + e);
            }
        }
    }
}