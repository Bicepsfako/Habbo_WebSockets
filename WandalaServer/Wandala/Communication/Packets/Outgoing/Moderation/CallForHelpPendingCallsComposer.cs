using Wandala.HabboHotel.Moderation;
using Wandala.Utilities;

namespace Wandala.Communication.Packets.Outgoing.Moderation
{
    class CallForHelpPendingCallsComposer  :ServerPacket
    {
        public CallForHelpPendingCallsComposer(ModerationTicket ticket)
            : base(ServerPacketHeader.CallForHelpPendingCallsMessageComposer)
        {
            base.WriteInteger(1);// Count for whatever reason?
            {
                base.WriteString(ticket.Id.ToString());
                base.WriteString(UnixTimestamp.FromUnixTimestamp(ticket.Timestamp).ToShortTimeString());// "11-02-2017 04:07:05";
                base.WriteString(ticket.Issue);
            }
        }
    }
}
