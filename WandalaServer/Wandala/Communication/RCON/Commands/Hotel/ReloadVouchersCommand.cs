namespace Wandala.Communication.RCON.Commands.Hotel
{
    class ReloadVouchersCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to reload the voucher manager."; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public bool TryExecute(string[] parameters)
        {
            WandalaEnvironment.GetGame().GetCatalog().GetVoucherManager().Init();

            return true;
        }
    }
}