using System;
using System.Data;

using Wandala.Communication.Packets.Incoming;
using Wandala.HabboHotel.GameClients;
using Wandala.HabboHotel.Catalog.Vouchers;



using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.Communication.Packets.Outgoing.Inventory.Purse;

using Wandala.Database.Interfaces;

namespace Wandala.Communication.Packets.Incoming.Catalog
{
    public class RedeemVoucherEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string VoucherCode = Packet.PopString().Replace("\r", "");

            Voucher Voucher = null;
            if (!WandalaEnvironment.GetGame().GetCatalog().GetVoucherManager().TryGetVoucher(VoucherCode, out Voucher))
            {
                Session.SendPacket(new VoucherRedeemErrorComposer(0));
                return;
            }

            if (Voucher.CurrentUses >= Voucher.MaxUses)
            {
                Session.SendNotification("Oops, this voucher has reached the maximum usage limit!");
                return;
            }

            DataRow GetRow = null;
            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `user_vouchers` WHERE `user_id` = @userId AND `voucher` = @Voucher LIMIT 1");
                dbClient.AddParameter("userId", Session.GetHabbo().Id);
                dbClient.AddParameter("Voucher", VoucherCode);
                GetRow = dbClient.GetRow();
            }

            if (GetRow != null)
            {
                Session.SendNotification("You've already used this voucher code, one per each user, sorry!");
                return;
            }
            else
            {
                using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `user_vouchers` (`user_id`,`voucher`) VALUES (@userId, @Voucher)");
                    dbClient.AddParameter("userId", Session.GetHabbo().Id);
                    dbClient.AddParameter("Voucher", VoucherCode);
                    dbClient.RunQuery();
                }
            }

            Voucher.UpdateUses();

            if (Voucher.Type == VoucherType.CREDIT)
            {
                Session.GetHabbo().Credits += Voucher.Value;
                Session.SendPacket(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }
            else if (Voucher.Type == VoucherType.DUCKET)
            {
                Session.GetHabbo().Duckets += Voucher.Value;
                Session.SendPacket(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Voucher.Value));
            }

            Session.SendPacket(new VoucherRedeemOkComposer());
        }
    }
}