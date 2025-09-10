using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using Common;

public class UIGuildItem : ListView.ListViewItem
{
    public Image icon;//����ͼ��
    public Text guildName;//������
    public TextMeshProUGUI memberCount;//����
    public Text leader;//�᳤

    public NGuildInfo Info;

    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.icon != null) this.icon.overrideSprite = Resources.Load<Sprite>(this.Info.GuildIcon);
        if (this.guildName != null) this.guildName.text = this.Info.GuildName;
        if (this.memberCount != null) this.memberCount.text = string.Format("{0} / {1}", this.Info.memberCount, GameDefine.GuildMaxMemberCount);
        if (this.leader != null) this.leader.text = this.Info.leaderName;
    }

}
