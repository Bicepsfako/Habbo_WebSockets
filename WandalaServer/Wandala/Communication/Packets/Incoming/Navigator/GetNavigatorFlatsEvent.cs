using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Incoming;
using Wandala.Communication.Packets.Outgoing.Navigator;
using Wandala.HabboHotel.Navigator;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = WandalaEnvironment.GetGame().GetNavigator().GetEventCategories();

            Session.SendPacket(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}