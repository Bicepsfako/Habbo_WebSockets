using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Outgoing.Inventory.Achievements;

namespace Wandala.Communication.Packets.Incoming.Inventory.Achievements
{
    class GetAchievementsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new AchievementsComposer(Session, WandalaEnvironment.GetGame().GetAchievementManager()._achievements.Values.ToList()));
        }
    }
}
