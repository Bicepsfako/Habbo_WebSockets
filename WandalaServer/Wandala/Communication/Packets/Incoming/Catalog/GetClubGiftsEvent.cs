using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Catalog;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new ClubGiftsComposer());
        }
    }
}
