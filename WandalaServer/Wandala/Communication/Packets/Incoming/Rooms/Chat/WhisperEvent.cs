using System;
using System.Collections.Generic;
using Wandala.Communication.Packets.Outgoing.Rooms.Chat;
using Wandala.Core;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Quests;
using Wandala.HabboHotel.Rooms;
using Wandala.Communication.Packets.Incoming;
using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.Utilities;
using Wandala.HabboHotel.Rooms.Chat.Styles;
using Wandala.HabboHotel.Rooms.Chat.Logs;

namespace Wandala.Communication.Packets.Incoming.Rooms.Chat
{
    public class WhisperEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool") && Room.CheckMute(Session))
            {
                Session.SendWhisper("Oops, you're currently muted.");
                return;
            }

            if (WandalaEnvironment.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
                return;

            string Params = Packet.PopString();
            string ToUser = Params.Split(' ')[0];
            string Message = Params.Substring(ToUser.Length + 1);
            int Colour = Packet.PopInt();

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            RoomUser User2 = Room.GetRoomUserManager().GetRoomUserByHabbo(ToUser);
            if (User2 == null)
                return;

            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendPacket(new MutedComposer(Session.GetHabbo().TimeMuted));
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override"))
                Message = WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckMessage(Message);

            ChatStyle Style = null;
            if (!WandalaEnvironment.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
                Colour = 0;

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

            if (!User2.GetClient().GetHabbo().ReceiveWhispers && !Session.GetHabbo().GetPermissions().HasRight("room_whisper_override"))
            {
                Session.SendWhisper("Oops, this user has their whispers disabled!");
                return;
            }
            
            WandalaEnvironment.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, "<Whisper to " + ToUser + ">: " + Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            if (WandalaEnvironment.GetGame().GetChatManager().GetFilter().CheckBannedWords(Message))
            {
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= (Convert.ToInt32(WandalaEnvironment.GetSettingsManager().TryGetValue("room.chat.filter.banned_phrases.chances"))))
                {
                    WandalaEnvironment.GetGame().GetModerationManager().BanUser("System", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Spamming banned phrases (" + Message + ")", (WandalaEnvironment.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                Session.SendPacket(new WhisperComposer(User.VirtualId, Message, 0, User.LastBubble));
                return;
            }


            WandalaEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT);

            User.UnIdle();
            User.GetClient().SendPacket(new WhisperComposer(User.VirtualId, Message, 0, User.LastBubble));

            if (User2 != null && !User2.IsBot && User2.UserId != User.UserId)
            {
                if (!User2.GetClient().GetHabbo().GetIgnores().IgnoredUserIds().Contains(Session.GetHabbo().Id))
                {
                    User2.GetClient().SendPacket(new WhisperComposer(User.VirtualId, Message, 0, User.LastBubble));
                }
            }
 
            List<RoomUser> ToNotify = Room.GetRoomUserManager().GetRoomUserByRank(2);
            if (ToNotify.Count > 0)
            {
                foreach (RoomUser user in ToNotify)
                {
                    if (user != null && user.HabboId != User2.HabboId && user.HabboId != User.HabboId)
                    {
                        if (user.GetClient() != null && user.GetClient().GetHabbo() != null && !user.GetClient().GetHabbo().IgnorePublicWhispers)
                        {
                            user.GetClient().SendPacket(new WhisperComposer(User.VirtualId, "[Whisper to " + ToUser + "] " + Message, 0, User.LastBubble));
                        }
                    }
                }
            }
        }
    }
}