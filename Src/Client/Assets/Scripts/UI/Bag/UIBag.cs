using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBag : UIWindow
{
    public TextMeshProUGUI money;//������ʾ��Ǯ

    public Sprite notEnableSlote;//δ�����ĸ���ͼƬ

    public Transform[] pages;//����ҳ

    public GameObject bagItem;//Ҫ��ʾ�� ������Prefab(��ʾ��ͼ����ı�)

    List<Button> slote;//�ۣ���ǰ�����ж��ٸ���

    void Start()
    {
        if (slote == null)
        {
            slote = new List<Button>();
            for (int page = 0; page < this.pages.Length; page++)
            {
                slote.AddRange(this.pages[page].GetComponentsInChildren<Button>(true));
            }
        }
        StartCoroutine(InitBags());

    }

    IEnumerator InitBags()
    {
        for (int i = 0; i < BagManager.Instance.items.Length; i++)
        {
            var item = BagManager.Instance.items[i];
            if (item.ItemId > 0)
            {
                GameObject go = Instantiate(bagItem, slote[i].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].Define;
                ui.SetMainIcon(def.Icon, item.Count.ToString());
            }
        }
        SetTitle();
        //������δ�����ĸ��ӵ�ͼ������Ϊ����ͼƬ
        for (int i = BagManager.Instance.items.Length; i < slote.Count; i++)
        {
            slote[i].transform.Find("Icon").GetComponent<Image>().sprite = notEnableSlote;
        }
        yield return null;
    }

    public void SetTitle()
    {
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    void Clear()
    {
        for (int i = 0; i < slote.Count; i++)
        {
            if (slote[i].transform.childCount > 1)
            {
                Destroy(slote[i].transform.GetChild(1).gameObject);
            }
        }
    }

    public void OnReset()
    {
        BagManager.Instance.Reset();
        this.Clear();
        StartCoroutine(InitBags());
    }
}
