using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Outgoing.Moderation;

namespace Wandala.Communication.Packets.Incoming.Moderation
{
    class ModeratorActionEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_caution"))
                return;

            if (!Session.GetHabbo().InRoom)
                return;

            Room CurrentRoom = Session.GetHabbo().CurrentRoom;
            if (CurrentRoom == null)
                return;

            int AlertMode = Packet.PopInt(); 
            string AlertMessage = Packet.PopString();
            bool IsCaution = AlertMode != 3;

            AlertMessage = IsCaution ? "Caution from Moderator:\n\n" + AlertMessage : "Message from Moderator:\n\n" + AlertMessage;
            Session.GetHabbo().CurrentRoom.SendPacket(new BroadcastMessageAlertComposer(AlertMessage));
        }
    }
}
