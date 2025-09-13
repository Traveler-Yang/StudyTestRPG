using Common;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameServer.Managers
{
    class FriendManager
    {
        Character Owner;
        /// <summary>
        /// 所有好友
        /// </summary>
        List<NFriendInfo> friends = new List<NFriendInfo>();

        bool FriendIsChanged = false;

        public FriendManager(Character owner)
        {
            this.Owner = owner;
            Init();
        }

        public void Init()
        {
            friends.Clear();
            foreach (var f in Owner.TChar.Friends)
            {
                this.friends.Add(GetNFriendInfo(f));
            }
        }

        /// <summary>
        /// 将数据库中的好友信息，改为网络信息
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        private NFriendInfo GetNFriendInfo(TCharacterFriend dbfriend)
        {
            //准备返回信息，因为协议中有角色的Info信息，所以也需要new一个NCharacterInfo信息
            NFriendInfo Nfriendinfo = new NFriendInfo();
            Nfriendinfo.Info = new NCharacterInfo();
            var character = CharacterManager.Instance.GetCharacter(dbfriend.FriendID);
            Nfriendinfo.Id = dbfriend.Id;
            if (character == null)//不在线
            {
                Nfriendinfo.Info.Id = dbfriend.FriendID;
                Nfriendinfo.Info.Name = dbfriend.Name;
                Nfriendinfo.Info.Level = dbfriend.Level;
                Nfriendinfo.Info.Class = (CharacterClass)dbfriend.Class;
                Nfriendinfo.Status = false;
            }
            else//在线
            {
                Nfriendinfo.Info = character.GetBasicInfo();
                Nfriendinfo.Info.Name = character.Info.Name;
                Nfriendinfo.Info.Level = character.Info.Level;
                Nfriendinfo.Info.Class = character.Info.Class;

                if (dbfriend.Level != character.Info.Level)
                {
                    dbfriend.Level = character.Info.Level;
                }
                character.FriendManager.UpdateFriendstatus(this.Owner.Info, true);
                Nfriendinfo.Status = true;
            }
            return Nfriendinfo;
        }

        public void GetFriendInfos(List<NFriendInfo> friends)
        {
            friends.Clear();
            foreach (var friend in friends)
            {
                friends.Add(friend);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="friendInfo"></param>
        /// <param name="status"></param>
        public void UpdateFriendstatus(NCharacterInfo friendInfo, bool status)
        {
            foreach (var f in friends)
            {
                if (f.Info.Id == friendInfo.Id)
                {
                    f.Status = status;
                    break;
                }
            }
            FriendIsChanged = true;
        }

        /// <summary>
        /// 下线
        /// </summary>
        public void OffLineNotify()
        {
            foreach (var f in friends)
            {
                Character friend = CharacterManager.Instance.GetCharacter(f.Id);
                if (friend != null)
                {
                    friend.FriendManager.UpdateFriendstatus(Owner.Info, false);
                }
            }
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
                CharacterID = toId,
                FriendID = id,
                Name = CharacterManager.Instance.GetCharacter(toId).Info.Name,
                Level = CharacterManager.Instance.GetCharacter(toId).Info.Level,
                Class = (int)CharacterManager.Instance.GetCharacter(toId).Info.Class,
            };
            Owner.TChar.Friends.Add(friend);
            FriendIsChanged = true;
        }

        /// <summary>
        /// 后处理器
        /// </summary>
        /// <param name="message"></param>
        public void PostProcess(NetMessageResponse message)
        {
            if (FriendIsChanged)
            {
                Init();
                if (message.friendList == null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(friends);
                }
                FriendIsChanged = false;
            }
        }
    }
}
