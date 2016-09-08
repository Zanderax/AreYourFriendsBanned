using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreYourFriendsBanned.Data_Structures.FriendJson
{
    public class Friend
    {
        public string steamid { get; set; }
        public string relationship { get; set; }
        public int friend_since { get; set; }
    }

    public class Friendslist
    {
        public List<Friend> friends { get; set; }
    }

    public class FriendJson
    {
        public Friendslist friendslist { get; set; }
    }
}
