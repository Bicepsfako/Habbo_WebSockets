using Wandala.HabboHotel.Rooms;

namespace Wandala.Communication.Packets.Outgoing.Rooms.Avatar
{
    public class ActionComposer : ServerPacket
    {
        public ActionComposer(int VirtualId, int Action)
            : base(ServerPacketHeader.ActionMessageComposer)
        {
            base.WriteInteger(VirtualId);
            base.WriteInteger(Action);
        }
    }
}