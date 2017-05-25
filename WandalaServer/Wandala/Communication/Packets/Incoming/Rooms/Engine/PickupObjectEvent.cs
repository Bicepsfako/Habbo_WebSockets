using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Items;
using Wandala.HabboHotel.Quests;
using Wandala.HabboHotel.Users;
using Wandala.HabboHotel.GameClients;

using Wandala.Communication.Packets.Outgoing.Inventory.Furni;
using Wandala.Database.Interfaces;

namespace Wandala.Communication.Packets.Incoming.Rooms.Engine
{
    class PickupObjectEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            int Unknown = Packet.PopInt();
            int ItemId = Packet.PopInt();

            Item Item = Room.GetRoomItemHandler().GetItem(ItemId);
            if (Item == null)
                return;

            if (Item.GetBaseItem().InteractionType == InteractionType.POSTIT)
                return;

            Boolean ItemRights = false;
            if (Item.UserID == Session.GetHabbo().Id || Room.CheckRights(Session, false))
                ItemRights = true;
            else if (Room.Group != null && Room.CheckRights(Session, false, true))//Room has a group, this user has group rights.
                ItemRights = true;
            else if (Session.GetHabbo().GetPermissions().HasRight("room_item_take"))
                ItemRights = true;

            if (ItemRights == true)
            {
                if (Item.GetBaseItem().InteractionType == InteractionType.TENT || Item.GetBaseItem().InteractionType == InteractionType.TENT_SMALL)
                    Room.RemoveTent(Item.Id);

                if (Item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
                {
                    using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE FROM `room_items_moodlight` WHERE `item_id` = '" + Item.Id + "' LIMIT 1");
                    }
                }
                else if (Item.GetBaseItem().InteractionType == InteractionType.TONER)
                {
                    using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE FROM `room_items_toner` WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                }


                if (Item.UserID == Session.GetHabbo().Id)
                {
                    Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                }
                else if (Session.GetHabbo().GetPermissions().HasRight("room_item_take"))//Staff are taking this item
                {
                    Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(false);

                }
                else//Item is being ejected.
                {
                    GameClient targetClient = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                    if (targetClient != null && targetClient.GetHabbo() != null)//Again, do we have an active client?
                    {
                        Room.GetRoomItemHandler().RemoveFurniture(targetClient, Item.Id);
                        targetClient.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        targetClient.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    }
                    else//No, query time.
                    {
                        Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                        using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                    }
                }

                WandalaEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_PICK);
            }
        }
    }
}