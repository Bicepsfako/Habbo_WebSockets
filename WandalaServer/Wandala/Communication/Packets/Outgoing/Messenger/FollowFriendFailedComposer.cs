﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Wandala.Communication.Packets.Outgoing.Messenger
{
    class FollowFriendFailedComposer : ServerPacket
    {
        public FollowFriendFailedComposer(int ErrorCode)
            : base(ServerPacketHeader.FollowFriendFailedMessageComposer)
        {
            base.WriteInteger(ErrorCode);
        }
    }
}
