using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class FriendService : Singleton<FriendService>
    {
        public FriendService()
        {
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAdd);
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
        }

        private void OnFriendList(NetConnection<NetSession> sender, FriendListResponse message)
        {
            //Character character = sender.Session.Character;
            //sender.Session.Response.friendList = new FriendListResponse();
            //sender.Session.Response.friendList.Friends = DBService.Instance.Entities.TCharacterFriends.
        }
    }
}
