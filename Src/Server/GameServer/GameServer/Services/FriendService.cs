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

        }

        private void OnFriendList(NetConnection<NetSession> sender, FriendListResponse message)
        {

        }
    }
}
