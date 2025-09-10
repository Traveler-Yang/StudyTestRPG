using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using TMPro;
using Services;

public class UIChat : MonoBehaviour
{
    public TextMeshProUGUI textArea;//����������ʾ��

    public TabView channelTab;//��ť��

    public InputField chatText;//���������
    public GameObject target;
    public Text chatTarget;

    public Dropdown channelSelect;

    void Start()
    {
        this.channelTab.OnTabSelect += OnDisPlayChannelSelected;
        ChatManager.Instance.OnChat += RefreshUI;
    }

    private void OnDestroy()
    {
        ChatManager.Instance.OnChat -= RefreshUI;
    }

    // Update is called once per frame
    void Update()
    {
        //�ҵ�ǰ����������Ƿ��н��㣬����н��㣬�������������
        InputManager.Instance.IsInputMode = chatText.isFocused;
    }

    private void OnDisPlayChannelSelected(int idx)
    {
        //ѡ��Ƶ��ʱ����ѡ���Ƶ����������������
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        //ѡ���ˢ��UI
        RefreshUI();
    }

    /// <summary>
    /// ˢ��UI
    /// </summary>
    public void RefreshUI()
    {
        //����ȡ���ĵ�ǰת�����ı���������Ϣ��ֵ����ǰUI
        this.textArea.text = ChatManager.Instance.GetCurrentMassage();
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        //�����ǰ���͵���˽�ģ���
        if (ChatManager.Instance.SendChannel == ChatChannel.Private)
        {
            this.target.gameObject.SetActive(true);//�������
            if (ChatManager.Instance.PrivateID != 0)//˽�Ķ���Ϊnull
            {
                this.chatTarget.text = string.Format("{0} :", ChatManager.Instance.PrivateName);//��˽�Ķ������ָ�ֵ
            }
            else
                this.chatTarget.text = "<��> :";
        }
        else
        {
            this.target.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ������Ϣ��ť
    /// </summary>
    public void OnClickSend()
    {
        OnEndInput(this.chatText.text);
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Btn_1);
    }

    /// <summary>
    /// ����������Ϣ�¼�
    /// </summary>
    /// <param name="text"></param>
    public void OnEndInput(string text)
    {
        //���������null��������Ϣ
        if (!string.IsNullOrEmpty(text.Trim()))
            this.SendChat(text);//������Ϣ
        this.chatText.text = "";//�������Ϣ��
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="content"></param>
    private void SendChat(string content)
    {
        ChatManager.Instance.SendChat(content, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }

    /// <summary>
    /// �л�Ƶ��
    /// </summary>
    /// <param name="idx">��ǰ�л���Ƶ������</param>
    public void OnSendChannelChanged(int idx)
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Btn_1);
        //�����ǰѡ���Ƶ���͵�ǰƵ��һ�£����л�Ƶ����ֱ�ӷ���
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;

        //�����ǰƵ����Ҫ�л���Ƶ������ͬ���Ÿ�ֵ
        //�ȸ���Manager�еĵ�ǰƵ����������ĳɹ��������UI������
        if (!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)idx + 1))
        {
            this.channelSelect.value = (int)(ChatManager.Instance.sendChannel - 1);
        }
        else
        {
            //���δ���ĳɹ�����ˢ��UI���޶��飬�޹���Ż᷵��ʧ�ܣ�
            RefreshUI();
        }
    }
}
