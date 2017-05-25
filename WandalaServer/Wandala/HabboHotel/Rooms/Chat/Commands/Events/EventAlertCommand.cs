using Wandala.Communication.Packets.Outgoing.Moderation;
using Wandala.HabboHotel.GameClients;
using System;
namespace Wandala.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class EventAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_event_alert";
            }
        }
        public string Parameters
        {
            get
            {
                return "";
            }
        }
        public string Description
        {
            get
            {
                return "Send a hotel alert for your event!";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session != null)
            {
                if (Room != null)
                {
                    if (Params.Length != 1)
                    {
                        Session.SendWhisper("Invalid command! :eventalert", 0);
                    }
                    else if (!WandalaEnvironment.Event)
                    {
                        WandalaEnvironment.GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(":follow " + Session.GetHabbo().Username + " for events! win prizes!\r\n- " + Session.GetHabbo().Username, ""), "");
                        WandalaEnvironment.lastEvent = DateTime.Now;
                        WandalaEnvironment.Event = true;
                    }
                    else
                    {
                        TimeSpan timeSpan = DateTime.Now - WandalaEnvironment.lastEvent;
                        if (timeSpan.Hours >= 1)
                        {
                            WandalaEnvironment.GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(":follow " + Session.GetHabbo().Username + " for events! win prizes!\r\n- " + Session.GetHabbo().Username, ""), "");
                            WandalaEnvironment.lastEvent = DateTime.Now;
                        }
                        else
                        {
                            int num = checked(60 - timeSpan.Minutes);
                            Session.SendWhisper("Event Cooldown! " + num + " minutes left until another event can be hosted.", 0);
                        }
                    }
                }
            }
        }
    }
}
