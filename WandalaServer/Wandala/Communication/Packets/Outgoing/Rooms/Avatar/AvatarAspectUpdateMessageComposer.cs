using System;
using System.Linq;
using System.Text;


using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;


namespace Wandala.Communication.Packets.Outgoing.Rooms.Engine
{
    class AvatarAspectUpdateMessageComposer : ServerPacket
    {
        public AvatarAspectUpdateMessageComposer(string Figure, string Gender)
            : base(ServerPacketHeader.AvatarAspectUpdateMessageComposer)
        {
            base.WriteString(Figure);
            base.WriteString(Gender);


        }
    }
}