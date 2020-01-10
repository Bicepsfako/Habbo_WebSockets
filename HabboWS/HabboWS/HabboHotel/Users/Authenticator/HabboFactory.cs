using System;
using System.Data;

namespace HabboWS.HabboHotel.Users.Authenticator
{
    public static class HabboFactory
    {
        public static Habbo GenerateHabbo(DataRow Row, DataRow UserInfo)
        {
            return new Habbo(
                Convert.ToInt32(Row["id"]),
                Convert.ToString(Row["username"]),
                Convert.ToInt32(Row["rank"]),
                Convert.ToString(Row["motto"]),
                Convert.ToString(Row["look"]),
                Convert.ToString(Row["gender"]),
                Convert.ToInt32(Row["credits"]),
                Convert.ToInt32(Row["activity_points"]),
                Convert.ToInt32(Row["home_room"]),
                HabboEnvironment.EnumToBool(Row["block_newfriends"].ToString()),
                Convert.ToInt32(Row["last_online"]),
                HabboEnvironment.EnumToBool(Row["hide_online"].ToString()),
                HabboEnvironment.EnumToBool(Row["hide_inroom"].ToString()),
                Convert.ToDouble(Row["account_created"]),
                Convert.ToInt32(Row["vip_points"]),
                Convert.ToString(Row["machine_id"]),
                Convert.ToString(Row["volume"]),
                HabboEnvironment.EnumToBool(Row["chat_preference"].ToString()),
                HabboEnvironment.EnumToBool(Row["focus_preference"].ToString()),
                HabboEnvironment.EnumToBool(Row["pets_muted"].ToString()),
                HabboEnvironment.EnumToBool(Row["bots_muted"].ToString()),
                Convert.ToDouble(Row["last_change"].ToString()),
                Convert.ToInt32(Row["gotw_points"]),
                HabboEnvironment.EnumToBool(Convert.ToString(Row["ignore_invites"])),
                HabboEnvironment.EnumToBool(Row["allow_gifts"].ToString()),
                HabboEnvironment.EnumToBool(Row["allow_mimic"].ToString()), 
                Convert.ToInt32(Row["rank_vip"])
                );
        }
    }
}
