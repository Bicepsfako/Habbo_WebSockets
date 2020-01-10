﻿using System;
using Alchemy.Classes;
using HabboWS.HabboHotel.Users;
using HabboWS.HabboHotel.Users.UserData;
using log4net;

namespace HabboWS.Communication
{
    public class WebSocket
    {
        private static readonly ILog log = LogManager.GetLogger("HabboWS.Communication.RCON.RCONConnection");

        public static void OnConnect(UserContext context)
        {
            log.Info("Connection from " + context.ClientAddress);
        }

        public static void OnReceive(UserContext context)
        {
            log.Info("Receiving data from " + context.ClientAddress);

            try
            {
                var json = context.DataFrame.ToString();
                log.Info(json);

                //dynamic obj = JsonConvert.DeserializeObject(json);
                string[] package = json.Split('|');

                switch (package[0])
                {
                    case "connect":
                        UserData data = UserDataFactory.GetUserData(int.Parse(package[1]));
                        if (data != null)
                        {
                            Habbo Generated = data.user;
                            if (Generated != null)
                            {
                                HabboEnvironment.AddToOnline(int.Parse(package[1]), Generated);
                                HabboEnvironment.UpdateConsoleTitle();
                            }
                        }
                        break;
                    case "disconnect":
                        Habbo Data = null;
                        HabboEnvironment.RemoveFromOnline(int.Parse(package[1]), out Data);
                        HabboEnvironment.UpdateConsoleTitle();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                log.Error("Error on receive: " + e.Message);
            }
        }

        private static void UpdateConsoleTitle()
        {
            throw new NotImplementedException();
        }

        public static void OnSend(UserContext context)
        {
            log.Info("Sending data to " + context.ClientAddress);
        }

        public static void OnDisconnect(UserContext context)
        {
            log.Info(context.ClientAddress + " disconnected");

            /*var user = OnlineUsers.Keys.Where(o => o.Context.ClientAddress == context.ClientAddress).Single();

            string trash; // Concurrent dictionaries make things weird

            OnlineUsers.TryRemove(user, out trash);

            if (!String.IsNullOrEmpty(user.Name))
            {
                var r = new Response { Type = ResponseType.Disconnect, Data = new { user.Name } };

                Broadcast(JsonConvert.SerializeObject(r));
            }

            BroadcastNameList();*/
        }
    }
}