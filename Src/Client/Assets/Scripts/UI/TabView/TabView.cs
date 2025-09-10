using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabView : MonoBehaviour
{
    public TabButton[] tabButtons;//��ť��
    public GameObject[] tabPages;//����ҳ

    public int index = -1;//��ǰ�ı���ҳ����


    public UnityAction<int> OnTabSelect;

    IEnumerator Start()
    {
        //��ʼʱ�������еİ�ť
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].tabView = this;//��ÿ����ť�������߸�ֵ���Լ�
            tabButtons[i].tabIndex = i;//����ť��������ֵ
        }
        yield return new WaitForEndOfFrame();//�ȴ�1֡
        SelectTab(0);//Ĭ��ѡ���һҳ��Ҳ����0
    }

    /// <summary>
    /// ѡ����һҳ����
    /// </summary>
    /// <param name="index">��������</param>
    public void SelectTab(int index)
    {
        //�����ǰ��ʾ�ı���������Ҫ�滻�ı�����һ��
        //����и���
        if (this.index != index)
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].Select(i == index);
                if (this.tabPages.Length > 0)
                    tabPages[i].SetActive(i == index);
            }

            this.index = index;

            // ���� Tab �л��¼����� UI ˢ��
            if (OnTabSelect != null)
            {
                OnTabSelect(index);
            }
        }
    }
}
