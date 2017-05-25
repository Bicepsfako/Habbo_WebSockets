using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    public class CheckPetNameEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string PetName = Packet.PopString();

            if (PetName.Length < 2)
            {
                Session.SendPacket(new CheckPetNameComposer(2, "2"));
                return;
            }
            else if (PetName.Length > 15)
            {
                Session.SendPacket(new CheckPetNameComposer(1, "15"));
                return;
            }
            else if (!WandalaEnvironment.IsValidAlphaNumeric(PetName))
            {
                Session.SendPacket(new CheckPetNameComposer(3, ""));
                return;
            }
            else if (WandalaEnvironment.GetGame().GetChatManager().GetFilter().IsFiltered(PetName))
            {
                Session.SendPacket(new CheckPetNameComposer(4, ""));
                return;
            }

            Session.SendPacket(new CheckPetNameComposer(0, ""));
        }
    }
}