﻿using Wandala.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Wandala.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class HALCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_hal"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Send a message to the entire hotel, with a link."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 2)
            {
                Session.SendWhisper("Please enter a message and a URL to send..");
                return;
            }

            string URL = Params[1];

            string Message = CommandManager.MergeParams(Params, 2);
            WandalaEnvironment.GetGame().GetClientManager().SendPacket(new RoomNotificationComposer("Habboon Hotel Alert!", Message + "\r\n" + "- " + Session.GetHabbo().Username, "", URL, URL));
            return;
        }
    }
}
