using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;//�������

    public Image backGround;//�������
    public Sprite normalBg;//��������������
    public Sprite selectBg;//������ѡ�б���

    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? selectBg : normalBg;
    }

    public Quest quest;
    void Start()
    {

    }

    bool isEquiped = false;

    /// <summary>
    /// �������������Ϣ
    /// </summary>
    /// <param name="item"></param>
    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (this.title != null) this.title.text = quest.Define.Name;
    }
}
