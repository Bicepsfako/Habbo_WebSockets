using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Wandala.HabboHotel.Users.Clothing;

using Wandala.HabboHotel.Users.Clothing.Parts;

namespace Wandala.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    class FigureSetIdsComposer : ServerPacket
    {
        public FigureSetIdsComposer(ICollection<ClothingParts> ClothingParts)
            : base(ServerPacketHeader.FigureSetIdsMessageComposer)
        {
            base.WriteInteger(ClothingParts.Count);
            foreach (ClothingParts Part in ClothingParts.ToList())
            {
                base.WriteInteger(Part.PartId);
            }

            base.WriteInteger(ClothingParts.Count);
            foreach (ClothingParts Part in ClothingParts.ToList())
            {
               base.WriteString(Part.Part);
            }
        }
    }
}
