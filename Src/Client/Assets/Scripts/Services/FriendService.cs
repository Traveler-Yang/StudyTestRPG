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
        MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddRes);
        //MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
        //MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemoveReq);
    }


    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddReq);
        MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddRes);
        //MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
        //MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemoveReq);
    }

    /// <summary>
    /// 发送添加好友请求
    /// </summary>
    /// <param name="toId"></param>
    /// <param name="toName"></param>
    public void SendFriendAdd(int toId, string toName)
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
    private void OnFriendAddRes(object sender, FriendAddResponse response)
    {
        Debug.LogFormat("OnFriendAddRes:{0} [{1}]", response.Result, response.Errormsg);
        if (sender == null)
            return;
        if (response.Result == Result.Success)
        {
            MessageBox.Show("添加好友:" + response.Request.ToName + "成功", "添加好友");
        }
        else
            MessageBox.Show("添加好友失败:\n" + response.Errormsg, "添加好友", MessageBoxType.Error);
    }

    private void SendFriendAddResponse(bool accept, FriendAddRequest request)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendAddRes = new FriendAddResponse();
        message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
        message.Request.friendAddRes.Errormsg = accept ? "接受好友请求" : "拒绝好友请求";
        message.Request.friendAddRes.Request = request;
        NetClient.Instance.SendMessage(message);
    }

    /// <summary>
    /// 接收添加好友的请求
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddReq(object sender, FriendAddRequest message)
    {
        Debug.LogFormat("OnFriendAddReq: {0} [{1}]", message.ToId, message.ToName);
        var box = MessageBox.Show(string.Format("{0}请求添加你为好友", message.FromName), "添加好友", MessageBoxType.Confirm, "接受", "拒绝");
        box.OnYes = () =>
        {
            SendFriendAddResponse(true, message);
        };
        box.OnNo = () =>
        {
            SendFriendAddResponse(false, message);
        };
    }
}
