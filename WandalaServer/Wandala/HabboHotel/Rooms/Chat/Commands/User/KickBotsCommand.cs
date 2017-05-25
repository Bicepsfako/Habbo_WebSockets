using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms.AI;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;

using Wandala.HabboHotel.Users.Inventory.Bots;
using Wandala.Communication.Packets.Outgoing.Inventory.Bots;

using Wandala.Database.Interfaces;


namespace Wandala.HabboHotel.Rooms.Chat.Commands.User
{
    class KickBotsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_kickbots"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Kick all of the bots from the room."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Oops, only the room owner can run this command!");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.IsPet || !User.IsBot)
                    continue;

                RoomUser BotUser = null;
                if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                    return;

                using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `bots` SET `room_id` = '0' WHERE `id` = @id LIMIT 1");
                    dbClient.AddParameter("id", User.BotData.Id);
                    dbClient.RunQuery();
                }

                Session.GetHabbo().GetInventoryComponent().TryAddBot(new Bot(Convert.ToInt32(BotUser.BotData.Id), Convert.ToInt32(BotUser.BotData.ownerID), BotUser.BotData.Name, BotUser.BotData.Motto, BotUser.BotData.Look, BotUser.BotData.Gender));
                Session.SendPacket(new BotInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetBots()));
                Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
            }

            Session.SendWhisper("Success, removed all bots.");
        }
    }
}
