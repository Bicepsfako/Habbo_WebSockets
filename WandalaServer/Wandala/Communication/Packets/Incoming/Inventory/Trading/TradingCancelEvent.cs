using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Rooms.Trading;
using Wandala.Communication.Packets.Outgoing.Inventory.Trading;

namespace Wandala.Communication.Packets.Incoming.Inventory.Trading
{
    class TradingCancelEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser RoomUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomUser == null)
                return;

            Trade Trade = null;
            if (!Room.GetTrading().TryGetTrade(RoomUser.TradeId, out Trade))
            {
                Session.SendPacket(new TradingClosedComposer(Session.GetHabbo().Id));
                return;
            }

            Trade.EndTrade(Session.GetHabbo().Id);
        }
    }
}