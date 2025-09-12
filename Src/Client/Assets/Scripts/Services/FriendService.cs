using Models;
using Network;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendService : Singleton<FriendService>, IDisposable
{
    public FriendService()
    {
        MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddReq);
        //MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddRes);
        //MessageDistributer.Instance.Subscribe<FriendListRequest>(this.OnFriendList);
        //MessageDistributer.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemoveReq);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddReq);
        //MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddRes);
        //MessageDistributer.Instance.Unsubscribe<FriendListRequest>(this.OnFriendList);
        //MessageDistributer.Instance.Unsubscribe<FriendRemoveRequest>(this.OnFriendRemoveReq);
    }

    /// <summary>
    /// 发送添加好友请求
    /// </summary>
    /// <param name="toId"></param>
    /// <param name="toName"></param>
    public void SendFriendAddReq(int toId, string toName)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendAddReq = new FriendAddRequest();
        message.Request.friendAddReq.ToId = toId;
        message.Request.friendAddReq.ToName = toName;
        message.Request.friendAddReq.FromId = User.Instance.CurrentCharacter.Id;
        message.Request.friendAddReq.FromName = User.Instance.CurrentCharacter.Name;
        NetClient.Instance.SendMessage(message);
    }

    /// <summary>
    /// 接收添加好友请求的回应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddReq(object sender, FriendAddRequest message)
    {
        
    }

}
