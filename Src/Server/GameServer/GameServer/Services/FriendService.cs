using Common;
using GameServer.Entities;
using GameServer.Managers;
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
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddReq);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddRes);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemoveReq);
        }

        public void Init()
        {

        }

        /// <summary>
        /// 接收发起好友请求的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnFriendAddReq(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddReq : : from_id:{0} from_name:{1} to_id{2} to_name{3}", character.Id, character.Name, request.ToId, request.ToName);
            //查询目标角色是否存在
            var cha = DBService.Instance.Entities.Characters.Where(c => c.ID == request.ToId).FirstOrDefault();
            sender.Session.Response.friendAddRes = new FriendAddResponse();
            //目标角色存在
            if (cha != null)
            {
                var friend = DBService.Instance.Entities.TCharacterFriends.Where(f => f.Id == request.ToId).FirstOrDefault();
                //判断是否已经是好友
                if (friend != null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.SendResPonse();
                    return;
                }
                //再判断好友是否在线
                //如果在线，则发送给对方
                NetConnection<NetSession> to = SessionManager.Instance.GetSession(request.ToId);
                if (to != null)
                {
                    to.Session.Response.friendAddReq = request;
                    to.SendResPonse();
                }
            }
            else
            {
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "该玩家不存在";
                sender.SendResPonse();
            }
        }

        private void OnFriendAddRes(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRes : : Result [{0}] : ErrorMsg [{1}]", response.Result, response.Errormsg);
            if (response.Result == Result.Success)
            {
                //同意添加好友
                NetConnection<NetSession> requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester != null)
                {
                    //加好友
                    character.FriendManager.AddFriend(response.Request.FromId, response.Request.ToId);
                    requester.Session.Character.FriendManager.AddFriend(response.Request.ToId, response.Request.FromId);
                    DBService.Instance.Save();
                    requester.Session.Response.friendRemoveRes = new FriendRemoveResponse();
                    requester.Session.Response.friendRemoveRes.Result = Result.Success;
                    requester.Session.Response.friendRemoveRes.Errormsg = response.Errormsg;
                    requester.SendResPonse();
                }
            }
        }

        private void OnFriendRemoveReq(NetConnection<NetSession> sender, FriendRemoveRequest message)
        {
            
        }
    }
}
