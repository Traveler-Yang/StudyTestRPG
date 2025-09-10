using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRideItem : ListView.ListViewItem
{
    public Image icon;//����ͼ��
    public Text title;//����
    public TextMeshProUGUI level;//�ȼ�

    public Image backGround;//����
    public Sprite normalBg;//��������
    public Sprite selectBg;//ѡ�б���
    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? selectBg : normalBg;
    }


    void Start()
    {
        
    }

    public Item item;

    public void SetRideItem(Item item, UIRide owner, bool equiped)
    {
        this.item = item;

        if (this.title != null) this.title.text = item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resources.Load<Sprite>(item.Define.Icon);
    }
}
