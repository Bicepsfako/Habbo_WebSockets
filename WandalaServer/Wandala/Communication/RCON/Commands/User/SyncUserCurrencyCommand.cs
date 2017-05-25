using System;
using Wandala.HabboHotel.GameClients;
using Wandala.Database.Interfaces;
using Wandala.Communication.Packets.Outgoing.Inventory.Purse;

namespace Wandala.Communication.RCON.Commands.User
{
    class SyncUserCurrencyCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to sync a users specified currency to the database."; }
        }

        public string Parameters
        {
            get { return "%userId% %currency%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = WandalaEnvironment.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the currency type
            if (string.IsNullOrEmpty(Convert.ToString(parameters[1])))
                return false;

            string currency = Convert.ToString(parameters[1]);

            switch (currency)
            {
                default:
                    return false;

                case "coins":
                case "credits":
                    {
                        using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `users` SET `credits` = @credits WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("credits", client.GetHabbo().Credits);
                            dbClient.AddParameter("id", userId);
                            dbClient.RunQuery();
                        }
                        break;
                    }

                case "pixels":
                case "duckets":
                    {
                        using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `users` SET `activity_points` = @duckets WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("duckets", client.GetHabbo().Duckets);
                            dbClient.AddParameter("id", userId);
                            dbClient.RunQuery();
                        }
                        break;
                    }

                case "diamonds":
                    {
                        using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `users` SET `vip_points` = @diamonds WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("diamonds", client.GetHabbo().Diamonds);
                            dbClient.AddParameter("id", userId);
                            dbClient.RunQuery();
                        }
                        break;
                    }

                case "gotw":
                    {
                        using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `users` SET `gotw_points` = @gotw WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("gotw", client.GetHabbo().GOTWPoints);
                            dbClient.AddParameter("id", userId);
                            dbClient.RunQuery();
                        }
                        break;
                    }
            }
            return true;
        }
    }
}