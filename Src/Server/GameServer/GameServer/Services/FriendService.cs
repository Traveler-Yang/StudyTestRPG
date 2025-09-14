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
            NetConnection<NetSession> requester = SessionManager.Instance.GetSession(response.Request.FromId);
            if (requester != null)
            {
                //同意添加好友
                requester.Session.Response.friendAddRes = new FriendAddResponse();
                if (response.Result == Result.Success)
                {
                    //加好友
                    character.FriendManager.AddFriend(response.Request.FromId, response.Request.ToId);
                    requester.Session.Character.FriendManager.AddFriend(response.Request.ToId, response.Request.FromId);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "同意";
                    requester.Session.Response.friendAddRes.Request = response.Request;
                    requester.SendResPonse();
                    return;
                }
                //拒绝添加好友
                requester.Session.Response.friendAddRes.Result = Result.Failed;
                requester.Session.Response.friendAddRes.Errormsg = "拒绝";
                requester.Session.Response.friendAddRes.Request = response.Request;
                requester.SendResPonse();
            }
            //请求者不在线
        }

        private void OnFriendRemoveReq(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemoveReq : : CharacterId [{0}] : ToId [{1}]", request.UserId, request.FriendId);
            sender.Session.Response.friendRemoveRes = new FriendRemoveResponse();
            //var friend = character.TChar.Friends.Where(f => f.FriendID == request.FriendId).FirstOrDefault();
            NFriendInfo friend = character.FriendManager.GetFriendInfo(request.FriendId);
            if (friend == null)//查找好友列表中有无此好友
            {
                sender.Session.Response.friendRemoveRes.Id = request.FriendId;
                sender.Session.Response.friendRemoveRes.Result = Result.Failed;
                sender.Session.Response.friendRemoveRes.Errormsg = "好友不存在";
                sender.SendResPonse();
                return;
            }
            NetConnection<NetSession> toSession = SessionManager.Instance.GetSession(request.FriendId);
            if (toSession != null)
            {
                //var toFriend = toCharacter.TChar.Friends.Where(tf => tf.FriendID == character.Info.Id).FirstOrDefault();
                NFriendInfo toFriend = toSession.Session.Character.FriendManager.GetFriendInfo(character.Info.Id);
                if (toFriend == null)//校验对方好友列表中是否有自己
                {
                    sender.Session.Response.friendRemoveRes.Id = request.FriendId;
                    sender.Session.Response.friendRemoveRes.Result = Result.Failed;
                    sender.Session.Response.friendRemoveRes.Errormsg = "双方列表不一致，请联系管理员！";
                    sender.SendResPonse();
                    return;
                }
                //对方在线
                toSession.Session.Response.friendRemoveNofity = new FriendRemoveNotify();
                toSession.Session.Response.friendRemoveNofity.Id = character.Info.Id;
                toSession.Session.Response.friendRemoveNofity.Result = Result.Success;
                toSession.Session.Response.friendRemoveNofity.Errormsg = string.Format("{0}将你从好友列表中删除了",character.Info.Id);
                sender.SendResPonse();
            }
            //双向删除好友
            character.FriendManager.RemoveFriend(request.FriendId);
            toSession.Session.Character.FriendManager.RemoveFriend(character.Info.Id);
            DBService.Instance.Save();
            sender.Session.Response.friendRemoveRes = new FriendRemoveResponse();
            sender.Session.Response.friendRemoveRes.Id = request.FriendId;
            sender.Session.Response.friendRemoveRes.Result = Result.Success;
            sender.Session.Response.friendRemoveRes.Errormsg = string.Format("成功将[{0}]好友删除", request.FriendId);
            sender.SendResPonse();
        }
    }
}
