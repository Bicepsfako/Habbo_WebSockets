using Wandala.Core;

namespace Wandala.Communication.RCON.Commands.Hotel
{
    class ReloadServerSettingsCommand : IRCONCommand
    {
        public string Description
        {
            get { return "This command is used to reload the server settings."; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public bool TryExecute(string[] parameters)
        {
            WandalaEnvironment.GetSettingsManager().Init();
            return true;
        }
    }
}