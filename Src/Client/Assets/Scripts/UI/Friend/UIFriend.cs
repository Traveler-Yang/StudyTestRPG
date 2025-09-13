using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Models;
using Managers;
using Entities;
using System;

public class UIFriend : UIWindow
{
    public ListView friendList;//�����б�
    public GameObject friendPrefab;//������Ԥ����
    public TMP_InputField inputField;//���������
    public UIFriendItem selectedItem;//��ǰѡ�еĺ�����

    void Start()
    {
        FriendService.Instance.OnFriendsUpdate = RefreshUI;
        friendList.onItemSelected += OnSelectedItem;
        RefreshUI();
    }

    void OnEnable()
    {
        FriendService.Instance.OnFriendsUpdate += RefreshUI;
    }

    void OnDisable()
    {
        FriendService.Instance.OnFriendsUpdate -= RefreshUI;
    }

    private void OnSelectedItem(ListView.ListViewItem item)
    {
        selectedItem = item as UIFriendItem;
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
    /// ������Ӻ��Ѱ�ť�¼�
    /// </summary>
    public void OnSearchClick()
    {
        
    }

    /// <summary>
    /// ˽�İ�ť
    /// </summary>
    public void OnFriendChatClick()
    {
        MessageBox.Show("��δ����");
    }

    /// <summary>
    /// ��Ӻ��Ѱ�ť
    /// </summary>
    public void OnFriendAddClick()
    {
        InputBox.Show("������Ҫ��ӵĺ���ID������", "��Ӻ���").OnSubmit += AddFriend_OnSubmit;
    }

    private bool AddFriend_OnSubmit(string inputText, out string tips)
    {
        int id = 0;
        string name = "";
        if (int.TryParse(inputText, out id))
        {
            //Ҫ��ӵĺ��Ѳ������Լ�
            if (User.Instance.CurrentCharacter.Id == id)
            {
                tips = "��������Լ�Ϊ����";
                return false;
            }
            FriendService.Instance.SendFriendAdd(id, User.Instance.CurrentCharacter.Name);
            tips = "��������ɹ�";
            return true;
        }
        else
        {
            name = inputText;
            //�������ֲ���
            Character cha =  CharacterManager.Instance.GetCharacterByName(name);
            if (cha != null)
            {
                FriendService.Instance.SendFriendAdd(cha.Id, cha.Name);
                tips = "��������ɹ�";
                return true;
            }
            else
            {
                tips = "û���ҵ������";
                return false;
            }
        }
    }

    public void Clear()
    {
        this.friendList.RemoveAll();
    }

    /// <summary>
    /// ˢ��UI
    /// </summary>
    public void RefreshUI()
    {
        this.Clear();
        this.Init();
    }
}
