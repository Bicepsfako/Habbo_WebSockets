using System;
using System.Linq;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Quests;

using Wandala.Communication.Packets.Outgoing.Rooms.Engine;
using Wandala.Database.Interfaces;
using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Wandala.Communication.Packets.Incoming.Users
{
    class UpdateFigureDataEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            string Gender = Packet.PopString().ToUpper();
            string Look = WandalaEnvironment.GetFigureManager().ProcessFigure(Packet.PopString(), Gender, Session.GetHabbo().GetClothing().GetClothingParts, true);

            if (Look == Session.GetHabbo().Look)
                return;

            if ((DateTime.Now - Session.GetHabbo().LastClothingUpdateTime).TotalSeconds <= 2.0)
            {
                Session.GetHabbo().ClothingUpdateWarnings += 1;
                if (Session.GetHabbo().ClothingUpdateWarnings >= 25)
                    Session.GetHabbo().SessionClothingBlocked = true;
                return;
            }

            if (Session.GetHabbo().SessionClothingBlocked)
                return;

            Session.GetHabbo().LastClothingUpdateTime = DateTime.Now;

            string[] AllowedGenders = { "M", "F" };
            if (!AllowedGenders.Contains(Gender))
            {
                Session.SendPacket(new BroadcastMessageAlertComposer("Sorry, you chose an invalid gender."));
                return;
            }

            WandalaEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.PROFILE_CHANGE_LOOK);

            Session.GetHabbo().Look = WandalaEnvironment.FilterFigure(Look);
            Session.GetHabbo().Gender = Gender.ToLower();

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `look` = @look, `gender` = @gender WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("look", Look);
                dbClient.AddParameter("gender", Gender);
                dbClient.RunQuery();
            }

            WandalaEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AvatarLooks", 1);
            Session.SendPacket(new AvatarAspectUpdateComposer(Look, Gender));
            if (Session.GetHabbo().Look.Contains("ha-1006"))
                WandalaEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.WEAR_HAT);

            if (Session.GetHabbo().InRoom)
            {
                RoomUser RoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (RoomUser != null)
                {
                    Session.SendPacket(new UserChangeComposer(RoomUser, true));
                    Session.GetHabbo().CurrentRoom.SendPacket(new UserChangeComposer(RoomUser, false));
                }
            }
        }
    }
}
