using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Users;
using Wandala.HabboHotel.Users.Messenger;
using Wandala.HabboHotel.Users.Relationships;

namespace Wandala.Communication.Packets.Outgoing.Messenger
{
    class MessengerInitComposer : ServerPacket
    {
        public MessengerInitComposer()
            : base(ServerPacketHeader.MessengerInitMessageComposer)
        {
            base.WriteInteger(Convert.ToInt32(WandalaEnvironment.GetSettingsManager().TryGetValue("messenger.buddy_limit")));//Friends max.
            base.WriteInteger(300);
            base.WriteInteger(800);
            base.WriteInteger(0); // category count
        }
    }
}
