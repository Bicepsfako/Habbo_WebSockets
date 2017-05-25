using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Outgoing.Rooms.Engine;
using Wandala.Database.Interfaces;


namespace Wandala.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class FacelessCommand :IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_faceless"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Allows you to go faceless!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
    
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null || User.GetClient() == null)
                return;

            string[] headParts;
            string[] figureParts = Session.GetHabbo().Look.Split('.');
            foreach (string Part in figureParts)
            {
                if (Part.StartsWith("hd"))
                {
                    headParts = Part.Split('-');
                    if (!headParts[1].Equals("99999"))
                        headParts[1] = "99999";
                    else
                        return;

                    Session.GetHabbo().Look = Session.GetHabbo().Look.Replace(Part, "hd-" + headParts[1] + "-" + headParts[2]);
                    break;
                }
            }

            Session.GetHabbo().Look = WandalaEnvironment.GetFigureManager().ProcessFigure(Session.GetHabbo().Look, Session.GetHabbo().Gender, Session.GetHabbo().GetClothing().GetClothingParts, true);

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `look` = '" + Session.GetHabbo().Look + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            Session.SendPacket(new UserChangeComposer(User, true));
            Session.GetHabbo().CurrentRoom.SendPacket(new UserChangeComposer(User, false));
            return;
        }
    }
}
