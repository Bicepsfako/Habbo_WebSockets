using HabboWS.Database.Interfaces;
using log4net;
using System.Collections.Generic;
using System.Data;

namespace HabboWS.Core.Language
{
    public class LanguageManager
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        private static readonly ILog log = LogManager.GetLogger("Wandala.Core.Language.LanguageManager");

        public LanguageManager()
        {
            this._values = new Dictionary<string, string>();
        }

        public void Init()
        {
            if (this._values.Count > 0)
                this._values.Clear();

            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_locale`");
                DataTable Table = dbClient.GetTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                        _values.Add(Row["key"].ToString(), Row["value"].ToString());
                }
            }

            log.Info("Language Manager > LOADED");
        }

        public string TryGetValue(string value)
        {
            return _values.ContainsKey(value) ? _values[value] : "No language locale found for [" + value + "]";
        }
    }
}
