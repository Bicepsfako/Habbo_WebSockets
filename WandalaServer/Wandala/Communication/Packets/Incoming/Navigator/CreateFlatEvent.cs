using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Outgoing.Navigator;
using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.HabboHotel.Navigator;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    class CreateFlatEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;
            
            if (Session.GetHabbo().UsersRooms.Count >= 500)
            {
                Session.SendPacket(new CanCreateRoomComposer(true, 500));
                return;
            }

            string Name = WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(Packet.PopString());
            string Description = WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(Packet.PopString());
            string ModelName = Packet.PopString();

            int Category = Packet.PopInt();
            int MaxVisitors = Packet.PopInt();//10 = min, 25 = max.
            int TradeSettings = Packet.PopInt();//2 = All can trade, 1 = owner only, 0 = no trading.

            if (Name.Length < 3)
                return;

            if (Name.Length > 25)
                return;

            RoomModel RoomModel = null;
            if (!WandalaEnvironment.GetGame().GetRoomManager().TryGetModel(ModelName, out RoomModel))
                return;

            SearchResultList SearchResultList = null;
            if (!WandalaEnvironment.GetGame().GetNavigator().TryGetSearchResultList(Category, out SearchResultList))
                Category = 36;
            
            if (SearchResultList.CategoryType != NavigatorCategoryType.CATEGORY || SearchResultList.RequiredRank > Session.GetHabbo().Rank)
                Category = 36;

            if (MaxVisitors < 10 || MaxVisitors > 25)
                MaxVisitors = 10;

            if (TradeSettings < 0 || TradeSettings > 2)
                TradeSettings = 0;

            RoomData NewRoom = WandalaEnvironment.GetGame().GetRoomManager().CreateRoom(Session, Name, Description, ModelName, Category, MaxVisitors, TradeSettings);
            if (NewRoom != null)
            {
                Session.SendPacket(new FlatCreatedComposer(NewRoom.Id, Name));
            }
        }
    }
}
