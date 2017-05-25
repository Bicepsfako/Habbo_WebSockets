using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Games;
using Wandala.Communication.Packets.Outgoing.GameCenter;

namespace Wandala.Communication.Packets.Incoming.GameCenter
{
    class GetGameListingEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<GameData> Games = WandalaEnvironment.GetGame().GetGameDataManager().GameData;

            Session.SendPacket(new GameListComposer(Games));
        }
    }
}
