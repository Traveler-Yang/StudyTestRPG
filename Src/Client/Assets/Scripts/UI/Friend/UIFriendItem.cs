using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SkillBridge.Message;

public class UIFriendItem : ListView.ListViewItem
{
    public Image icon;//ͷ��
    public TextMeshProUGUI name;//����
    public TextMeshProUGUI level;//�ȼ�
    public TextMeshProUGUI @class;//ְҵ
    public TextMeshProUGUI status;//״̬

    public Image backGround;//����ͼ
    public Sprite normalBg;//����״̬�ı���ͼ
    public Sprite selectBg;//ѡ��״̬�ı���ͼ

    public override void onSelected(bool selected)
    {
        backGround.sprite = selected ? selectBg : normalBg;
    }

    NFriendInfo info;

    public void SetFriendInfo(NFriendInfo info)
    {
        this.info = info;
        this.name.text = this.info.Info.Name;
        this.level.text = this.info.Info.Level.ToString();
        switch (this.info.Info.Class)
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
        this.status.text = this.info.Status ? "����" : "����";
    }
}
