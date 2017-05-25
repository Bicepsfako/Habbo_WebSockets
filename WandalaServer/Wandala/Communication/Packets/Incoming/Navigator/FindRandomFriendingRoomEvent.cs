using Wandala.Communication.Packets.Outgoing.Rooms.Session;
using Wandala.HabboHotel.Rooms;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    class FindRandomFriendingRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Instance = WandalaEnvironment.GetGame().GetRoomManager().TryGetRandomLoadedRoom();

            if (Instance != null)
                Session.SendPacket(new RoomForwardComposer(Instance.Id));
        }
    }
}
