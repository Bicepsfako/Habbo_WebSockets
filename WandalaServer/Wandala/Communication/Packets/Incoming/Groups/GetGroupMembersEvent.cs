using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Users;
using Wandala.HabboHotel.Groups;
using Wandala.Communication.Packets.Outgoing.Groups;
using Wandala.HabboHotel.Users.Authenticator;
using Wandala.HabboHotel.Cache;
using Wandala.HabboHotel.Cache.Type;

namespace Wandala.Communication.Packets.Incoming.Groups
{
    class GetGroupMembersEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int Page = Packet.PopInt();
            string SearchVal = Packet.PopString();
            int RequestType = Packet.PopInt();

            Group Group = null;
            if (!WandalaEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            List<UserCache> Members = new List<UserCache>();

            switch (RequestType)
            {
                case 0:
                    {
                        List<int> MemberIds = Group.GetAllMembers;
                        foreach (int Id in MemberIds.ToList())
                        {
                            UserCache GroupMember = WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                            if (GroupMember == null)
                                continue;

                            if (!Members.Contains(GroupMember))
                                Members.Add(GroupMember);
                        }
                        break;
                    }

                case 1:
                    {
                        List<int> AdminIds = Group.GetAdministrators;
                        foreach (int Id in AdminIds.ToList())
                        {
                            UserCache GroupMember = WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                            if (GroupMember == null)
                                continue;

                            if (!Members.Contains(GroupMember))
                                Members.Add(GroupMember);
                        }
                        break;
                    }

                case 2:
                    {
                        List<int> RequestIds = Group.GetRequests;
                        foreach (int Id in RequestIds.ToList())
                        {
                            UserCache GroupMember = WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(Id);
                            if (GroupMember == null)
                                continue;

                            if (!Members.Contains(GroupMember))
                                Members.Add(GroupMember);
                        }
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(SearchVal))
                Members = Members.Where(x => x.Username.StartsWith(SearchVal)).ToList();

            int StartIndex = ((Page - 1) * 14 + 14);
            int FinishIndex = Members.Count;

            Session.SendPacket(new GroupMembersComposer(Group, Members.Skip(StartIndex).Take(FinishIndex - StartIndex).ToList(), Members.Count, Page, (Group.CreatorId == Session.GetHabbo().Id || Group.IsAdmin(Session.GetHabbo().Id)), RequestType, SearchVal));
        }
    }
}