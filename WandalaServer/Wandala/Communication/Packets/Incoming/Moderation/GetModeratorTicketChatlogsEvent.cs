using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Moderation;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class GetModeratorTicketChatlogsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tickets"))
                return;

            int TicketId = Packet.PopInt();

            ModerationTicket Ticket = null;
            if (!WandalaEnvironment.GetGame().GetModerationManager().TryGetTicket(TicketId, out Ticket) || Ticket.Room == null)
                return;

            RoomData Data = WandalaEnvironment.GetGame().GetRoomManager().GenerateRoomData(Ticket.Room.Id);
            if (Data == null)
                return;

            Session.SendPacket(new ModeratorTicketChatlogComposer(Ticket, Data, Ticket.Timestamp));
        }
    }
}
