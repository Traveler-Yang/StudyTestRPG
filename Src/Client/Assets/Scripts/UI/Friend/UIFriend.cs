using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Models;
using Managers;
using Entities;

public class UIFriend : UIWindow
{
    public ListView friendList;//好友列表
    public GameObject friendPrefab;//好友项预制体
    public TMP_InputField inputField;//搜索输入框
    public UIFriendItem selectedItem;//当前选中的好友项

    void Start()
    {
        RefreshUI();
    }


    
    public void Init()
    {
        foreach (var friend in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(friendPrefab, friendList.transform);
            UIFriendItem item = go.GetComponent<UIFriendItem>();
            item.owner = friendList;
            item.SetFriendInfo(friend);
            friendList.AddItem(item);
        }
    }

    /// <summary>
    /// 搜索添加好友按钮事件
    /// </summary>
    public void OnSearchClick()
    {
        
    }

    /// <summary>
    /// 私聊按钮
    /// </summary>
    public void OnFriendChatClick()
    {
        MessageBox.Show("暂未开放");
    }

    /// <summary>
    /// 添加好友按钮
    /// </summary>
    public void OnFriendAddClick()
    {
        InputBox.Show("请输入要添加的好友ID或名称", "添加好友").OnSubmit += AddFriend_OnSubmit;
    }

    private bool AddFriend_OnSubmit(string inputText, out string tips)
    {
        int id = 0;
        string name = "";
        if (int.TryParse(inputText, out id))
        {
            //要添加的好友不能是自己
            if (User.Instance.CurrentCharacter.Id.ToString() == inputText)
            {
                tips = "不能添加自己为好友";
                return false;
            }
            FriendService.Instance.SendFriendAdd(id, name);
            tips = "发送请求成功";
            return true;
        }
        else
        {
            //则用名字查找
            Character cha =  CharacterManager.Instance.GetCharacterByName(inputText);
            if (cha != null)
            {
                FriendService.Instance.SendFriendAdd(cha.Id, cha.Name);
                tips = "发送请求成功";
                return true;
            }
            else
            {
                tips = "没有找到该玩家";
                return false;
            }
        }
    }

    public void Clear()
    {
        this.friendList.RemoveAll();
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        this.Clear();
        this.Init();
    }
}
