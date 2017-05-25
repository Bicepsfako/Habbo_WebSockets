
using Wandala.Communication.Packets.Outgoing.Navigator;

using Wandala.Database.Interfaces;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Users;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    public class AddFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            int RoomId = Packet.PopInt();

            RoomData Data = WandalaEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);

            if (Data == null || Session.GetHabbo().FavoriteRooms.Count >= 30 || Session.GetHabbo().FavoriteRooms.Contains(RoomId))
            {
                // send packet that favourites is full.
                return;
            }

            Session.GetHabbo().FavoriteRooms.Add(RoomId);
            Session.SendPacket(new UpdateFavouriteRoomComposer(RoomId, true));

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("INSERT INTO user_favorites (user_id,room_id) VALUES (" + Session.GetHabbo().Id + "," + RoomId + ")");
            }
        }
    }
}