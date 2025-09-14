using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriend : UIWindow
{
    public ListView friendList;//�����б�
    public GameObject friendItemPrefab;//����Ԥ����
    public UIFriendItem selected;//ѡ�еĺ���


    private void Start()
    {
        FriendService.Instance.UpdateFriendList = RefreshUI;
        friendList.onItemSelected += OnSelectedItem;
        this.RefreshUI();
    }

    private void OnEnable()
    {
        FriendService.Instance.UpdateFriendList += RefreshUI;
    }

    private void OnDisable()
    {
        FriendService.Instance.UpdateFriendList -= RefreshUI;
    }

    private void OnSelectedItem(ListView.ListViewItem item)
    {
        this.selected = item as UIFriendItem;
    }

    public void OnAddFriendClick()
    {
        InputBox.Show("������Ҫ��ӵĺ���ID", "��Ӻ���").OnSubmit += AddFriendOnSubmit;
    }

    private bool AddFriendOnSubmit(string inputText, out string tips)
    {
        throw new NotImplementedException();
    }

    public void OnChatClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("��δ����", "˽��");
        }
        MessageBox.Show("��ѡ��Ҫ˽�ĵĺ���", "˽��");
    }

    public void OnFriendRemoveClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("��δ����", "ɾ������");
        }
        MessageBox.Show("��ѡ��Ҫɾ���ĺ���", "ɾ������");
    }

    public void OnLookFriendInfoClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("��δ����", "ɾ������");
        }
        MessageBox.Show("��ѡ��Ҫ�鿴�ĺ���", "�鿴��Ϣ");
    }

    public void RefreshUI()
    {
        ClearFriend();
        InitFriend();
    }

    public void InitFriend()
    {
        foreach (var f in FriendManager.Instance.allFriend)
        {
            GameObject go = Instantiate(friendItemPrefab, friendList.transform);
            UIFriendItem item =  go.GetComponent<UIFriendItem>();
            item.SetFriendInfo(f);
            this.friendList.AddItem(item);
        }
    }

    public void ClearFriend()
    {
        this.friendList.RemoveAll();
    }
}
