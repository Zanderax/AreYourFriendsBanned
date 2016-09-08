using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using AreYourFriendsBanned.Data_Structures.BanJson;
using AreYourFriendsBanned.Data_Structures.FriendJson;
using AreYourFriendsBanned.Data_Structures.PlayerSummery;

using Newtonsoft.Json;

namespace AreYourFriendsBanned
{
    class Program
    {
        static void Main(string[] args)
        {
            string response;
            do
            {
                Console.WriteLine("Enter your 64bit steamID or q to exit.");
                Console.ReadLine();
                response = "76561198011268623";
                if (response != "q" || response != "Q")
                {
                    getFriendsList(response);
                }
            } while (response != "q" || response != "Q");
        }

        static void getFriendsList(string steamID)
        {
            WebClient client = new WebClient();

            string friendsJson = client.DownloadString(String.Format("http://api.steampowered.com/ISteamUser/GetFriendList/v0001/?key={0}&steamid={1}", "7E3C0833A9CDBDBD72C73A8D1B42875D", steamID));
            FriendJson friendslist = JsonConvert.DeserializeObject<FriendJson>(friendsJson);
            List<string> playersWithCurrentBan = new List<string>();
            foreach(Friend friend in friendslist.friendslist.friends)
            {
                string banJson = client.DownloadString(String.Format("http://api.steampowered.com/ISteamUser/GetPlayerBans/v1/?key={0}&steamids={1}", "7E3C0833A9CDBDBD72C73A8D1B42875D", friend.steamid));
                BanJson playerBan = JsonConvert.DeserializeObject<BanJson>(banJson);
                if (playerBan.players.FirstOrDefault().NumberOfVACBans > 0 || playerBan.players.FirstOrDefault().NumberOfGameBans > 0)
                {
                    playersWithCurrentBan.Add(playerBan.players.FirstOrDefault().SteamId);
                }
            }
            if(playersWithCurrentBan.Count > 0)
            {
                Console.WriteLine("The following friends of yours have bans in the past:");
            }
            else
            {
                Console.WriteLine("You have no friends with VAC bans.");
            }
            foreach (string steamId in playersWithCurrentBan)
            {
                string playerJson = client.DownloadString(String.Format("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}", "7E3C0833A9CDBDBD72C73A8D1B42875D", steamId));
                PlayerSummery playerSummery = JsonConvert.DeserializeObject<PlayerSummery>(playerJson);
                string name = playerSummery.response.players.FirstOrDefault().personaname;
                Console.WriteLine(name);
            }
            Console.WriteLine();
        }
    }
}
