using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;

namespace Wandala.HabboHotel.Rooms.Chat.Commands.User
{
    class UnloadCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_unload"; }
        }

        public string Parameters
        {
            get { return "%id%"; }
        }

        public string Description
        {
            get { return "Unload the current room."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().GetPermissions().HasRight("room_unload_any"))
            {
                Room R = null;
                if (!WandalaEnvironment.GetGame().GetRoomManager().TryGetRoom(Room.Id, out R))
                    return;

                WandalaEnvironment.GetGame().GetRoomManager().UnloadRoom(R, true);
            }
            else
            {
                if (Room.CheckRights(Session, true))
                {
                    WandalaEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);
                }
            }
        }
    }
}
