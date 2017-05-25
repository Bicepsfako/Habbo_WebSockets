using System;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Handshake;

namespace Wandala.Communication.Packets.Incoming.Handshake
{
    public class SSOTicketEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.RC4Client == null || Session.GetHabbo() != null)
                return;

            string SSO = Packet.PopString();
            if (string.IsNullOrEmpty(SSO) || SSO.Length < 15)
                return;

            Session.TryAuthenticate(SSO);
        }
    }
}