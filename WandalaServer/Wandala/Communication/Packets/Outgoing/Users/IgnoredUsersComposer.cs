using System.Collections.Generic;

namespace Wandala.Communication.Packets.Outgoing.Users
{
    public class IgnoredUsersComposer : ServerPacket
    {
        public IgnoredUsersComposer(List<string> ignoredUsers)
            : base(ServerPacketHeader.IgnoredUsersMessageComposer)
        {
            base.WriteInteger(ignoredUsers.Count);
            foreach (string Username in ignoredUsers)
            {
                base.WriteString(Username);
            }
        }
    }
}
