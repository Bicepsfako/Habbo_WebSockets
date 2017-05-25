using System.Collections.Generic;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Quests;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            WandalaEnvironment.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}