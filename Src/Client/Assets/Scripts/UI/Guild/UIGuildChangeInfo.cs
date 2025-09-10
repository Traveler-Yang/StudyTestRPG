using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildChangeInfo : UIWindow
{
    public ListView listMain;//ͼ���б�
    public InputField inputNotice;//���
    public UIGuildIconItem[] iconItems;
    public UIGuildIconItem selectIconItem;//��ǰѡ���ͼ��

    void Start()
    {
        this.listMain.onItemSelected += OnIconSelected;
        InitIcons();
    }

    private void OnIconSelected(ListView.ListViewItem item)
    {
        this.selectIconItem = item as UIGuildIconItem;
    }

    public override void OnYesClick()
    {
        if (string.IsNullOrEmpty(this.inputNotice.text))
        {
            MessageBox.Show("�����빫����", "����", MessageBoxType.Error);
            return;
        }
        if (this.inputNotice.text.Length < 5 || this.inputNotice.text.Length > 30)
        {
            MessageBox.Show("������Ӧ��5-30���ַ�֮��", "����", MessageBoxType.Error);
            return;
        }
        if (this.selectIconItem == null)
        {
            MessageBox.Show("��ѡ�񹫻�ͼ��", "����", MessageBoxType.Error);
            return;
        }

        GuildService.Instance.SendAdminCommand(GuildAdminCommand.ChangeInfo, this.inputNotice.text, DataManager.Instance.Icons[selectIconItem.id].Icon, User.Instance.CurrentCharacter.Id);
    }

    public void InitIcons()
    {
        for (int i = 0; i < this.iconItems.Length; i++)
        {
            iconItems[i].Init();
            this.listMain.AddItem(iconItems[i]);
        }
    }
}
