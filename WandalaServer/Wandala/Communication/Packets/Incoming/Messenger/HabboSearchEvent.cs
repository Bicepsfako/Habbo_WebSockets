using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Utilities;
using Wandala.HabboHotel.Users.Messenger;

using Wandala.Communication.Packets.Outgoing.Messenger;

namespace Wandala.Communication.Packets.Incoming.Messenger
{
    class HabboSearchEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            string Query = StringCharFilter.Escape(Packet.PopString().Replace("%", ""));
            if (Query.Length < 1 || Query.Length > 100)
                return;

            List<SearchResult> Friends = new List<SearchResult>();
            List<SearchResult> OthersUsers = new List<SearchResult>();

            List<SearchResult> Results = SearchResultFactory.GetSearchResult(Query);
            foreach (SearchResult Result in Results.ToList())
            {
                if (Session.GetHabbo().GetMessenger().FriendshipExists(Result.UserId))
                    Friends.Add(Result);
                else
                    OthersUsers.Add(Result);
            }

            Session.SendPacket(new HabboSearchResultComposer(Friends, OthersUsers));
        }
    }
}