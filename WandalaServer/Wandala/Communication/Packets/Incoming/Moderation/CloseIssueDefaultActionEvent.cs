using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Moderation;
using Wandala.Database.Interfaces;
using Wandala.HabboHotel.Rooms;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class CloseIssueDefaultActionEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null)
                return;
        }
    }
}
