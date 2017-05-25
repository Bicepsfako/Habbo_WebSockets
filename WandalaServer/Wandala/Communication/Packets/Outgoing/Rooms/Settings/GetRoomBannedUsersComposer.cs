using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Users;
using Wandala.HabboHotel.Cache;
using Wandala.HabboHotel.Cache.Type;

namespace Wandala.Communication.Packets.Outgoing.Rooms.Settings
{
    class GetRoomBannedUsersComposer : ServerPacket
    {
        public GetRoomBannedUsersComposer(Room instance)
            : base(ServerPacketHeader.GetRoomBannedUsersMessageComposer)
        {
            base.WriteInteger(instance.Id);

            base.WriteInteger(instance.GetBans().BannedUsers().Count);//Count
            foreach (int Id in instance.GetBans().BannedUsers().ToList())
            {
                UserCache Data = WandalaEnvironment.GetGame().GetCacheManager().GenerateUser(Id);

                if (Data == null)
                {
                    base.WriteInteger(0);
                    base.WriteString("Unknown Error");
                }
                else
                {
                    base.WriteInteger(Data.Id);
                    base.WriteString(Data.Username);
                }
            }
        }
    }
}