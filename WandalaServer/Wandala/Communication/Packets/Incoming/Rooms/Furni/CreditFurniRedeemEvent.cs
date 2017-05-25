using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Items;
using Wandala.Communication.Packets.Outgoing.Inventory.Purse;
using Wandala.Communication.Packets.Outgoing.Inventory.Furni;

using Wandala.Database.Interfaces;


namespace Wandala.Communication.Packets.Incoming.Rooms.Furni
{
    class CreditFurniRedeemEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = null;
            if (!WandalaEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;
            
            if (WandalaEnvironment.GetSettingsManager().TryGetValue("room.item.exchangeables.enabled") != "1")
            {
                Session.SendNotification("The hotel managers have temporarilly disabled exchanging!");
                return;
            }

            Item Exchange = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Exchange == null)
                return;

            if (Exchange.Data.InteractionType != InteractionType.EXCHANGE)
                return;


            int Value = Exchange.Data.BehaviourData;

            if (Value > 0)
            {
                Session.GetHabbo().Credits += Value;
                Session.SendPacket(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @exchangeId LIMIT 1");
                dbClient.AddParameter("exchangeId", Exchange.Id);
                dbClient.RunQuery();
            }

            Session.SendPacket(new FurniListUpdateComposer());
            Room.GetRoomItemHandler().RemoveFurniture(null, Exchange.Id, false);
            Session.GetHabbo().GetInventoryComponent().RemoveItem(Exchange.Id);

        }
    }
}
