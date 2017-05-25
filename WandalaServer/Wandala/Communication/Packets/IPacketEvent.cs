using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;

namespace Wandala.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient session, ClientPacket packet);
    }
}