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
        /// <summary>
        /// 所有者
        /// </summary>
        Character Owner;
        /// <summary>
        /// 好友列表
        /// </summary>
        public List<NFriendInfo> friends;
        /// <summary>
        /// 是否变化
        /// </summary>
        bool friendChanged = false;

        public FriendManager(Character owner)
        {
            this.Owner = owner;
        }

        public void GetFriendInfos(List<NFriendInfo> friends)
        {
            foreach (var f in this.friends)
            {
                friends.Add(f);
            }
        }
    }
}
