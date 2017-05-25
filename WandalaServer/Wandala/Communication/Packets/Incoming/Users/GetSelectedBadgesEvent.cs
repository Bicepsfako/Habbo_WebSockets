using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Users;
using Wandala.Communication.Packets.Outgoing.Users;

namespace Wandala.Communication.Packets.Incoming.Users
{
    class GetSelectedBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            Habbo Habbo = WandalaEnvironment.GetHabboById(UserId);
            if (Habbo == null)
                return;

            Session.SendPacket(new HabboUserBadgesComposer(Habbo));
        }
    }
}