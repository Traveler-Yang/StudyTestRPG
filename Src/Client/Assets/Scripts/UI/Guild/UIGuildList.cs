using Services;
using SkillBridge.Message;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildList : UIWindow
{
    public GameObject itemPrefab;//����Ԥ����
    public ListView listMain;//�����б�
    public Transform listRoot;//�����б���ڵ�
    public UIGuildInfo uiInfo;//������Ϣ��
    public UIGuildItem selectedItem;//��ǰѡ�еĹ���
    public InputField searchInput;//���������

    void Start()
    {
        this.listMain.onItemSelected += OnGuildMemberSelected;
        this.uiInfo.Info = null;
        GuildService.Instance.OnGuildListResult += UpdateGuildList;//�����б��¼�

        GuildService.Instance.OnGuildSearchResult += OnSearchEvent;//�����¼�
        GuildService.Instance.SendGuildListRequest();//�����б�����
    }

    private void OnDisable()
    {
        GuildService.Instance.OnGuildListResult -= UpdateGuildList;
    }

    private void UpdateGuildList(List<NGuildInfo> guilds)
    {
        ClearList();
        InitItems(guilds);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildItem;
        this.uiInfo.Info = this.selectedItem.Info;
    }

    /// <summary>
    /// ��ʼ�������б�
    /// </summary>
    /// <param name="guilds"></param>
    private void InitItems(List<NGuildInfo> guilds)
    {
        foreach (var item in guilds)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildItem ui = go.GetComponent<UIGuildItem>();
            ui.SetGuildInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    /// <summary>
    /// �������ᰴť
    /// </summary>
    public void OnClickSearch()
    {
        if (this.searchInput.text.Length > 0)
        {
            GuildService.Instance.SendGuildSearch(searchInput.text);
        }
        else
        {
            MessageBox.Show("������Ҫ�����Ĺ�����", "��������", MessageBoxType.Information);
        }
    }

    /// <summary>
    /// ���������ɹ��¼�
    /// </summary>
    /// <param name="guild"></param>
    public void OnSearchEvent(NGuildInfo guild)
    {
        ClearList();
        GameObject go = Instantiate(itemPrefab, this.listMain.transform);
        UIGuildItem ui = go.GetComponent<UIGuildItem>();
        ui.SetGuildInfo(guild);
        this.listMain.AddItem(ui);
    }

    /// <summary>
    /// ���빫�ᰴť
    /// </summary>
    public void OnClickJoin()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ����Ĺ���", "����", MessageBoxType.Error);
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ���빫�� [{0}] ��?", selectedItem.Info.GuildName), "������빫��", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinRequest(this.selectedItem.Info.Id);
        };
    }
}
