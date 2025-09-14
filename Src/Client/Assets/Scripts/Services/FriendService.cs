using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FriendService : Singleton<FriendService>, IDisposable
{
    public UnityAction UpdateFriendList;

    public FriendService()
    {
        MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddRes);
        MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
        MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemoveRes);
        MessageDistributer.Instance.Subscribe<FriendAddNotify>(this.OnFriendAddNotify);
        MessageDistributer.Instance.Subscribe<FriendRemoveNotify>(this.OnFriendRemoveNotify);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddRes);
        MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
        MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemoveRes);
        MessageDistributer.Instance.Unsubscribe<FriendAddNotify>(this.OnFriendAddNotify);
        MessageDistributer.Instance.Unsubscribe<FriendRemoveNotify>(this.OnFriendRemoveNotify);
    }

    public void Init()
    {

    }

    /// <summary>
    /// 发送加好友请求
    /// </summary>
    /// <param name="toId"></param>
    /// <param name="toName"></param>
    public void SendFriendAddRequest(int toId, string toName)
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

    private void OnFriendAddRes(object sender, FriendAddResponse message)
    {
        
    }

    private void OnFriendList(object sender, FriendListResponse message)
    {
        
    }

    private void OnFriendRemoveRes(object sender, FriendRemoveResponse message)
    {
        
    }

    private void OnFriendAddNotify(object sender, FriendAddNotify message)
    {
        
    }

    private void OnFriendRemoveNotify(object sender, FriendRemoveNotify message)
    {
        
    }

}
