using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Wandala.Utilities;
using Wandala.Core;
using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Quests;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Rooms.Engine;

using Wandala.Database.Interfaces;


namespace Wandala.Communication.Packets.Incoming.Rooms.Avatar
{
    class ChangeMottoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendNotification("Oops, you're currently muted - you cannot change your motto.");
                return;
            }

            if ((DateTime.Now - Session.GetHabbo().LastMottoUpdateTime).TotalSeconds <= 2.0)
            {
                Session.GetHabbo().MottoUpdateWarnings += 1;
                if (Session.GetHabbo().MottoUpdateWarnings >= 25)
                    Session.GetHabbo().SessionMottoBlocked = true;
                return;
            }

            if (Session.GetHabbo().SessionMottoBlocked)
                return;

            Session.GetHabbo().LastMottoUpdateTime = DateTime.Now;

            string newMotto = StringCharFilter.Escape(Packet.PopString().Trim());

            if (newMotto.Length > 38)
                newMotto = newMotto.Substring(0, 38);

            if (newMotto == Session.GetHabbo().Motto)
                return;

            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override"))
                newMotto = WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(newMotto);

            Session.GetHabbo().Motto = newMotto;

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `motto` = @motto WHERE `id` = @userId LIMIT 1");
                dbClient.AddParameter("userId", Session.GetHabbo().Id);
                dbClient.AddParameter("motto", newMotto);
                dbClient.RunQuery();
            }

            WandalaEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.PROFILE_CHANGE_MOTTO);
            WandalaEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_Motto", 1);

            if (Session.GetHabbo().InRoom)
            {
                Room Room = Session.GetHabbo().CurrentRoom;
                if (Room == null)
                    return;

                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (User == null || User.GetClient() == null)
                    return;

                Room.SendPacket(new UserChangeComposer(User, false));
            }
        }
    }
}
