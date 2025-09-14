using Models;
using Network;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FriendService : Singleton<FriendService>, IDisposable
{
    public UnityAction OnFriendsUpdate;

    public void Init()
    {

    }

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

    /// <summary>
    /// ������Ӻ�������
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
    /// ������Ӻ�������Ļ�Ӧ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddRes(object sender, FriendAddResponse response)
    {
        Debug.LogFormat("OnFriendAddRes:{0} [{1}]", response.Result, response.Errormsg);
        if (response.Request == null)
            return;
        if (response.Result == Result.Success)
        {
            MessageBox.Show(string.Format("{0}{1}���������", response.Request.ToName, response.Errormsg), "��Ӻ���");
        }
        else
            MessageBox.Show("��Ӻ���ʧ��:\n" + response.Errormsg, "��Ӻ���", MessageBoxType.Error);
    }

    private void SendFriendAddResponse(bool accept, FriendAddRequest request)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendAddRes = new FriendAddResponse();
        message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
        message.Request.friendAddRes.Errormsg = accept ? "���ܺ�������" : "�ܾ���������";
        message.Request.friendAddRes.Request = request;
        NetClient.Instance.SendMessage(message);
    }

    /// <summary>
    /// ������Ӻ��ѵ�����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddReq(object sender, FriendAddRequest message)
    {
        Debug.LogFormat("OnFriendAddReq: {0} [{1}]", message.ToId, message.ToName);
        var box = MessageBox.Show(string.Format("{0}���������Ϊ����", message.FromName), "��Ӻ���", MessageBoxType.Confirm, "����", "�ܾ�");
        box.OnYes = () =>
        {
            SendFriendAddResponse(true, message);
        };
        box.OnNo = () =>
        {
            SendFriendAddResponse(false, message);
        };
    }

    private void OnFriendList(object sender, FriendListResponse response)
    {
        Debug.LogFormat("OnFriendList: [{0}] : [{1}] : [{2}]", response.Friends.Count, response.Result, response.Errormsg);
        FriendManager.Instance.allFriends = response.Friends;
        if(OnFriendsUpdate != null)
            OnFriendsUpdate.Invoke();
    }

    public void SendRemoveFriend(int FriendId)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.friendRemoveReq = new FriendRemoveRequest();
        message.Request.friendRemoveReq.UserId = User.Instance.CurrentCharacter.Id;
        message.Request.friendRemoveReq.FriendId = FriendId;
        NetClient.Instance.SendMessage(message);
    }

    private void OnFriendRemoveRes(object sender, FriendRemoveResponse response)
    {
        Debug.LogFormat("OnFriendRemoveReq: RemoveToID��[{0}] : [{1}] : [{2}]", response.Id, response.Result, response.Errormsg);
        if (response.Result == Result.Success)
        {
            MessageBox.Show(string.Format("ɾ�����Ѳ����ɹ���\n", response.Errormsg), "ɾ������");
            return;
        }
        MessageBox.Show(string.Format("ɾ�����Ѳ���ʧ�ܣ�\n", response.Errormsg), "ɾ������", MessageBoxType.Error);
    }

    private void OnFriendRemoveNotify(object sender, FriendRemoveNotify response)
    {
        Debug.LogFormat("OnFriendRemoveNotify: RemoveToID��[{0}] : [{1}] : [{2}]", response.Id, response.Result, response.Errormsg);
        if (response.Result == Result.Success)
        {
            MessageBox.Show(string.Format("ɾ�����Ѳ����ɹ���\n", response.Errormsg), "ɾ������");
            return;
        }
        MessageBox.Show(string.Format("ɾ�����Ѳ���ʧ�ܣ�\n", response.Errormsg), "ɾ������", MessageBoxType.Error);
    }
}
