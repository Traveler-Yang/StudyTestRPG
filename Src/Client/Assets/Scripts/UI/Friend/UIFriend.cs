using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Models;

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
        if (inputField.text != "")
        {
            int id = 0;
            //如果输入的是数字，则按ID搜索，否则按名称搜索
            if (int.TryParse(inputField.text, out id))
            {
                if (User.Instance.CurrentCharacter.Id == id)
                {
                    MessageBox.Show("不能添加自己为好友", "好友搜索", MessageBoxType.Information);
                    return;
                }else
                {
                    FriendService.Instance.SendFriendAddReq(id, "");
                    return;
                }
            }
            FriendService.Instance.SendFriendAddReq(id, inputField.text);
        }
        else
            MessageBox.Show("请输入要搜索的好友ID或名称", "好友搜索", MessageBoxType.Information);
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
