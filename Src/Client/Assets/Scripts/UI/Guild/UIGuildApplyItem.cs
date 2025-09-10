using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillBridge.Message;
using Services;

public class UIGuildApplyItem : ListView.ListViewItem
{
    public Text nickName;//�ǳ�
    public Text @class;//ְҵ
    public TextMeshProUGUI level;//�ȼ�

    void Start()
    {
        
    }

    private NGuildApplyInfo info;
    public void SetItemInfo(NGuildApplyInfo info)
    {
        this.info = info;
        if (this.nickName != null) this.nickName.text = this.info.characterName;
        if (this.@class != null)
        {
            switch (this.info.Class)
            {
                case (int)CharacterClass.Warrior:
                    this.@class.text = "սʿ";
                    break;
                case (int)CharacterClass.Wizard:
                    this.@class.text = "��ʦ";
                    break;
                case (int)CharacterClass.Archer:
                    this.@class.text = "������";
                    break;
            }
        }
        if (this.level != null) this.level.text = string.Format("Lv: {0}", this.info.Level);
    }

    /// <summary>
    /// ͨ��
    /// </summary>
    public void OnAccept()
    {
        MessageBox.Show(string.Format("ȷ��Ҫͨ�� [{0}] �Ĺ���������", this.info.characterName), "��������", MessageBoxType.Confirm, "ͨ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendGuilJoinApply(true, this.info);
        };
    }

    /// <summary>
    /// �ܾ�
    /// </summary>
    public void OnDecline()
    {
        MessageBox.Show(string.Format("ȷ��Ҫ�ܾ� [{0}] �Ĺ���������", this.info.characterName), "��������", MessageBoxType.Confirm, "�ܾ�", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendGuilJoinApply(false, this.info);
        };
    }
}
