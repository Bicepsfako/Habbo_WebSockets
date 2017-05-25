using System.Collections.Generic;

using Wandala.HabboHotel.Users.Messenger;
using Wandala.HabboHotel.Cache;
using Wandala.HabboHotel.Cache.Type;

namespace Wandala.Communication.Packets.Outgoing.Messenger
{
    class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
            base.WriteInteger(requests.Count);
            base.WriteInteger(requests.Count);

            foreach (MessengerRequest Request in requests)
            {
                base.WriteInteger(Request.From);
                base.WriteString(Request.Username);

                UserCache User = WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(Request.From);
                base.WriteString(User != null ? User.Look : "");
            }
        }
    }
}
