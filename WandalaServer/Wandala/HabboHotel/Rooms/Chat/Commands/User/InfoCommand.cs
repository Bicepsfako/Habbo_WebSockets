using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Wandala.Communication.Packets.Outgoing.Users;
using Wandala.Communication.Packets.Outgoing.Notifications;


using Wandala.Communication.Packets.Outgoing.Handshake;
using Wandala.Communication.Packets.Outgoing.Quests;
using Wandala.HabboHotel.Items;
using Wandala.Communication.Packets.Outgoing.Inventory.Furni;
using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.HabboHotel.Quests;
using Wandala.HabboHotel.Rooms;
using System.Threading;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Rooms.Avatar;
using Wandala.Communication.Packets.Outgoing.Pets;
using Wandala.Communication.Packets.Outgoing.Messenger;
using Wandala.HabboHotel.Users.Messenger;
using Wandala.Communication.Packets.Outgoing.Rooms.Polls;
using Wandala.Communication.Packets.Outgoing.Rooms.Notifications;
using Wandala.Communication.Packets.Outgoing.Availability;
using Wandala.Communication.Packets.Outgoing;
using Wandala.Communication.Packets.Outgoing.Rooms.Polls.Questions;

namespace Wandala.HabboHotel.Rooms.Chat.Commands.User
{
    class InfoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Displays generic information that everybody loves to see."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            TimeSpan Uptime = DateTime.Now - WandalaEnvironment.ServerStarted;
            int OnlineUsers = WandalaEnvironment.GetGame().GetClientManager().Count;
            int RoomCount = WandalaEnvironment.GetGame().GetRoomManager().Count;

            Session.SendPacket(new RoomNotificationComposer("Powered by WandalaServer",
                 "<b>Credits</b>:\n" +
                 "DevBest Community\n\n" +
                 "<b>Current run time information</b>:\n" +
                 "Online Users: " + OnlineUsers + "\n" +
                 "Rooms Loaded: " + RoomCount + "\n" +
                 "Uptime: " + Uptime.Days + " day(s), " + Uptime.Hours + " hours and " + Uptime.Minutes + " minutes.\n\n" +
                 "<b>SWF Revision</b>:\n" + WandalaEnvironment.SWFRevision, "Wandala", ""));
        }
    }
}