﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Outgoing.Rooms.Settings;

namespace Wandala.Communication.Packets.Incoming.Rooms.Settings
{
    class GetRoomRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null)
                return;

            if (!Instance.CheckRights(Session))
                return;


            Session.SendPacket(new RoomRightsListComposer(Instance));
        }
    }
}
