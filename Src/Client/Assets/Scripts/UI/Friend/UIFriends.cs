using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class UIFriends : UIWindow
{
    public GameObject itemPrefab;//������Ԥ����
    public ListView listMain;//�����б��
    public Transform itemRoot;//�����б���ڵ�
    public UIFriendItem selectedItem;//��ǰѡ�е���˭

    void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += OnFriendSelected;
        RefreshUI();

    }

    private void OnEnable()
    {
        FriendService.Instance.OnFriendUpdate += RefreshUI;
    }

    private void OnDisable()
    {
        FriendService.Instance.OnFriendUpdate -= RefreshUI;
    }

    public void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }

    public void OnClickFriendAdd()
    {
        InputBox.Show("������Ҫ��ӵĺ��ѵ����ƻ�ID", "��Ӻ���").OnSubmit += OnFriendAddSubmit;
    }

    /// <summary>
    /// ��Ӻ����¼������ȷ�ϰ�ťִ�е��¼���
    /// </summary>
    /// <param name="input">�������Ϣ</param>
    /// <param name="tips">��ʾ��Ϣ</param>
    /// <returns></returns>
    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        //��Ϊ����������п�����һ��id������һ���ı�
        //��������Ҫ���ж�����������ݿɲ�����ת����int����
        //���������ת�����ͽ��������Ϣ��ֵ������
        if (!int.TryParse(input, out friendId))
            friendName = input;
        //�������ת��
        //�ж�����������Ƿ����Լ�
        if (friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
            tips = "����������Լ�Ŷ~";
            return false;
        }
        //��ѯ�����Ƿ����
        foreach (var item in FriendManager.Instance.allFriends)
        {
            //������ڣ��򷵻أ����²�ѯ
            if (item.friendInfo.Id == friendId)
            {
                friendName = item.friendInfo.Name;
                break;
            }
            //���û���ҵ����򷵻�false
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickFriendChat()
    {
        MessageBox.Show("��δ����");
    }

    /// <summary>
    /// ������ӵ���¼�
    /// </summary>
    public void OnClickFriendTemp()
    {
        if (selectedItem == null)//δѡ�񲻿�����
        {
            MessageBox.Show("��ѡ��Ҫ����ĺ���");
            return;
        }
        if (!selectedItem.info.Status)//δ���߲�������
        {
            MessageBox.Show("��ѡ�����ߵĺ���");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ������� [{0}] ���������", selectedItem.info.friendInfo.Name), "�������", MessageBoxType.Confirm, "����", "ȡ��").OnYes = () =>
        {
            TempService.Instance.SendTempInviteRequest(this.selectedItem.info.friendInfo.Id, this.selectedItem.info.friendInfo.Name);
        };

    }

    /// <summary>
    /// ���ɾ�������¼�
    /// </summary>
    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫɾ���ĺ���");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫɾ��[{0}]������", selectedItem.info.friendInfo.Name), "ɾ������", MessageBoxType.Confirm, "ɾ��", "ȡ��").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(selectedItem.info.Id, selectedItem.info.friendInfo.Id);
        };
    }

    /// <summary>
    /// ������������¼�
    /// </summary>
    public void OnClickSearchFriend()
    {
        InputBox.Show("������Ҫ�����ĺ���ID���ǳ�", "��������").OnSubmit += OnFriendSearchSubmit;
    }

    /// <summary>
    /// �ڵ�ǰ�����б�����������
    /// </summary>
    public NFriendInfo SearchLocalFriend(string input, out string tips)
    {
        tips = "";

        if (string.IsNullOrWhiteSpace(input))
        {
            tips = "��������Ч�ĺ����ǳƻ�ID";
            return null;
        }

        // �ų��Լ�
        if (input == User.Instance.CurrentCharacter.Name || input == User.Instance.CurrentCharacter.Id.ToString())
        {
            tips = "���������Լ�Ŷ~";
            return null;
        }
        //��ѯ�б�
        foreach (var item in FriendManager.Instance.allFriends)
        {
            if (item.friendInfo.Name == input || item.friendInfo.Id.ToString() == input)
            {
                return item;
            }
        }

        tips = "û���ҵ��������~";
        return null;
    }
    
    /// <summary>
    /// ���������¼�
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tips"></param>
    /// <returns></returns>
    private bool OnFriendSearchSubmit(string input, out string tips)
    {
        var result = SearchLocalFriend(input, out tips);
        if (result == null)
            return false;

        // չʾ UI
        ClearFriendList();
        GameObject go = Instantiate(itemPrefab, this.listMain.transform);
        UIFriendItem ui = go.GetComponent<UIFriendItem>();
        ui.SetFriendInfo(result);
        this.listMain.AddItem(ui);

        return true;
    }


    void RefreshUI()
    {
        ClearFriendList();
        InitFriendItems();
    }

    /// <summary>
    /// ��ʼ�����к����б�
    /// </summary>
    private void InitFriendItems()
    {
        // ����״̬�������ߵ�����ǰ�棨Status == true��
        var sortedFriends = FriendManager.Instance.allFriends
                               .OrderByDescending(f => f.Status)
                               .ToList();

        foreach (var item in sortedFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    /// <summary>
    /// ��������б�
    /// </summary>
    private void ClearFriendList()
    {
        this.listMain.RemoveAll();
    }
}
