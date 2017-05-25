﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Wandala.Communication.Packets.Outgoing.Catalog;
using Wandala.Core;
using Wandala.HabboHotel.GameClients;

namespace Wandala.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class UpdateCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }

        public string Parameters
        {
            get { return "%variable%"; }
        }

        public string Description
        {
            get { return "Reload a specific part of the hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("You must inculde a thing to update, e.g. :update catalog");
                return;
            }

            string UpdateVariable = Params[1];
            switch (UpdateVariable.ToLower())
            {
                case "cata":
                case "catalog":
                case "catalogue":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_catalog' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetCatalog().Init(WandalaEnvironment.GetGame().GetItemManager());
                        WandalaEnvironment.GetGame().GetClientManager().SendPacket(new CatalogUpdatedComposer());
                        Session.SendWhisper("Catalogue successfully updated.");
                        break;
                    }

                case "items":
                case "furni":
                case "furniture":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_furni' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetItemManager().Init();
                        Session.SendWhisper("Items successfully updated.");
                        break;
                    }

                case "models":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_models"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_models' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetRoomManager().LoadModels();
                        Session.SendWhisper("Room models successfully updated.");
                        break;
                    }

                case "promotions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_promotions"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_promotions' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetLandingManager().LoadPromotions();
                        Session.SendWhisper("Landing view promotions successfully updated.");
                        break;
                    }

                case "youtube":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_youtube"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_youtube' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetTelevisionManager().Init();
                        Session.SendWhisper("Youtube televisions playlist successfully updated.");
                        break;
                    }

                case "filter":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_filter"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_filter' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetChatManager().GetFilter().Init();
                        Session.SendWhisper("Filter definitions successfully updated.");
                        break;
                    }

                case "navigator":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_navigator"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_navigator' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetNavigator().Init();
                        Session.SendWhisper("Navigator items successfully updated.");
                        break;
                    }

                case "ranks":
                case "rights":
                case "permissions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rights"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_rights' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetPermissionManager().Init();

                        foreach (GameClient Client in WandalaEnvironment.GetGame().GetClientManager().GetClients.ToList())
                        {
                            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetPermissions() == null)
                                continue;

                            Client.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                        }

                        Session.SendWhisper("Rank definitions successfully updated.");
                        break;
                    }

                case "config":
                case "settings":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_configuration"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_configuration' permission.");
                            break;
                        }

                        WandalaEnvironment.GetSettingsManager().Init();
                        Session.SendWhisper("Server configuration successfully updated.");
                        break;
                    }

                case "bans":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bans"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_bans' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetModerationManager().ReCacheBans();
                        Session.SendWhisper("Ban cache re-loaded.");
                        break;
                    }

                case "quests":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_quests"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_quests' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetQuestManager().Init();
                        Session.SendWhisper("Quest definitions successfully updated.");
                        break;
                    }

                case "achievements":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_achievements"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_achievements' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetAchievementManager().LoadAchievements();
                        Session.SendWhisper("Achievement definitions bans successfully updated.");
                        break;
                    }

                case "moderation":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_moderation"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_moderation' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetModerationManager().Init();
                        WandalaEnvironment.GetGame().GetClientManager().ModAlert("Moderation presets have been updated. Please reload the client to view the new presets.");

                        Session.SendWhisper("Moderation configuration successfully updated.");
                        break;
                    }

                case "vouchers":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_vouchers"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_vouchers' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetCatalog().GetVoucherManager().Init();
                        Session.SendWhisper("Catalogue vouche cache successfully updated.");
                        break;
                    }

                case "gc":
                case "games":
                case "gamecenter":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_game_center"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_game_center' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetGameDataManager().Init();
                        Session.SendWhisper("Game Center cache successfully updated.");
                        break;
                    }

                case "pet_locale":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_pet_locale"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_pet_locale' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetChatManager().GetPetLocale().Init();
                        Session.SendWhisper("Pet locale cache successfully updated.");
                        break;
                    }

                case "locale":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_locale"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_locale' permission.");
                            break;
                        }

                        WandalaEnvironment.GetLanguageManager().Init();
                        Session.SendWhisper("Locale cache successfully updated.");
                        break;
                    }

                case "mutant":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_anti_mutant"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_anti_mutant' permission.");
                            break;
                        }

                        WandalaEnvironment.GetFigureManager().Init();
                        Session.SendWhisper("FigureData manager successfully reloaded.");
                        break;
                    }

                case "bots":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bots"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_bots' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetBotManager().Init();
                        Session.SendWhisper("Bot managaer successfully reloaded.");
                        break;
                    }

                case "rewards":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rewards"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_rewards' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetRewardManager().Reload();
                        Session.SendWhisper("Rewards managaer successfully reloaded.");
                        break;
                    }

                case "chat_styles":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_chat_styles"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_chat_styles' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetChatManager().GetChatStyles().Init();
                        Session.SendWhisper("Chat Styles successfully reloaded.");
                        break;
                    }

                case "badge_definitions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_badge_definitions"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_badge_definitions' permission.");
                            break;
                        }

                        WandalaEnvironment.GetGame().GetBadgeManager().Init();
                        Session.SendWhisper("Badge definitions successfully reloaded.");
                        break;
                    }

                default:
                    Session.SendWhisper("'" + UpdateVariable + "' is not a valid thing to reload.");
                    break;
            }
        }
    }
}
