using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Groups;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Users;

namespace Wandala.Communication.Packets.Incoming.Users
{
    class GetHabboGroupBadgesEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            Dictionary<int, string> Badges = new Dictionary<int, string>();
            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers().ToList())
            {
                if (User.IsBot || User.IsPet || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                    continue;

                if (User.GetClient().GetHabbo().GetStats().FavouriteGroupId == 0 || Badges.ContainsKey(User.GetClient().GetHabbo().GetStats().FavouriteGroupId))
                    continue;

                Group Group = null;
                if (!WandalaEnvironment.GetGame().GetGroupManager().TryGetGroup(User.GetClient().GetHabbo().GetStats().FavouriteGroupId, out Group))
                    continue;

                if (!Badges.ContainsKey(Group.Id))
                    Badges.Add(Group.Id, Group.Badge);
            }

            if (Session.GetHabbo().GetStats().FavouriteGroupId > 0)
            {
                Group Group = null;
                if (WandalaEnvironment.GetGame().GetGroupManager().TryGetGroup(Session.GetHabbo().GetStats().FavouriteGroupId, out Group))
                {
                    if (!Badges.ContainsKey(Group.Id))
                        Badges.Add(Group.Id, Group.Badge);
                }
            }

            Room.SendPacket(new HabboGroupBadgesComposer(Badges));
            Session.SendPacket(new HabboGroupBadgesComposer(Badges));
        }
    }
}