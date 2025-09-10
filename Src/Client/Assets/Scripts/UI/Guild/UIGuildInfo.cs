using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkillBridge.Message;
using Common;

public class UIGuildInfo : MonoBehaviour
{
    public Image guildIcon;//����ͼ��
    public Text guildName;//��������
    public TextMeshProUGUI guildID;//����ID
    public Text guildLeader;//����᳤
    public Text guildNotice;//������
    public TextMeshProUGUI memberNumber;//�����Ա

    private NGuildInfo info;
    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value; this.UpdateUI(); }
    }

    /// <summary>
    /// ˢ��UI
    /// </summary>
    private void UpdateUI()
    {
        if (this.Info == null)
        {
            this.guildName.text = "��";
            this.guildID.text = "ID: 0";
            this.guildLeader.text = "��";
            this.guildNotice.text = "";
            this.memberNumber.text = string.Format("0 / {0}", GameDefine.GuildMaxMemberCount);
        }
        else
        {
            this.guildIcon.overrideSprite = Resources.Load<Sprite>(this.info.GuildIcon);
            this.guildName.text = this.info.GuildName;
            this.guildID.text = string.Format("ID: {0}", this.info.Id);
            this.guildLeader.text = this.info.leaderName;
            this.guildNotice.text = this.info.Notice;
            this.memberNumber.text = string.Format("{0} / {1}", this.info.memberCount, GameDefine.GuildMaxMemberCount);
        }
    }
}
