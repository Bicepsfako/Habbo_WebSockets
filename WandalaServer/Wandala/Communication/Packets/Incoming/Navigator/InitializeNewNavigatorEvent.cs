using System.Collections.Generic;
using Wandala.Communication.Packets.Outgoing.Navigator.New;
using Wandala.HabboHotel.Navigator;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    class InitializeNewNavigatorEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<TopLevelItem> TopLevelItems = WandalaEnvironment.GetGame().GetNavigator().GetTopLevelItems();

            Session.SendPacket(new NavigatorMetaDataParserComposer(TopLevelItems));
            Session.SendPacket(new NavigatorLiftedRoomsComposer());
            Session.SendPacket(new NavigatorCollapsedCategoriesComposer());
            Session.SendPacket(new NavigatorPreferencesComposer());
        }
    }
}
