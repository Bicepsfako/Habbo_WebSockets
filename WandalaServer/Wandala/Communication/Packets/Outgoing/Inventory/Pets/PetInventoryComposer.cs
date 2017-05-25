using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms.AI;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Users.Inventory;

namespace Wandala.Communication.Packets.Outgoing.Inventory.Pets
{
    class PetInventoryComposer : ServerPacket
    {
        public PetInventoryComposer(ICollection<Pet> Pets)
            : base(ServerPacketHeader.PetInventoryMessageComposer)
        {
            base.WriteInteger(1);
            base.WriteInteger(1);
            base.WriteInteger(Pets.Count);
            foreach (Pet Pet in Pets.ToList())
            {
                base.WriteInteger(Pet.PetId);
               base.WriteString(Pet.Name);
                base.WriteInteger(Pet.Type);
                base.WriteInteger(int.Parse(Pet.Race));
               base.WriteString(Pet.Color);
                base.WriteInteger(0);
                base.WriteInteger(0);
                base.WriteInteger(0);
            }
        }
    }
}