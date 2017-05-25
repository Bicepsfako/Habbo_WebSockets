using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Users.Effects;
using Wandala.Communication.Packets.Outgoing.Inventory.AvatarEffects;

namespace Wandala.Communication.Packets.Incoming.Inventory.AvatarEffects
{
    class AvatarEffectActivatedEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int EffectId = Packet.PopInt();

            AvatarEffect Effect = Session.GetHabbo().Effects().GetEffectNullable(EffectId, false, true);

            if (Effect == null || Session.GetHabbo().Effects().HasEffect(EffectId, true))
            {
                return;
            }

            if (Effect.Activate())
            {
                Session.SendPacket(new AvatarEffectActivatedComposer(Effect));
            }
        }
    }
}
