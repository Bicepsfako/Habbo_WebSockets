namespace Wandala.Communication.Packets.Outgoing.Rooms.Chat
{
    public class FloodControlComposer : ServerPacket
    {
        public FloodControlComposer(int floodTime)
            : base(ServerPacketHeader.FloodControlMessageComposer)
        {
            base.WriteInteger(floodTime);
        }
    }
}