using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Wandala.HabboHotel.Groups;
using Wandala.Communication.Packets.Outgoing.Groups;

namespace Wandala.Communication.Packets.Incoming.Groups
{
    class GetGroupInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            bool NewWindow = Packet.PopBoolean();

            Group Group = null;
            if (!WandalaEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            Session.SendPacket(new GroupInfoComposer(Group, Session, NewWindow));     
        }
    }
}
