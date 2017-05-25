using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Outgoing.Groups;

namespace Wandala.Communication.Packets.Incoming.Groups
{
    class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            session.SendPacket(new BadgeEditorPartsComposer(
                WandalaEnvironment.GetGame().GetGroupManager().BadgeBases,
                WandalaEnvironment.GetGame().GetGroupManager().BadgeSymbols,
                WandalaEnvironment.GetGame().GetGroupManager().BadgeBaseColours,
                WandalaEnvironment.GetGame().GetGroupManager().BadgeSymbolColours,
                WandalaEnvironment.GetGame().GetGroupManager().BadgeBackColours));
        }
    }
}
