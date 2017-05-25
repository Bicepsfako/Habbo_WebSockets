using System;

using Wandala.Utilities;
using Wandala.HabboHotel.Quests;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Rooms.Chat.Logs;
using Wandala.Communication.Packets.Outgoing.Rooms.Chat;
using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.HabboHotel.Rooms.Chat.Styles;

namespace Wandala.Communication.Packets.Incoming.Rooms.Chat
{
    public class ChatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            string Message = StringCharFilter.Escape(Packet.PopString());
            if (Message.Length > 100)
                Message = Message.Substring(0, 100);

            int Colour = Packet.PopInt();

            ChatStyle Style = null;
            if (!WandalaEnvironment.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
                Colour = 0;

            User.UnIdle();

            if (WandalaEnvironment.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
                return;

            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendPacket(new MutedComposer(Session.GetHabbo().TimeMuted));
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("room_ignore_mute") && Room.CheckMute(Session))
            {
                Session.SendWhisper("Oops, you're currently muted.");
                return;
            }

            User.LastBubble = Session.GetHabbo().CustomBubbleId == 0 ? Colour : Session.GetHabbo().CustomBubbleId;

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                int MuteTime;
                if (User.IncrementAndCheckFlood(out MuteTime))
                {
                    Session.SendPacket(new FloodControlComposer(MuteTime));
                    return;
                }
            }

            WandalaEnvironment.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            if (Message.StartsWith(":", StringComparison.CurrentCulture) && WandalaEnvironment.GetGame().GetChatManager().GetCommands().Parse(Session, Message))
                return;

            if (WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckBannedWords(Message))
            {
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= (Convert.ToInt32(WandalaEnvironment.GetSettingsManager().TryGetValue("room.chat.filter.banned_phrases.chances"))))
                {
                    WandalaEnvironment.GetGame().GetModerationManager().BanUser("System", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Spamming banned phrases (" + Message + ")", (WandalaEnvironment.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }

                Session.SendPacket(new ChatComposer(User.VirtualId, Message, 0, Colour));
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override"))
                Message = WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(Message);


            WandalaEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT);

            User.OnChat(User.LastBubble, Message, false);
        }
    }
}