using Alchemy.Classes;

namespace Wandala
{
    class User
    {
        UserContext context;
        public int id;
        private string username;
        private string mail;
        private int rank;
        private int credits;
        private bool online;

        public User(UserContext context)
        {
            this.context = context;
        }
    }
}
