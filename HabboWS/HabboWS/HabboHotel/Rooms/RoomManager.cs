using HabboWS.Database.Interfaces;
using HabboWS.HabboHotel.GameClients;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HabboWS.HabboHotel.Rooms
{
    public class RoomManager
    {
        private static readonly ILog log = LogManager.GetLogger("HabboWS.HabboHotel.Rooms.RoomManager");

        private Dictionary<string, RoomModel> _roomModels;

        private ConcurrentDictionary<int, Room> _rooms;
        private ConcurrentDictionary<int, RoomData> _loadedRoomData;

        public RoomManager()
        {
            this._roomModels = new Dictionary<string, RoomModel>();

            this._rooms = new ConcurrentDictionary<int, Room>();
            this._loadedRoomData = new ConcurrentDictionary<int, RoomData>();

            this.LoadModels();

            log.Info("Room Manager -> LOADED");
        }

        public void LoadModels()
        {
            if (this._roomModels.Count > 0)
                _roomModels.Clear();

            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id,door_x,door_y,door_z,door_dir,heightmap,public_items,club_only,poolmap,`wall_height` FROM `room_models` WHERE `custom` = '0'");
                DataTable Data = dbClient.GetTable();

                if (Data == null)
                    return;

                foreach (DataRow Row in Data.Rows)
                {
                    string Modelname = Convert.ToString(Row["id"]);
                    string staticFurniture = Convert.ToString(Row["public_items"]);

                    _roomModels.Add(Modelname, new RoomModel(Modelname, Convert.ToInt32(Row["door_x"]), Convert.ToInt32(Row["door_y"]),
                                                             Convert.ToInt32(Row["door_z"]), Convert.ToInt32(Row["door_dir"]),
                                                             Convert.ToString(Row["heightmap"]), Convert.ToInt32(Row["wall_height"])));
                }
            }

            log.Info("Room Models -> LOADED");
        }

        public void LoadModel(string Id)
        {
            DataRow Row = null;
            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id,door_x,door_y,door_z,door_dir,heightmap,public_items,club_only,poolmap,`wall_height` FROM `room_models` WHERE `custom` = '1' AND `id` = '" + Id + "' LIMIT 1");
                Row = dbClient.GetRow();

                if (Row == null)
                    return;

                string Modelname = Convert.ToString(Row["id"]);
                if (!this._roomModels.ContainsKey(Id))
                {
                    this._roomModels.Add(Modelname, new RoomModel(Modelname, Convert.ToInt32(Row["door_x"]), Convert.ToInt32(Row["door_y"]), Convert.ToInt32(Row["door_z"]), Convert.ToInt32(Row["door_dir"]),
                      Convert.ToString(Row["heightmap"]), Convert.ToInt32(Row["wall_height"])));
                }
            }
        }

        public void ReloadModel(string Id)
        {
            if (!this._roomModels.ContainsKey(Id))
            {
                this.LoadModel(Id);
                return;
            }

            this._roomModels.Remove(Id);
            this.LoadModel(Id);
        }

        public RoomModel GetModel(string Model)
        {
            if (_roomModels.ContainsKey(Model))
                return (RoomModel)_roomModels[Model];

            return null;
        }

        public ICollection<Room> GetRooms()
        {
            return this._rooms.Values;
        }

        public RoomData GenerateRoomData(int RoomId)
        {
            if (_loadedRoomData.ContainsKey(RoomId))
                return (RoomData)_loadedRoomData[RoomId];

            RoomData Data = new RoomData();

            Room Room;

            if (TryGetRoom(RoomId, out Room))
                return Room.RoomData;

            DataRow Row = null;
            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM rooms WHERE id = " + RoomId + " LIMIT 1");
                Row = dbClient.GetRow();
            }

            if (Row == null)
                return null;

            Data.Fill(Row);

            if (!_loadedRoomData.ContainsKey(RoomId))
                _loadedRoomData.TryAdd(RoomId, Data);

            return Data;
        }

        public bool TryGetRoom(int RoomId, out Room Room)
        {
            return this._rooms.TryGetValue(RoomId, out Room);
        }

        public RoomData CreateRoom(GameClient Session, string Name, string Description, string Model, int Category, int MaxVisitors, int TradeSettings,
            string wallpaper = "0.0", string floor = "0.0", string landscape = "0.0", int wallthick = 0, int floorthick = 0)
        {
            if (!_roomModels.ContainsKey(Model))
            {
                Session.SendPacket(HabboEnvironment.GetLanguageManager().TryGetValue("room.creation.model.not_found"));
                return null;
            }

            if (Name.Length < 3)
            {
                Session.SendPacket(HabboEnvironment.GetLanguageManager().TryGetValue("room.creation.name.too_short"));
                return null;
            }

            int RoomId = 0;

            using (IQueryAdapter dbClient = HabboEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `rooms` (`roomtype`,`caption`,`description`,`owner`,`model_name`,`category`,`users_max`,`trade_settings`,`wallpaper`,`floor`,`landscape`,`floorthick`,`wallthick`) VALUES ('private',@caption,@description,@UserId,@model,@category,@usersmax,@tradesettings,@wallpaper,@floor,@landscape,@floorthick,@wallthick)");
                dbClient.AddParameter("caption", Name);
                dbClient.AddParameter("description", Description);
                dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                dbClient.AddParameter("model", Model);
                dbClient.AddParameter("category", Category);
                dbClient.AddParameter("usersmax", MaxVisitors);
                dbClient.AddParameter("tradesettings", TradeSettings);
                dbClient.AddParameter("wallpaper", wallpaper);
                dbClient.AddParameter("floor", floor);
                dbClient.AddParameter("landscape", landscape);
                dbClient.AddParameter("floorthick", floorthick);
                dbClient.AddParameter("wallthick", wallthick);

                RoomId = Convert.ToInt32(dbClient.InsertQuery());
            }

            RoomData newRoomData = GenerateRoomData(RoomId);
            Session.GetHabbo().Rooms.Add(newRoomData);
            return newRoomData;
        }

        public void UnloadRoom(Room Room, bool RemoveData = false)
        {
            if (Room == null)
                return;

            Room room = null;
            if (this._rooms.TryRemove(Room.Id, out room))
            {
                //Room.Dispose();

                if (RemoveData)
                {
                    RoomData Data = null;
                    this._loadedRoomData.TryRemove(Room.Id, out Data);
                }
            }
            //Logging.WriteLine("[RoomMgr] Unloaded room: \"" + Room.Name + "\" (ID: " + Room.RoomId + ")");
        }

        public void Dispose()
        {
            int length = _rooms.Count;
            int i = 0;
            foreach (Room Room in this._rooms.Values.ToList())
            {
                if (Room == null)
                    continue;

                HabboEnvironment.GetGame().GetRoomManager().UnloadRoom(Room);
                Console.Clear();
                log.Info("<<- SERVER SHUTDOWN ->> ROOM ITEM SAVE: " + String.Format("{0:0.##}", ((double)i / length) * 100) + "%");
                i++;
            }
            log.Info("Done disposing rooms!");
        }
    }
}