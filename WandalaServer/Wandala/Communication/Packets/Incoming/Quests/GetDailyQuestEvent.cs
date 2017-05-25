using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.LandingView;

namespace Wandala.Communication.Packets.Incoming.Quests
{
    class GetDailyQuestEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int UsersOnline = WandalaEnvironment.GetGame().GetClientManager().Count;

            Session.SendPacket(new ConcurrentUsersGoalProgressComposer(UsersOnline));
        }
    }
}
