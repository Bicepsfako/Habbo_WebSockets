using System.Collections.Concurrent;
using System.Collections.Generic;
using Wandala.HabboHotel.Achievements;
using Wandala.HabboHotel.Rooms;
using Wandala.HabboHotel.Users.Badges;
using Wandala.HabboHotel.Users.Messenger;
using Wandala.HabboHotel.Users.Relationships;

namespace Wandala.HabboHotel.Users.UserData
{
    public class UserData
    {
        public int userID;
        public Habbo user;

        public Dictionary<int, Relationship> Relations;
        public ConcurrentDictionary<string, UserAchievement> achievements;
        public List<Badge> badges;
        public List<int> favouritedRooms;
        public Dictionary<int, MessengerRequest> requests;
        public Dictionary<int, MessengerBuddy> friends;
        public Dictionary<int, int> quests;
        public List<RoomData> rooms;

        public UserData(int userID, ConcurrentDictionary<string, UserAchievement> achievements, List<int> favouritedRooms,
            List<Badge> badges, Dictionary<int, MessengerBuddy> friends, Dictionary<int, MessengerRequest> requests, List<RoomData> rooms, Dictionary<int, int> quests, Habbo user, 
            Dictionary<int, Relationship> Relations)
        {
            this.userID = userID;
            this.achievements = achievements;
            this.favouritedRooms = favouritedRooms;
            this.badges = badges;
            this.friends = friends;
            this.requests = requests;
            this.rooms = rooms;
            this.quests = quests;
            this.user = user;
            this.Relations = Relations;
        }
    }
}