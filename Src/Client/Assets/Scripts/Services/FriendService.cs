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
        //MessageDistributer.Instance.Subscribe<FriendListRequest>(this.OnFriendList);
        //MessageDistributer.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemoveReq);
    }


    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddReq);
        MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddRes);
        //MessageDistributer.Instance.Unsubscribe<FriendListRequest>(this.OnFriendList);
        //MessageDistributer.Instance.Unsubscribe<FriendRemoveRequest>(this.OnFriendRemoveReq);
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
        if (response.Result == Result.Success)
        {
            MessageBox.Show("��Ӻ���" + response.Request.ToName + "�ɹ�", "��Ӻ���");
        }
        else
        {
            MessageBox.Show("��Ӻ���ʧ��:\n" + response.Errormsg, "��Ӻ���", MessageBoxType.Error);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnFriendAddReq(object sender, FriendAddRequest message)
    {
        
    }

}
