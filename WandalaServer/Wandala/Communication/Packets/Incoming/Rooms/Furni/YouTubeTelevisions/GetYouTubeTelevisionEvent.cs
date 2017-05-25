using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Items;
using Wandala.HabboHotel.Items.Televisions;
using Wandala.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Wandala.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
    class GetYouTubeTelevisionEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            int ItemId = Packet.PopInt();
            ICollection<TelevisionItem> Videos = WandalaEnvironment.GetGame().GetTelevisionManager().TelevisionList;
            if (Videos.Count == 0)
            {
                Session.SendNotification("Oh, it looks like the hotel manager haven't added any videos for you to watch! :(");
                return;
            }

            Dictionary<int, TelevisionItem> dict = WandalaEnvironment.GetGame().GetTelevisionManager()._televisions;
            foreach (TelevisionItem value in RandomValues(dict).Take(1))
            {
                Session.SendPacket(new GetYouTubeVideoComposer(ItemId, value.YouTubeId));
            }

            Session.SendPacket(new GetYouTubePlaylistComposer(ItemId, Videos));
        }

        public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TValue> values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }
    }
}