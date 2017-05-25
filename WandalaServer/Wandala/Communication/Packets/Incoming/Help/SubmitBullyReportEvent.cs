﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.HabboHotel.GameClients;
using Wandala.Communication.Packets.Outgoing.Help;

namespace Wandala.Communication.Packets.Incoming.Help
{
    class SubmitBullyReportEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            //0 = sent, 1 = blocked, 2 = no chat, 3 = already reported.
            if (Session == null)
                return;

            int UserId = Packet.PopInt();
            if (UserId == Session.GetHabbo().Id)//Hax
                return;

            if (Session.GetHabbo().AdvertisingReportedBlocked)
            {
                Session.SendPacket(new SubmitBullyReportComposer(1));//This user is blocked from reporting.
                return;
            }

            GameClient Client = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(Convert.ToInt32(UserId));
            if (Client == null)
            {
                Session.SendPacket(new SubmitBullyReportComposer(0));//Just say it's sent, the user isn't found.
                return;
            }

            if (Session.GetHabbo().LastAdvertiseReport > WandalaEnvironment.GetUnixTimestamp())
            {
                Session.SendNotification("Reports can only be sent per 5 minutes!");
                return;
            }

            if (Client.GetHabbo().GetPermissions().HasRight("mod_tool"))//Reporting staff, nope!
            {
                Session.SendNotification("Sorry, you cannot report staff members via this tool.");
                return;
            }

            //This user hasn't even said a word, nope!
            if (!Client.GetHabbo().HasSpoken)
            {
                Session.SendPacket(new SubmitBullyReportComposer(2));
                return;
            }

            //Already reported, nope.
            if (Client.GetHabbo().AdvertisingReported && Session.GetHabbo().Rank < 2)
            {
                Session.SendPacket(new SubmitBullyReportComposer(3));
                return;
            }

            if (Session.GetHabbo().Rank <= 1)
                Session.GetHabbo().LastAdvertiseReport = WandalaEnvironment.GetUnixTimestamp() + 300;
            else
                Session.GetHabbo().LastAdvertiseReport = WandalaEnvironment.GetUnixTimestamp();

            Client.GetHabbo().AdvertisingReported = true;
            Session.SendPacket(new SubmitBullyReportComposer(0));
            //WandalaEnvironment.GetGame().GetClientManager().ModAlert("New advertising report! " + Client.GetHabbo().Username + " has been reported for advertising by " + Session.GetHabbo().Username +".");
            WandalaEnvironment.GetGame().GetClientManager().DoAdvertisingReport(Session, Client);     
            return;
        }
    }
}