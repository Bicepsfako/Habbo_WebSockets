using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Moderation;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class CallForHelpPendingCallsDeletedEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null)
                return;
            
            if (WandalaEnvironment.GetGame().GetModerationManager().UserHasTickets(session.GetHabbo().Id))
            {
                ModerationTicket PendingTicket = WandalaEnvironment.GetGame().GetModerationManager().GetTicketBySenderId(session.GetHabbo().Id);
                if (PendingTicket != null)
                {
                    PendingTicket.Answered = true;
                    WandalaEnvironment.GetGame().GetClientManager().SendPacket(new ModeratorSupportTicketComposer(session.GetHabbo().Id, PendingTicket), "mod_tool");
                }
            }
        }
    }
}
