using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Sound;
using Wandala.Database.Interfaces;


namespace Wandala.Communication.Packets.Incoming.Users
{
    class SetUserFocusPreferenceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            bool FocusPreference = Packet.PopBoolean();

            Session.GetHabbo().FocusPreference = FocusPreference;
            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `focus_preference` = @focusPreference WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("focusPreference", WandalaEnvironment.BoolToEnum(FocusPreference));
                dbClient.RunQuery();
            }
        }
    }
}
