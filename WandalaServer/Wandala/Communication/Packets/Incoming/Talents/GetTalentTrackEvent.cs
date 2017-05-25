using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Talents;
using Wandala.Communication.Packets.Outgoing.Talents;

namespace Wandala.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ICollection<TalentTrackLevel> Levels = WandalaEnvironment.GetGame().GetTalentTrackManager().GetLevels();

            Session.SendPacket(new TalentTrackComposer(Levels, Type));
        }
    }
}
