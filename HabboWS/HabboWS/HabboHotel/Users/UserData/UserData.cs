using System.Collections.Generic;

namespace HabboWS.HabboHotel.Users.UserData
{
    public class UserData
    {
        public int userID;
        public Habbo user;
        public List<int> favouritedRooms;
        //public List<RoomData> rooms;

        public UserData(int userID, List<int> favouritedRooms, /*List<RoomData> rooms,*/ Habbo user)
        {
            this.userID = userID;
            //this.rooms = rooms;
            this.user = user;
        }
    }
}
