using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Incoming;

namespace Wandala.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Build = Packet.PopString();

            if (WandalaEnvironment.SWFRevision != Build)
                WandalaEnvironment.SWFRevision = Build;
        }
    }
}