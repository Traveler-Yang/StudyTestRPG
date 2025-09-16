using Entities;
using Managers;
using Models;
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
        int id = 0;
        string name = "";
        //�������ת��Ϊ���Σ�����id
        if (inputText != "")
        {
            if (int.TryParse(inputText, out id))
            {
                if (User.Instance.Info.Id == id)
                {
                    tips = "����������Լ�Ŷ~";
                    return false;
                }
                FriendService.Instance.SendFriendAddRequest(id, CharacterManager.Instance.GetCharacter(id).Info.Name);
            }
            name = inputText;
            Character cha = CharacterManager.Instance.GetCharacterByName(name);
            if (cha != null)
            {
                FriendService.Instance.SendFriendAddRequest(cha.Info.Id, name);
                tips = "��������ɹ���";
                return true;
            }
        }
        tips = "������Ҫ�����ĺ���ID";
        return false;
    }

    public void OnChatClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("��δ����", "˽��");
            return;
        }
        MessageBox.Show("��ѡ��Ҫ˽�ĵĺ���", "˽��");
    }

    public void OnFriendRemoveClick()
    {
        if (this.selected != null)
        {
            var msgBox = MessageBox.Show("ȷ��Ҫɾ���ú�����", "ɾ������", MessageBoxType.Confirm);
            msgBox.OnYes = () =>
            {
                FriendService.Instance.SendFriendRemoveReq(this.selected.info.Info.Id);
            };
            return;
        }
        MessageBox.Show("��ѡ��Ҫɾ���ĺ���", "ɾ������");
    }

    public void OnLookFriendInfoClick()
    {
        if (this.selected != null)
        {
            MessageBox.Show("��δ����", "ɾ������");
            return;
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
            item.owner = this.friendList;
            item.SetFriendInfo(f);
            this.friendList.AddItem(item);
        }
    }

    public void ClearFriend()
    {
        this.friendList.RemoveAll();
    }
}
