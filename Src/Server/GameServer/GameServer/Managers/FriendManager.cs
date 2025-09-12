using Common;
using GameServer.Entities;
using GameServer.Services;
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

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toId"></param>
        public void AddFriend(int id, int toId)
        {
            TCharacterFriend friend = new TCharacterFriend()
            {
                UserID = toId,
                FriendID = id,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            DBService.Instance.Entities.TCharacterFriend.Add(friend);
        }
    }
}
