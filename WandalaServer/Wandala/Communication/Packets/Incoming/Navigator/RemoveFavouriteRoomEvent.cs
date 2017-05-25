
using Wandala.Communication.Packets.Outgoing.Navigator;

using Wandala.Database.Interfaces;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Users;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Navigator
{
    public class RemoveFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Id = Packet.PopInt();

            Session.GetHabbo().FavoriteRooms.Remove(Id);
            Session.SendPacket(new UpdateFavouriteRoomComposer(Id, false));

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM user_favorites WHERE user_id = " + Session.GetHabbo().Id + " AND room_id = " + Id + " LIMIT 1");
            }
        }
    }
}