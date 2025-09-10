using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// UI�����Ϣ
    /// </summary>
    class UIElement
    {
        public string Resource;     //��Դ·��
        public bool cache;          //�Ƿ񻺴�
        public GameObject instance; //UIʵ��
    }
    /// <summary>
    /// �洢UI��Դ��Ϣ
    /// </summary>
    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

    public UIManager()
    {
        //��ʼ��UI��Դ��Ϣ
        UIResources.Add(typeof(UIBag), new UIElement() { Resource = "UI/UIBagPrefab/UIBag", cache = false });
        UIResources.Add(typeof(UIShop), new UIElement() { Resource = "UI/UIShopPrefab/UIShop", cache = false });
        UIResources.Add(typeof(UICharEquip), new UIElement() { Resource = "UI/UIEquipPrefab/UICharEquip", cache = false });
        UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resource = "UI/UIQuestPrefab/UIQuestSystem", cache = false });
        UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resource = "UI/UIQuestPrefab/UIQuestDialog", cache = false });
        UIResources.Add(typeof(UIFriends), new UIElement() { Resource = "UI/UIFriend/UIFriends", cache = false });
        UIResources.Add(typeof(UISetting), new UIElement() { Resource = "UI/UISetting/UISetting", cache = false });
        UIResources.Add(typeof(UIGuild), new UIElement() { Resource = "UI/UIGuild/Guild/UIGuild", cache = false });
        UIResources.Add(typeof(UIGuildList), new UIElement() { Resource = "UI/UIGuild/GuildList/UIGuildList", cache = false });
        UIResources.Add(typeof(UIGuildPopNoGuild), new UIElement() { Resource = "UI/UIGuild/UIGuildPopNoGuild", cache = false });
        UIResources.Add(typeof(UIGuildPopCreate), new UIElement() { Resource = "UI/UIGuild/UIGuildPopCreate", cache = false });
        UIResources.Add(typeof(UIGuildApplyList), new UIElement() { Resource = "UI/UIGuild/GuildApply/UIGuildApplyList", cache = false });
        UIResources.Add(typeof(UIGuildChangeInfo), new UIElement() { Resource = "UI/UIGuild/UIGuildChangeInfo", cache = false });
        UIResources.Add(typeof(UIPopCharMenu), new UIElement() { Resource = "UI/UIPopCharMenu", cache = false });
        UIResources.Add(typeof(UIRide), new UIElement() { Resource = "UI/Ride/UIRide", cache = false });
        //���Լ����������UI����
    }

    /// <summary>
    /// ��UI����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Show<T>()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Open);
        //��ȡUI����
        Type type = typeof(T);
        //�ж�UI�Ƿ����
        if (UIResources.ContainsKey(type))
        {
            //������ڣ����ȡUI��Ϣ
            UIElement uiInfo = UIResources[type];
            if (uiInfo.instance != null)
            {
                //����Ѿ��򿪵ģ���ֱ������
                uiInfo.instance.SetActive(true);
            }
            else
            {
                //���û�д򿪵ģ��������Դ
                //��Resoueces�м�����Դ
                UnityEngine.Object prefab = Resources.Load(uiInfo.Resource);
                //ʵ������Prefab UI
                uiInfo.instance = (GameObject)GameObject.Instantiate(prefab);
            }
            //�������UI���س�ȥ
            return uiInfo.instance.GetComponent<T>();
        }
        return default(T);
    }

    /// <summary>
    /// �ر�UI����
    /// </summary>
    /// <param name="type">UI��Ϣ�ֵ��Keyֵ��ָ����һ��UI</param>
    public void Close(Type type)
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Close);
        //���UI�Ƿ����
        if (UIResources.ContainsKey(type))
        {
            //��ȡUI��Ϣ
            UIElement uiInfo = UIResources[type];
            if (uiInfo.cache)
            {
                //���UIʵ�����ڣ��������
                uiInfo.instance.SetActive(false);
            }
            else
            {
                //���UIʵ�������ڣ���������
                GameObject.Destroy(uiInfo.instance);
                //Ȼ��ʵ���ÿ�
                uiInfo.instance = null;
            }
        }
    }

    public void Close<T>()
    {
        //����Close����������UI����
        Close(typeof(T));
    }
}
