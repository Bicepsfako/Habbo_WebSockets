using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.Users;
using Wandala.HabboHotel.Navigator;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Navigator;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = WandalaEnvironment.GetGame().GetNavigator().GetFlatCategories();

            Session.SendPacket(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}