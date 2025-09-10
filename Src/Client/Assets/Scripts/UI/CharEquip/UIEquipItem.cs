using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Managers;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{

    public Image icon;
    public Text title;
    public TextMeshProUGUI level;
    public Text limitClass;
    public Text limitCategory;

    public Image backGround;
    public Sprite normalBg;
    public Sprite selectBg;


    private bool selected;
    /// <summary>
    /// �Ƿ�ѡ��
    /// </summary>
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            //ÿ�θ�ֵ�ͻ�ı䵱ǰ����ͼ
            backGround.overrideSprite = selected ? selectBg : normalBg;

        }
    }

    public int Index { get; set; }
    private UICharEquip owner;

    private Item item;

    bool isEquiped = false;

    internal void SetEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
    {
        this.owner = owner;
        this.equip = owner;
        this.Index = idx;
        this.item = item;
        this.isEquiped = equiped;

        if (this.title != null) this.title.text = item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.limitClass != null)
        {
            switch (item.Define.LimitClass)
            {
                case SkillBridge.Message.CharacterClass.Warrior:
                    this.limitClass.text = "սʿ";
                    break;
                case SkillBridge.Message.CharacterClass.Wizard:
                    this.limitClass.text = "��ʦ";
                    break;
                case SkillBridge.Message.CharacterClass.Archer:
                    this.limitClass.text = "������";
                    break;
            }
        }
        if (this.limitCategory != null) this.limitCategory.text = item.Define.Category;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(item.Define.Icon);
    }

    private UICharEquip equip;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else
            {
                this.Selected = true;
            }
            equip.SelectEquipItem(this);
        }
    }

    private void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("Ҫװ��[{0}]��", this.item.Define.Name), "ȷ��", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
            if (oldEquip != null)
            {
                var newmsg = MessageBox.Show(string.Format("Ҫ�滻��[{0}]��", this.item.Define.Name), "ȷ��", MessageBoxType.Confirm);
                newmsg.OnYes = () =>
                {
                    this.owner.DoEquip(this.item);
                };
            }
            else
            {
                this.owner.DoEquip(this.item);
            }
        };
    }

    private void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("Ҫȡ��װ��[{0}]��", this.item.Define.Name), "ȷ��", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }
}
