using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillBridge.Message;
using Assets.Scripts.UI.Temp;

public class UIFriendItem : ListView.ListViewItem
{
    public Image icon;//ͷ��ͼ��
    public Text nickName;//�ǳ�
    public TextMeshProUGUI @class;//ְҵ
    public TextMeshProUGUI Level;//�ȼ�
    public Text status;//״̬

    public Image backGround;//����
    public Sprite normalBg;//��������
    public Sprite selectBg;//ѡ�б���
    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? selectBg : normalBg;
    }

    public NFriendInfo info;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���ú�������Ϣ
    /// </summary>
    /// <param name="item"></param>
    public void SetFriendInfo(NFriendInfo item)
    {
        this.info = item;
        if (this.nickName != null) this.nickName.text = this.info.friendInfo.Name;
        if (this.icon != null) this.icon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.info.friendInfo.Class - 1];
        if (this.@class != null)
        {
            switch (this.info.friendInfo.Class)
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
                default:
                    break;
            }
        }
        if (this.Level != null) this.Level.text = string.Format("Lv: {0}", this.info.friendInfo.Level);
        if (this.status != null) this.status.text = this.info.Status == true ? "����" : "����";
    }
}
