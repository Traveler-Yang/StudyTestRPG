using Common.Data;
using Managers;
using Models;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
    public Text title;//����
    public TextMeshProUGUI money;//��Ǯ

    public GameObject shopItem;//�̶���ƷԤ����
    ShopDefine shop;
    public Transform[] itemRoot;//�̵�ҳ��ĸ��ڵ�

    void Start()
    {
        StartCoroutine(InitItems());
    }

    /// <summary>
    /// ��ʼ���̵�ĵ��߱�
    /// </summary>
    /// <returns></returns>
    IEnumerator InitItems()
    {
        int count = 0;
        int pape = 0;
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status > 0)
            {
                GameObject go = Instantiate(shopItem, itemRoot[pape]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
                count++;
                if (count >= 10)
                {
                    count = 0;
                    pape++;
                    itemRoot[pape].gameObject.SetActive(true);
                }
            }
        }
        yield return null;
    }

    /// <summary>
    /// �����̵�Ļ�������
    /// </summary>
    /// <param name="shop"></param>
    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    #region ѡ���ĸ��̵���Ʒ

    private UIShopItem selsecShopItem;
    /// <summary>
    /// ѡ����ĸ���Ʒ�����õ������Ʒ
    /// </summary>
    /// <param name="item">��ǰѡ�����Ʒ</param>
    public void SelectShopItem(UIShopItem item)
    {
        if (selsecShopItem != null)
            selsecShopItem.Selected = false;
        selsecShopItem = item;
    }

    #endregion

    /// <summary>
    /// �������
    /// </summary>
    public void OnClickBuy()
    {
        if (this.selsecShopItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ����ĵ���", "������Ʒ");
            return;
        }
        if (!ShopManager.Instance.BuyItem(this.shop.ID, this.selsecShopItem.ShopItemID))
        {
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Shop_Buy);
        }
    }
}
