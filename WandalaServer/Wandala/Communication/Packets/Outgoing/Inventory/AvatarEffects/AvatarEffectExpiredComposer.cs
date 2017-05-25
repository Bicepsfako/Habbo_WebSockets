using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Users.Effects;

namespace Wandala.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
            base.WriteInteger(Effect.SpriteId);
        }
    }
}
