﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;

namespace Wandala.Communication.Packets.Outgoing.Rooms.Settings
{
    class FlatControllerRemovedComposer : ServerPacket
    {
        public FlatControllerRemovedComposer(Room Instance, int UserId)
            : base(ServerPacketHeader.FlatControllerRemovedMessageComposer)
        {
            base.WriteInteger(Instance.Id);
            base.WriteInteger(UserId);
        }
    }
}
