using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Rooms.Engine;
using Wandala.Database.Interfaces;
using Wandala.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Wandala.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class MimicCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mimic"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Liking someone elses swag? Copy it!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Please enter the username of the user you wish to mimic.");
                return;
            }

            GameClient TargetClient = WandalaEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
                return;
            }

            if (!TargetClient.GetHabbo().AllowMimic)
            {
                Session.SendWhisper("Oops, you cannot mimic this user - sorry!");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("An error occoured whilst finding that user, maybe they're not online or in this room.");
                return;
            }

            Session.GetHabbo().Gender = TargetUser.GetClient().GetHabbo().Gender;
            Session.GetHabbo().Look = TargetUser.GetClient().GetHabbo().Look;

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `gender` = @gender, `look` = @look WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("gender", Session.GetHabbo().Gender);
                dbClient.AddParameter("look", Session.GetHabbo().Look);
                dbClient.AddParameter("id", Session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User != null)
            {
                Session.SendPacket(new AvatarAspectUpdateComposer(Session.GetHabbo().Look, Session.GetHabbo().Gender));
                Session.SendPacket(new UserChangeComposer(User, true));
                Room.SendPacket(new UserChangeComposer(User, false));
            }
        }
    }
}