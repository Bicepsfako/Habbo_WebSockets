using System;
using System.Data;

namespace Wandala.HabboHotel.Users.Authenticator
{
    public static class HabboFactory
    {
        public static Habbo GenerateHabbo(DataRow Row, DataRow UserInfo)
        {
            return new Habbo(Convert.ToInt32(Row["id"]), Convert.ToString(Row["username"]), Convert.ToInt32(Row["rank"]), Convert.ToString(Row["motto"]), Convert.ToString(Row["look"]),
                Convert.ToString(Row["gender"]), Convert.ToInt32(Row["credits"]), Convert.ToInt32(Row["activity_points"]), 
                Convert.ToInt32(Row["home_room"]), WandalaEnvironment.EnumToBool(Row["block_newfriends"].ToString()), Convert.ToInt32(Row["last_online"]),
                WandalaEnvironment.EnumToBool(Row["hide_online"].ToString()), WandalaEnvironment.EnumToBool(Row["hide_inroom"].ToString()),
                Convert.ToDouble(Row["account_created"]), Convert.ToInt32(Row["vip_points"]),Convert.ToString(Row["machine_id"]), Convert.ToString(Row["volume"]),
                WandalaEnvironment.EnumToBool(Row["chat_preference"].ToString()), WandalaEnvironment.EnumToBool(Row["focus_preference"].ToString()), WandalaEnvironment.EnumToBool(Row["pets_muted"].ToString()), WandalaEnvironment.EnumToBool(Row["bots_muted"].ToString()),
                WandalaEnvironment.EnumToBool(Row["advertising_report_blocked"].ToString()), Convert.ToDouble(Row["last_change"].ToString()), Convert.ToInt32(Row["gotw_points"]),
                WandalaEnvironment.EnumToBool(Convert.ToString(Row["ignore_invites"])), Convert.ToDouble(Row["time_muted"]), Convert.ToDouble(UserInfo["trading_locked"]),
                WandalaEnvironment.EnumToBool(Row["allow_gifts"].ToString()), Convert.ToInt32(Row["friend_bar_state"]),  WandalaEnvironment.EnumToBool(Row["disable_forced_effects"].ToString()),
                WandalaEnvironment.EnumToBool(Row["allow_mimic"].ToString()), Convert.ToInt32(Row["rank_vip"]));
        }
    }
}