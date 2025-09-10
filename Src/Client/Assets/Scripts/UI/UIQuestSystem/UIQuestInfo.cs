using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Common.Data;
using Managers;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;//�������

    public Text[] targets;//����Ŀ���б�
    public GameObject target;

    public Text description;//��������

    public UIIconItem[] rewardItems;//������Ʒ�б�
    public GameObject rewardItem;

    public TextMeshProUGUI rewarGold;//���������
    public TextMeshProUGUI rewarExp;//����������

    public Button navButton;//������ť
    private int npc = 0;

    private void Start()
    {
        for (int i = 0; i < rewardItems.Length; i++)
        {
            rewardItems[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ��������������Ϣ
    /// </summary>
    /// <param name="quest"></param>
    internal void SetQuestInfo(Quest quest)
    {
        //����������Ϣ���ı���
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type == QuestType.Main ? "����" : "֧��", quest.Define.Name);
        //�����������Ϣ
        //���infoΪnull�����ʾ������������ʾ����
        if (quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            //���info��Ϊnull����������״̬���Ѿ���ɵģ�����ʾ������ɵĶԻ�
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.DialogFinish;
            }
        }
        if (quest.Define.Target1ID == 0)
        {
            this.target.SetActive(false);
        }
        else
        {
            this.target.SetActive(true);
            if (quest.Define.Target1 == QuestTraget.Kill)
            {
                this.targets[0].text = string.Format("����{0}ֻ{1} {2}/{3}", quest.Define.Target1Num, DataManager.Instance.Characters[quest.Define.Target1ID].Name, 1, quest.Define.Target1Num);
            }
            if (quest.Define.Target1 == QuestTraget.Item)
            {
                this.targets[0].text = "���ż��͵�������";
            }
            this.targets[1].text = "";
            this.targets[2].text = "";
        }
        SetRewardItems(quest);
        //���ý�����Һ;���
        this.rewarGold.text = quest.Define.RewardGold.ToString();
        this.rewarExp.text = quest.Define.RewardExp.ToString();

        if (quest.Info == null)
        {
            //���infoΪnull�����ʾ������������ʾ����NPC
            this.npc = quest.Define.AcceptNPC;
        }
        else if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
        {
            //���info��Ϊnull����������״̬���Ѿ���ɵģ�����ʾ�ύNPC
            this.npc = quest.Define.SubmitNPC;
        }
        this.navButton.gameObject.SetActive(this.npc > 0);

        foreach (var fitter in GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickAbandon()
    {

    }

    public void OnClickNav()
    {
        Vector3 pos = NPCManager.Instance.GetNpcPosition(this.npc);
        User.Instance.CurrentCharacterObject.StartNav(pos);
        UIManager.Instance.Close<UIQuestSystem>();
    }

    /// <summary>
    /// ������������Ϣ
    /// </summary>
    private void SetRewardItems(Quest quest)
    {
        int[] rewardItemIds = new int[] {
        quest.Define.RewardItem1,
        quest.Define.RewardItem2,
        quest.Define.RewardItem3
    };
        int[] rewardItemCounts = new int[] {
        quest.Define.RewardItem1Count,
        quest.Define.RewardItem2Count,
        quest.Define.RewardItem3Count
    };

        for (int i = 0; i < rewardItems.Length; i++)
        {
            if (rewardItems[i] == null) continue;

            if (i < rewardItemIds.Length && rewardItemIds[i] > 0)
            {
                var itemDefine = DataManager.Instance.Items[rewardItemIds[i]];
                rewardItems[i].gameObject.SetActive(true);
                rewardItems[i].SetMainIcon(itemDefine.Icon, rewardItemCounts[i].ToString());
            }
            else
            {
                rewardItems[i].gameObject.SetActive(false);
            }
        }
    }

}
