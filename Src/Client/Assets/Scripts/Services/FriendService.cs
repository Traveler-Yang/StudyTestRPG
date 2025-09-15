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

    /// <summary>
    /// 添加好友的请求方响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddRes(object sender, FriendAddResponse message)
    {
        Debug.LogFormat("OnFriendAddRes : Result:{0} ErrorMsg{1}", message.Result, message.Errormsg);
        if (message.Result == Result.Success)
        {
            MessageBox.Show(message.Errormsg, "添加好友");
        }
    }

    public void SendFriendAddNotify(bool isAgree)
    {
        NetMessage message = new NetMessage();
        message.Response = new NetMessageResponse();
        message.Response.friendAddRes = new FriendAddResponse();
        message.Response.friendAddRes.Result = isAgree ? Result.Success : Result.Success;
        message.Response.friendAddRes.Errormsg = isAgree ? "对方同意了你的请求" : "对方拒绝了你的请求";
        NetClient.Instance.SendMessage(message);
    }

    /// <summary>
    /// 添加好友的接收方响应
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddNotify(object sender, FriendAddNotify message)
    {
        Debug.LogFormat("OnFriendAddNotify : Result:{0} ErrorMsg{1}", message.Result, message.Errormsg);
        if (message.Result == Result.Success)
        {
            var msgBox = MessageBox.Show(message.Errormsg, "添加好友", MessageBoxType.Confirm, "同意", "拒绝");
            msgBox.OnYes = () =>
            {
                this.SendFriendAddNotify(true);
            };
            msgBox.OnNo = () =>
            {
                this.SendFriendAddNotify(false);
            };
        }
    }

    private void OnFriendList(object sender, FriendListResponse message)
    {
        
    }

    private void OnFriendRemoveRes(object sender, FriendRemoveResponse message)
    {
        
    }

    private void OnFriendRemoveNotify(object sender, FriendRemoveNotify message)
    {
        
    }

}
