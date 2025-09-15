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
    public class FriendService : Singleton<FriendService>
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
        /// 接收添加好友的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnFriendAddReq(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddReq : : fromId:{0} : fromName:{1} : toId{2} : toName{3}", request.FromId, request.FromName, request.ToId, request.ToName);
            NetConnection<NetSession> toRequest = SessionManager.Instance.GetSession(request.ToId);
            if (toRequest == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "对方不在线";
                sender.SendResPonse();
                return;
            }
            var toFriend = character.TChar.Friends.Where(f => f.FriendID == request.ToId).FirstOrDefault();
            if (toFriend != null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "对方已经是好友了";
                sender.SendResPonse();
                return;
            }
            sender.Session.Response.friendAddRes = new FriendAddResponse();
            sender.Session.Response.friendAddRes.Result = Result.Success;
            sender.Session.Response.friendAddRes.Errormsg = "请求发送成功！";
            sender.SendResPonse();
            //消息转发
            toRequest.Session.Response.friendAddNotify = new FriendAddNotify();
            toRequest.Session.Response.friendAddNotify.Result = Result.Success;
            toRequest.Session.Response.friendAddNotify.Errormsg = string.Format("{0}请求添加你为好友是否同意？", request.FromName);
            toRequest.SendResPonse();
        }

        /// <summary>
        /// 接收好友添加是否同意的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnFriendAddRes(NetConnection<NetSession> sender, FriendAddResponse message)
        {
            
        }


        private void OnFriendRemoveReq(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            
        }
    }
}
