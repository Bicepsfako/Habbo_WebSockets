using System;
using Wandala.Database.Interfaces;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Handshake;

namespace Wandala.Communication.Packets.Incoming.Handshake
{
    public class UniqueIDEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Junk = Packet.PopString();
            string MachineId = Packet.PopString();

            Session.MachineId = MachineId;

            Session.SendPacket(new SetUniqueIdComposer(MachineId));
        }
    }
}