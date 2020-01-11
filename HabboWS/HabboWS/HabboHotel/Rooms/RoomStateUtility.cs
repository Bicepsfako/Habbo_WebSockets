namespace HabboWS.HabboHotel.Rooms
{
    public static class RoomStateUtility
    {
        public static int GetRoomStatePacketNum(RoomState Access)
        {
            switch (Access)
            {
                default:
                case RoomState.OPEN:
                    return 0;

                case RoomState.DOORBELL:
                    return 1;

                case RoomState.PASSWORD:
                    return 2;

                case RoomState.INVISIBLE:
                    return 3;
            }
        }

        public static RoomState ToRoomState(string Id)
        {
            switch (Id)
            {
                default:
                case "open":
                    return RoomState.OPEN;

                case "locked":
                    return RoomState.DOORBELL;

                case "password":
                    return RoomState.PASSWORD;

                case "invisible":
                    return RoomState.INVISIBLE;
            }
        }

        public static RoomState ToRoomState(int Id)
        {
            switch (Id)
            {
                default:
                case 0:
                    return RoomState.OPEN;

                case 1:
                    return RoomState.DOORBELL;

                case 2:
                    return RoomState.PASSWORD;

                case 3:
                    return RoomState.INVISIBLE;
            }
        }
    }
}
