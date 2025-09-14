using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriend : UIWindow
{
    public ListView friendList;//好友列表
    public GameObject friendItemPrefab;//好友预制体
    public UIFriendItem selected;//选中的好友


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
        InputBox.Show("请输入要添加的好友ID", "添加好友").OnSubmit += AddFriendOnSubmit;
    }

    private bool AddFriendOnSubmit(string inputText, out string tips)
    {
        throw new NotImplementedException();
    }

    public void OnChatClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("暂未开放", "私聊");
        }
        MessageBox.Show("请选择要私聊的好友", "私聊");
    }

    public void OnFriendRemoveClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("暂未开放", "删除好友");
        }
        MessageBox.Show("请选择要删除的好友", "删除好友");
    }

    public void OnLookFriendInfoClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("暂未开放", "删除好友");
        }
        MessageBox.Show("请选择要查看的好友", "查看信息");
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
