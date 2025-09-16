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
        MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddReq);
        MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddRes);
        MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
        MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemoveRes);
        MessageDistributer.Instance.Subscribe<FriendRemoveNotify>(this.OnFriendRemoveNotify);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddReq);
        MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddRes);
        MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
        MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemoveRes);
        MessageDistributer.Instance.Unsubscribe<FriendRemoveNotify>(this.OnFriendRemoveNotify);
    }

    public void Init()
    {

    }

    /// <summary>
    /// ���ͼӺ�������
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
    /// ��Ӻ��ѵ�������Ӧ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddRes(object sender, FriendAddResponse message)
    {
        Debug.LogFormat("OnFriendAddRes : Result:{0} ErrorMsg{1}", message.Result, message.Errormsg);
        MessageBox.Show(message.Errormsg, "��Ӻ���");
        if (message.Result == Result.Success)
        {
            
        }
    }

    private void SendFriendAddRes(bool accept, FriendAddRequest request)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendAddRes = new FriendAddResponse();
        message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
        message.Request.friendAddRes.Errormsg = accept ? "�Է�ͬ�����������" : "�Է��ܾ����������";
        message.Request.friendAddRes.Request = request;
        NetClient.Instance.SendMessage(message);
    }

    /// <summary>
    /// ��Ӻ��ѵĽ��շ���Ӧ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddReq(object sender, FriendAddRequest message)
    {
        Debug.LogFormat("OnFriendAddRes : FromID:{0} FromName:{1} :: ToID{2} ToName:{3}", message.FromId, message.FromName, message.ToId, message.ToName);
        var msgBox = MessageBox.Show(string.Format("{0} ���������Ϊ���ѣ��Ƿ�ͬ�⣿", message.FromName), "��Ӻ���", MessageBoxType.Confirm, "ͬ��", "�ܾ�");
        msgBox.OnYes = () =>
        {
            this.SendFriendAddRes(true, message);
        };
        msgBox.OnNo = () =>
        {
            this.SendFriendAddRes(false, message);
        };
    }

    private void OnFriendList(object sender, FriendListResponse message)
    {
        Debug.LogFormat("OnFriendList : Result:{0} ErrorMsg{1}", message.Result, message.Errormsg);
        this.UpdateFriendList?.Invoke();
        FriendManager.Instance.allFriend = message.Friends;
    }

    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="id"></param>
    public void SendFriendRemoveReq(int id)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendRemoveReq = new FriendRemoveRequest();
        message.Request.friendRemoveReq.Id = id;
        NetClient.Instance.SendMessage(message);
    }

    private void OnFriendRemoveRes(object sender, FriendRemoveResponse message)
    {
        if (message.Result == Result.Success)
        {
            MessageBox.Show(message.Errormsg, "ɾ������", MessageBoxType.Information);
            return;
        }
        MessageBox.Show(message.Errormsg, "ɾ������", MessageBoxType.Error);
    }

    private void OnFriendRemoveNotify(object sender, FriendRemoveNotify message)
    {
        if (message.Result == Result.Success)
        {
            MessageBox.Show(message.Errormsg, "ɾ������", MessageBoxType.Information);
            return;
        }
        MessageBox.Show(message.Errormsg, "ɾ������", MessageBoxType.Error);
    }
}
