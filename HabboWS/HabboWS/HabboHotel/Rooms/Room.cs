using System;

namespace HabboWS.HabboHotel.Rooms
{
    public class Room : RoomData
    {
        private RoomData _roomData;

        public Room(RoomData Data)
        {
            this._roomData = Data;

            this.Id = Data.Id;
            this.Type = Data.Type;
            this.Name = Data.Name;
            this.OwnerId = Data.OwnerId;
            this.Owner = HabboEnvironment.GetOnlineUser(OwnerId);
            this.Description = Data.Description;
            this.State = Data.State;
            this.UsersMax = Data.UsersMax;
            this.UsersNow = 0;
            this.ModelName = Data.ModelName;
        }

        public RoomData RoomData
        {
            get { return _roomData; }
        }
    }
}

