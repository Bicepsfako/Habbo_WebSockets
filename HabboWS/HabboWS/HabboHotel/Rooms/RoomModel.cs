using log4net;
using System;

namespace HabboWS.HabboHotel.Rooms
{
    public enum SquareState
    {
        OPEN = 0,
        BLOCKED = 1,
        SEAT = 2,
        POOL = 3, // Should be closed ASAP
        VIP = 4
    }

    public class RoomModel
    {
        private static readonly ILog log = LogManager.GetLogger("HabboWS.HabboHotel.Rooms.RoomModel");

        public string Id;
        public int DoorX;
        public int DoorY;
        public int DoorZ;
        public int DoorDir;
        public string Heightmap;
        public int WallHeight;

        public int MapSizeX;
        public int MapSizeY;
        public short[,] SqFloorHeight;
        public byte[,] SqSeatRot;
        public SquareState[,] SqState;

        public RoomModel(string Id, int DoorX, int DoorY, int DoorZ, int DoorDir, string Heightmap, int WallHeight)
        {
            try
            {
                this.Id = Id;
                this.DoorX = DoorX;
                this.DoorY = DoorY;
                this.DoorZ = DoorZ;
                this.DoorDir = DoorDir;
                this.Heightmap = Heightmap;
                this.WallHeight = WallHeight;

                string[] tmpHeightmap = Heightmap.Split(Convert.ToChar(13));
                this.MapSizeX = tmpHeightmap[0].Length;
                this.MapSizeY = tmpHeightmap.Length;

                SqState = new SquareState[MapSizeX, MapSizeY];
                SqFloorHeight = new short[MapSizeX, MapSizeY];
                SqSeatRot = new byte[MapSizeX, MapSizeY];

                for (int y = 0; y < MapSizeY; y++)
                {
                    string line = tmpHeightmap[y];
                    line = line.Replace("\r", "");
                    line = line.Replace("\n", "");

                    int x = 0;
                    foreach (char square in line)
                    {
                        if (square == 'x')
                        {
                            SqState[x, y] = SquareState.BLOCKED;
                        }
                        else
                        {
                            SqState[x, y] = SquareState.OPEN;
                            SqFloorHeight[x, y] = parse(square);
                        }
                        x++;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Error during loading modeldata for model " + Id);
                throw e;
            }
        }

        public static short parse(char input)
        {

            switch (input)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'a':
                    return 10;
                case 'b':
                    return 11;
                case 'c':
                    return 12;
                case 'd':
                    return 13;
                case 'e':
                    return 14;
                case 'f':
                    return 15;
                case 'g':
                    return 16;
                case 'h':
                    return 17;
                case 'i':
                    return 18;
                case 'j':
                    return 19;
                case 'k':
                    return 20;
                case 'l':
                    return 21;
                case 'm':
                    return 22;
                case 'n':
                    return 23;
                case 'o':
                    return 24;
                case 'p':
                    return 25;
                case 'q':
                    return 26;
                case 'r':
                    return 27;
                case 's':
                    return 28;
                case 't':
                    return 29;
                case 'u':
                    return 30;
                case 'v':
                    return 31;
                case 'w':
                    return 32;

                default:
                    throw new FormatException("The input was not in a correct format: input must be between (0-k)");
            }
        }

        public static byte parseByte(char input)
        {
            switch (input)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                default:
                    throw new FormatException("The input was not in a correct format: input must be a number between 0 and 9");
            }
        }

        public void Destroy()
        {
            Heightmap = null;
            SqState = null;
            SqFloorHeight = null;
            SqSeatRot = null;
        }
    }
}
