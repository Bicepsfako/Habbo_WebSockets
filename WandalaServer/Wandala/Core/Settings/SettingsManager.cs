using System.Data;
using System.Collections.Generic;
using Wandala.Database.Interfaces;
using log4net;

namespace Wandala.Core.Settings
{
    public class SettingsManager
    {
        private Dictionary<string, string> _settings = new Dictionary<string, string>();
        private static readonly ILog log = LogManager.GetLogger("Wandala.Core.Settings.SettingsManager");

        public SettingsManager()
        {
            _settings = new Dictionary<string, string>();
        }

        public void Init()
        {
            if (_settings.Count > 0)
                _settings.Clear();

            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_settings`");
                DataTable Table = dbClient.GetTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        this._settings.Add(Row["key"].ToString().ToLower(), Row["value"].ToString().ToLower());
                    }
                }
            }

            log.Info("Loaded " + _settings.Count + " server settings.");
        }

        public string TryGetValue(string value)
        {
            return this._settings.ContainsKey(value) ? _settings[value] : "0";
        }
    }
}
