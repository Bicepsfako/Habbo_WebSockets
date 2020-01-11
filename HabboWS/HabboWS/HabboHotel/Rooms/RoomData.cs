using HabboWS.HabboHotel.Users;
using System;
using System.Data;

namespace HabboWS.HabboHotel.Rooms
{
    public class RoomData
	{
        public int Id;
        public string Type;
        public string Name;
        public int OwnerId;
        public Habbo Owner;
        public string Description;
        public RoomState State;
        public int UsersMax;
        public int UsersNow;
        public string ModelName;
        private RoomModel mModel;

        public void Fill(DataRow Row)
        {
            this.Id = Convert.ToInt32(Row["id"]);
            this.Name = Convert.ToString(Row["caption"]);
            this.Description = Convert.ToString(Row["description"]);
            this.Type = Convert.ToString(Row["roomtype"]);
            this.OwnerId = Convert.ToInt32(Row["owner"]);
            this.Owner = HabboEnvironment.GetOnlineUser(OwnerId);

            this.State = RoomStateUtility.ToRoomState(Row["state"].ToString().ToLower());

            if (!string.IsNullOrEmpty(Row["users_now"].ToString()))
                UsersNow = Convert.ToInt32(Row["users_now"]);
            else
                UsersNow = 0;
            UsersMax = Convert.ToInt32(Row["users_max"]);
            ModelName = Convert.ToString(Row["model_name"]);

            mModel = HabboEnvironment.GetGame().GetRoomManager().GetModel(ModelName);
        }

        public RoomModel Model
        {
            get
            {
                if (mModel == null)
                    mModel = HabboEnvironment.GetGame().GetRoomManager().GetModel(ModelName);
                return mModel;
            }
        }
    }
}
