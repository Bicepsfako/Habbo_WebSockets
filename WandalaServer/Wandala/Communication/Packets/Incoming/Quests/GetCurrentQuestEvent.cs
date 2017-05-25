using Wandala.HabboHotel.Quests;
using Wandala.Database.Interfaces;
using Wandala.Communication.Packets.Outgoing.Quests;

namespace Wandala.Communication.Packets.Incoming.Quests
{
    class GetCurrentQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Quest UserQuest = WandalaEnvironment.GetGame().GetQuestManager().GetQuest(Session.GetHabbo().QuestLastCompleted);
            Quest NextQuest = WandalaEnvironment.GetGame().GetQuestManager().GetNextQuestInSeries(UserQuest.Category, UserQuest.Number + 1);

            if (NextQuest == null)
                return;

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("REPLACE INTO `user_quests`(`user_id`,`quest_id`) VALUES (" + Session.GetHabbo().Id + ", " + NextQuest.Id + ")");
                dbClient.RunQuery("UPDATE `user_stats` SET `quest_id` = '" + NextQuest.Id + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            Session.GetHabbo().GetStats().QuestID = NextQuest.Id;
            WandalaEnvironment.GetGame().GetQuestManager().GetList(Session, null);
            Session.SendPacket(new QuestStartedComposer(Session, NextQuest));
        }
    }
}
