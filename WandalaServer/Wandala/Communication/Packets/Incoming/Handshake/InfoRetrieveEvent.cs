using System;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.Groups;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Handshake;

namespace Wandala.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendPacket(new UserObjectComposer(Session.GetHabbo()));
            Session.SendPacket(new UserPerksComposer(Session.GetHabbo()));
        }
    }
}