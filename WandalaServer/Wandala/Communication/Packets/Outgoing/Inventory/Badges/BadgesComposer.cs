﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Users.Badges;

namespace Wandala.Communication.Packets.Outgoing.Inventory.Badges
{
    class BadgesComposer : ServerPacket
    {
        public BadgesComposer(GameClient Session)
            : base(ServerPacketHeader.BadgesMessageComposer)
        {
            List<Badge> EquippedBadges = new List<Badge>();

            base.WriteInteger(Session.GetHabbo().GetBadgeComponent().Count);
            foreach (Badge Badge in Session.GetHabbo().GetBadgeComponent().GetBadges().ToList())
            {
                base.WriteInteger(1);
                base.WriteString(Badge.Code);

                if (Badge.Slot > 0)
                    EquippedBadges.Add(Badge);
            }

            base.WriteInteger(EquippedBadges.Count);
            foreach (Badge Badge in EquippedBadges)
            {
                base.WriteInteger(Badge.Slot);
                base.WriteString(Badge.Code);
            }
        }
    }
}
