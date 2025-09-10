using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopCharMenu : UIWindow, IDeselectHandler
{
    //Ŀ��id
    public int targetId;
    //Ŀ���ǳ�
    public string targetName;

    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData;
        if (ed.hovered.Contains(this.gameObject))
            return;
        this.Close(UIWindowResult.None);
    }

    private void OnEnable()
    {
        this.GetComponent<Selectable>().Select();
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
    }

    /// <summary>
    /// ˽��
    /// </summary>
    public void OnChat()
    {
        ChatManager.Instance.StartPrivateChat(targetId, targetName);
        this.Close(UIWindowResult.None);
    }

    /// <summary>
    /// ��Ӻ���
    /// </summary>
    public void OnAddFriend()
    {
        FriendService.Instance.SendFriendAddRequest(targetId, targetName);
        this.Close(UIWindowResult.None);
    }

    /// <summary>
    /// �������
    /// </summary>
    public void OnInviteTemp()
    {
        TempService.Instance.SendTempInviteRequest(targetId, targetName);
        this.Close(UIWindowResult.None);
    }
}
