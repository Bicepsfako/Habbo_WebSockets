using System;
using System.Linq;
using System.Text;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Inventory.Purse;

namespace Wandala.Communication.Packets.Incoming.Inventory.Purse
{
    class GetCreditsInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new CreditBalanceComposer(Session.GetHabbo().Credits));
            Session.SendPacket(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
        }
    }
}
