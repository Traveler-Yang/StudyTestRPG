using Common;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class FriendManager
    {
        Character Owner;
        /// <summary>
        /// 所有好友
        /// </summary>
        List<NFriendInfo> friends;

        public FriendManager(Character owner)
        {
            this.Owner = owner;
        }

        public void GetFriendInfos(List<NFriendInfo> friends)
        {
            
        }
    }
}
