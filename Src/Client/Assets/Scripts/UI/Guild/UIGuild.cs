using Managers;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;//��ԱPrefab
    public ListView listMain;//��Ա�б�
    public Transform listRoot;//��Ա�б���ڵ�
    public UIGuildInfo uiInfo;//������Ϣ��
    public UIGuildMemberItem selectedItem;//��ǰѡ�еĳ�Ա

    public GameObject panelAdmin;//�������
    public GameObject panelLeader;//�᳤���

    void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += OnGuildMemberSelected;
        this.UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    private void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        //����ҵĹ���Ȩ�޴�����ͨ��Ա�ģ�����ʾ����Ա�İ�ť����֮��
        this.panelAdmin.SetActive(GuildManager.Instance.MyMemberInfo.Duty > GuildDuty.None);
        //������ǻ᳤������ʾ�᳤��Ȩ�ް�ť����֮��
        this.panelLeader.SetActive(GuildManager.Instance.MyMemberInfo.Duty == GuildDuty.President);
    }

    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }

    /// <summary>
    /// ��ʼ����Ա�б�
    /// </summary>
    /// <param name="guilds"></param>
    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    /// <summary>
    /// ת��
    /// </summary>
    public void OnClickTransfer()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫת�õĳ�Ա", "����", MessageBoxType.Error);
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ���᳤ְ��ת�ø� [{0}] ��", selectedItem.info.charInfo.Name), "ת�û᳤", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, "", "", this.selectedItem.info.charInfo.Id);
        };
    }

    /// <summary>
    /// ��ְ
    /// </summary>
    public void OnClickPromotion()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ�����ĳ�Ա", "����", MessageBoxType.Error);
            return;
        }
        if (this.selectedItem.info.Duty != GuildDuty.None)
        {
            MessageBox.Show("�Է��ƺ�������������", "����", MessageBoxType.Information);
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ�� [{0}] ����Ϊ���᳤��", selectedItem.info.charInfo.Name), "��Ա����", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, "", "", this.selectedItem.info.charInfo.Id);
        };
    }

    /// <summary>
    /// ����
    /// </summary>
    public void OnClickRecall()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ����ĳ�Ա", "����", MessageBoxType.Error);
            return;
        }
        if (this.selectedItem.info.Duty == GuildDuty.None)
        {
            MessageBox.Show("�Է��ƺ���ְ����", "����", MessageBoxType.Information);
            return;
        }
        if (this.selectedItem.info.Duty == GuildDuty.President)
        {
            MessageBox.Show("�᳤�ɲ������ܶ���", "����", MessageBoxType.Information);
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ�� [{0}] ����Ϊ��ͨ��Ա��", selectedItem.info.charInfo.Name), "ְ�����", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, "", "", this.selectedItem.info.charInfo.Id);
        };
    }

    /// <summary>
    /// �����б�
    /// </summary>
    public void OnClickRequestList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    /// <summary>
    /// �߳�����
    /// </summary>
    public void OnClickKickOut()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ�߳��ĳ�Ա", "����", MessageBoxType.Error);
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ�� [{0}] �߳�������", selectedItem.info.charInfo.Name), "�߳�����", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, "", "", this.selectedItem.info.charInfo.Id);
        };
    }

    /// <summary>
    /// ˽��
    /// </summary>
    public void OnClickChat()
    {
        MessageBox.Show("��δ����");
    }

    /// <summary>
    /// �˳�����
    /// </summary>
    public void OnClickLeave()
    {
        MessageBox.Show(string.Format("ȷ��Ҫ�˳����� [{0}] ��", GuildManager.Instance.guildInfo.GuildName), "�˳�����", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendGuildLeaveRequest();
        };
    }

    /// <summary>
    /// �޸Ĺ��ṫ��
    /// </summary>
    public void OnClickChangeNotice()
    {
        UIManager.Instance.Show<UIGuildChangeInfo>();
    }
}
