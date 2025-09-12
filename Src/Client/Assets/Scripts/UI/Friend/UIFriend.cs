using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Models;

public class UIFriend : UIWindow
{
    public ListView friendList;//�����б�
    public GameObject friendPrefab;//������Ԥ����
    public TMP_InputField inputField;//���������
    public UIFriendItem selectedItem;//��ǰѡ�еĺ�����

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
    /// ������Ӻ��Ѱ�ť�¼�
    /// </summary>
    public void OnSearchClick()
    {
        if (inputField.text != "")
        {
            int id = 0;
            //�������������֣���ID������������������
            if (int.TryParse(inputField.text, out id))
            {
                if (User.Instance.CurrentCharacter.Id == id)
                {
                    MessageBox.Show("��������Լ�Ϊ����", "��������", MessageBoxType.Information);
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
            MessageBox.Show("������Ҫ�����ĺ���ID������", "��������", MessageBoxType.Information);
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
