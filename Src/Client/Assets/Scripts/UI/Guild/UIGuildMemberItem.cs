using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public Text nickName;//�ǳ�
    public Text @class;//ְҵ
    public TextMeshProUGUI level;//�ȼ�
    public Text duty;//ְ��
    public Text status;//״̬

    public Image backGround;//����
    public Sprite normalBg;//��������
    public Sprite selectBg;//ѡ�б���
    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? selectBg : normalBg;
    }

    public NGuildMemberInfo info;
    public void SetGuildMemberInfo(NGuildMemberInfo info)
    {
        this.info = info;
        if (this.nickName != null) this.nickName.text = this.info.charInfo.Name;
        if (this.@class != null)
        {
            switch (this.info.charInfo.Class)
            {
                case CharacterClass.Warrior:
                    this.@class.text = "սʿ";
                    break;
                case CharacterClass.Wizard:
                    this.@class.text = "��ʦ";
                    break;
                case CharacterClass.Archer:
                    this.@class.text = "������";
                    break;
            }
        }
        if (this.level != null) this.level.text = this.info.charInfo.Level.ToString();
        if (this.duty != null)
        {
            switch (this.info.Duty)
            {
                case GuildDuty.None:
                    this.duty.text = "��ͨ��Ա";
                    break;
                case GuildDuty.President:
                    this.duty.text = "�᳤";
                    break;
                case GuildDuty.VicePresident:
                    this.duty.text = "���᳤";
                    break;
            }
        }
        if (this.status != null) this.status.text = this.info.Status ? "����" : "����";
    }

    /// <summary>
    /// �����ϸ��Ϣ
    /// </summary>
    public void OnclickDetails()
    {

    }
}
