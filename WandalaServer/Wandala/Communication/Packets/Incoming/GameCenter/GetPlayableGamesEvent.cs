using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Outgoing.GameCenter;

namespace Wandala.Communication.Packets.Incoming.GameCenter
{
    class GetPlayableGamesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();

            Session.SendPacket(new GameAccountStatusComposer(GameId));
            Session.SendPacket(new PlayableGamesComposer(GameId));
            Session.SendPacket(new GameAchievementListComposer(Session, WandalaEnvironment.GetGame().GetAchievementManager().GetGameAchievements(GameId), GameId));
        }
    }
}