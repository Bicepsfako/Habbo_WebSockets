using Wandala.Communication.Packets.Outgoing.Catalog;

namespace Wandala.Communication.RCON.Commands.Hotel
{
    class ReloadCatalogCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to reload the catalog."; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public bool TryExecute(string[] parameters)
        {
            WandalaEnvironment.GetGame().GetCatalog().Init(WandalaEnvironment.GetGame().GetItemManager());
            WandalaEnvironment.GetGame().GetClientManager().SendPacket(new CatalogUpdatedComposer());
            return true;
        }
    }
}