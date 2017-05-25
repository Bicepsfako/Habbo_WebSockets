using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Items;
using Wandala.Communication.Packets.Outgoing.Catalog;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ItemData Item = WandalaEnvironment.GetGame().GetItemManager().GetItemByName(Type);
            if (Item == null)
                return;

            int PetId = Item.BehaviourData;

            Session.SendPacket(new SellablePetBreedsComposer(Type, PetId, WandalaEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}