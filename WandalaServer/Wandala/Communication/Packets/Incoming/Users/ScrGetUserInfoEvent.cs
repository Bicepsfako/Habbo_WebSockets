using System;
using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Users;

namespace Wandala.Communication.Packets.Incoming.Users
{
    class ScrGetUserInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new ScrSendUserInfoComposer());
        }
    }
}
