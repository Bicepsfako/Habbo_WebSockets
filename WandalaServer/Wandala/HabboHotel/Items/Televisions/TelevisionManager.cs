using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Wandala.Database.Interfaces;

using log4net;

namespace Wandala.HabboHotel.Items.Televisions
{
    public class TelevisionManager
    {
        private static readonly ILog log = LogManager.GetLogger("Wandala.HabboHotel.Items.Televisions.TelevisionManager");

        public Dictionary<int, TelevisionItem> _televisions;

        public TelevisionManager()
        {
            this._televisions =  new Dictionary<int, TelevisionItem>();

            this.Init();
        }

        public void Init()
        {
            if (this._televisions.Count > 0)
                _televisions.Clear();

            DataTable getData = null;
            using (IQueryAdapter dbClient = WandalaEnvironment.GetDatabaseManager().GetQueryReactor()) 
            {
                dbClient.SetQuery("SELECT * FROM `items_youtube` ORDER BY `id` DESC");
                getData = dbClient.GetTable();

                if (getData != null)
                {
                    foreach (DataRow Row in getData.Rows)
                    {
                        this._televisions.Add(Convert.ToInt32(Row["id"]), new TelevisionItem(Convert.ToInt32(Row["id"]), Row["youtube_id"].ToString(), Row["title"].ToString(), Row["description"].ToString(), WandalaEnvironment.EnumToBool(Row["enabled"].ToString())));
                    }
                }
            }


            log.Info("Television Items -> LOADED");
        }


        public ICollection<TelevisionItem> TelevisionList
        {
            get
            {
                return this._televisions.Values;
            }
        }

        public bool TryGet(int ItemId, out TelevisionItem TelevisionItem)
        {
            if (this._televisions.TryGetValue(ItemId, out TelevisionItem))
                return true;
            return false;
        }
    }
}
