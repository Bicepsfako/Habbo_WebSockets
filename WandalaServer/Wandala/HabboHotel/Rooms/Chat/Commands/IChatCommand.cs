using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;

namespace Wandala.HabboHotel.Rooms.Chat.Commands
{
    public interface IChatCommand
    {
        string PermissionRequired { get; }
        string Parameters { get; }
        string Description { get; }
        void Execute(GameClient Session, Room Room, string[] Params);
    }
}
