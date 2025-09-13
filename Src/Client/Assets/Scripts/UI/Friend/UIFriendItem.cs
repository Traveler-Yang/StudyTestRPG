using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SkillBridge.Message;

public class UIFriendItem : ListView.ListViewItem
{
    public Image icon;//头像
    public TextMeshProUGUI name;//名字
    public TextMeshProUGUI level;//等级
    public TextMeshProUGUI @class;//职业
    public TextMeshProUGUI status;//状态

    public Image backGround;//背景图
    public Sprite normalBg;//正常状态的背景图
    public Sprite selectBg;//选中状态的背景图

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
                this.@class.text = "战士";
                break;
            case CharacterClass.Wizard:
                this.@class.text = "法师";
                break;
            case CharacterClass.Archer:
                this.@class.text = "弓箭手";
                break;
        }
        this.status.text = this.info.Status ? "在线" : "离线";
    }
}
