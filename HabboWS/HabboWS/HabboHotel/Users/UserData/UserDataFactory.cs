using HabboWS.Database.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using HabboWS.HabboHotel.Users.Authenticator;

namespace HabboWS.HabboHotel.Users.UserData
{
    public class UserDataFactory
    {
        /*public static UserData GetUserData(string SessionTicket, out byte errorCode)
        {
            int UserId;
            DataRow dUserInfo = null;
            DataTable dAchievements = null;
            DataTable dFavouriteRooms = null;
            DataTable dBadges = null;
            DataTable dFriends = null;
            DataTable dRequests = null;
            DataTable dRooms = null;
            DataTable dQuests = null;
            DataTable dRelations = null;
            DataRow UserInfo = null;

            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `id`,`username`,`rank`,`motto`,`look`,`gender`,`last_online`,`credits`,`activity_points`,`home_room`,`block_newfriends`,`hide_online`,`hide_inroom`,`vip`,`account_created`,`vip_points`,`machine_id`,`volume`,`chat_preference`,`focus_preference`, `pets_muted`,`bots_muted`,`advertising_report_blocked`,`last_change`,`gotw_points`,`ignore_invites`,`time_muted`,`allow_gifts`,`friend_bar_state`,`disable_forced_effects`,`allow_mimic`,`rank_vip` FROM `users` WHERE `auth_ticket` = @sso LIMIT 1");
                dbClient.AddParameter("sso", SessionTicket);
                dUserInfo = dbClient.GetRow();

                if (dUserInfo == null)
                {
                    errorCode = 1;
                    return null;
                }

                UserId = Convert.ToInt32(dUserInfo["id"]);
                if (HabboEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId) != null)
                {
                    errorCode = 2;
                    HabboEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId).Disconnect();
                    return null;
                }

                dbClient.SetQuery("SELECT `group`,`level`,`progress` FROM `user_achievements` WHERE `userid` = '" + UserId + "'");
                dAchievements = dbClient.GetTable();

                dbClient.SetQuery("SELECT room_id FROM user_favorites WHERE `user_id` = '" + UserId + "'");
                dFavouriteRooms = dbClient.GetTable();

                dbClient.SetQuery("SELECT `badge_id`,`badge_slot` FROM user_badges WHERE `user_id` = '" + UserId + "'");
                dBadges = dbClient.GetTable();

                dbClient.SetQuery(
                    "SELECT users.id,users.username,users.motto,users.look,users.last_online,users.hide_inroom,users.hide_online " +
                    "FROM users " +
                    "JOIN messenger_friendships " +
                    "ON users.id = messenger_friendships.user_one_id " +
                    "WHERE messenger_friendships.user_two_id = " + UserId + " " +
                    "UNION ALL " +
                    "SELECT users.id,users.username,users.motto,users.look,users.last_online,users.hide_inroom,users.hide_online " +
                    "FROM users " +
                    "JOIN messenger_friendships " +
                    "ON users.id = messenger_friendships.user_two_id " +
                    "WHERE messenger_friendships.user_one_id = " + UserId);
                dFriends = dbClient.GetTable();

                dbClient.SetQuery("SELECT messenger_requests.from_id,messenger_requests.to_id,users.username FROM users JOIN messenger_requests ON users.id = messenger_requests.from_id WHERE messenger_requests.to_id = " + UserId);
                dRequests = dbClient.GetTable();

                dbClient.SetQuery("SELECT * FROM rooms WHERE `owner` = '" + UserId + "' LIMIT 150");
                dRooms = dbClient.GetTable();

                dbClient.SetQuery("SELECT `quest_id`,`progress` FROM user_quests WHERE `user_id` = '" + UserId + "'");
                dQuests = dbClient.GetTable();

                dbClient.SetQuery("SELECT `id`,`user_id`,`target`,`type` FROM `user_relationships` WHERE `user_id` = '" + UserId + "'");
                dRelations = dbClient.GetTable();

                dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + UserId + "' LIMIT 1");
                UserInfo = dbClient.GetRow();
                if (UserInfo == null)
                {
                    dbClient.RunQuery("INSERT INTO `user_info` (`user_id`) VALUES ('" + UserId + "')");

                    dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + UserId + "' LIMIT 1");
                    UserInfo = dbClient.GetRow();
                }

                dbClient.RunQuery("UPDATE `users` SET `online` = '1', `auth_ticket` = '' WHERE `id` = '" + UserId + "' LIMIT 1");
            }
        }*/

        public static UserData GetUserData(int UserId)
        {
            DataRow dUserInfo = null;
            DataRow UserInfo = null;
            DataTable dRelations = null;
            DataTable dGroups = null;

            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `id`,`username`,`rank`,`motto`,`look`,`gender`,`last_online`,`credits`,`activity_points`,`home_room`,`block_newfriends`,`hide_online`,`hide_inroom`,`vip`,`account_created`,`vip_points`,`machine_id`,`volume`,`chat_preference`, `focus_preference`, `pets_muted`,`bots_muted`,`advertising_report_blocked`,`last_change`,`gotw_points`,`ignore_invites`,`time_muted`,`allow_gifts`,`friend_bar_state`,`disable_forced_effects`,`allow_mimic`,`rank_vip` FROM `users` WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("id", UserId);
                dUserInfo = dbClient.GetRow();

                /*HabboEnvironment.GetGame().GetClientManager().LogClonesOut(Convert.ToInt32(UserId));

                if (dUserInfo == null)
                    return null;

                if (HabboEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId) != null)
                    return null;*/


                dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + UserId + "' LIMIT 1");
                UserInfo = dbClient.GetRow();
                if (UserInfo == null)
                {
                    dbClient.RunQuery("INSERT INTO `user_info` (`user_id`) VALUES ('" + UserId + "')");

                    dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + UserId + "' LIMIT 1");
                    UserInfo = dbClient.GetRow();
                }

                dbClient.SetQuery("SELECT group_id,rank FROM group_memberships WHERE user_id=@id");
                dbClient.AddParameter("id", UserId);
                dGroups = dbClient.GetTable();

                dbClient.SetQuery("SELECT `id`,`target`,`type` FROM user_relationships WHERE user_id=@id");
                dbClient.AddParameter("id", UserId);
                dRelations = dbClient.GetTable();
            }

            //ConcurrentDictionary<string, UserAchievement> Achievements = new ConcurrentDictionary<string, UserAchievement>();
            List<int> FavouritedRooms = new List<int>();
            //List<Badge> Badges = new List<Badge>();
            //Dictionary<int, MessengerBuddy> Friends = new Dictionary<int, MessengerBuddy>();
            //Dictionary<int, MessengerRequest> FriendRequests = new Dictionary<int, MessengerRequest>();
            //List<RoomData> Rooms = new List<RoomData>();
            Dictionary<int, int> Quests = new Dictionary<int, int>();

            /*Dictionary<int, Relationship> Relationships = new Dictionary<int, Relationship>();
            foreach (DataRow Row in dRelations.Rows)
            {
                if (!Relationships.ContainsKey(Convert.ToInt32(Row["id"])))
                {
                    Relationships.Add(Convert.ToInt32(Row["target"]), new Relationship(Convert.ToInt32(Row["id"]), Convert.ToInt32(Row["target"]), Convert.ToInt32(Row["type"].ToString())));
                }
            }*/

            Habbo user = HabboFactory.GenerateHabbo(dUserInfo, UserInfo);
            return new UserData(UserId, /*Achievements,*/ FavouritedRooms, /*Badges, Friends, FriendRequests, Rooms, Quests,*/ user /*Relationships*/);
        }
    }
}
