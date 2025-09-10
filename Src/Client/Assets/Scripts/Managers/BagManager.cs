using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BagManager : Singleton<BagManager>
{

    public int Unlocked;

    public BagItem[] items;

    NBagInfo Info;

    unsafe public void Init(NBagInfo info)
    {
        this.Info = info;
        this.Unlocked = info.Unlocked;
        items = new BagItem[this.Unlocked];
        if (info.Items != null && info.Items.Length >= this.Unlocked)
        {
            Analyze(info.Items);
        }
        else
        {
            info.Items = new byte[sizeof(BagItem) * this.Unlocked];
            Reset();
        }
    }

    public void AddItem(int itemId, int count)
    {
        ushort addCount = (ushort)count;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].ItemId == itemId)
            {
                //�������ӵ�����
                ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - this.items[i].Count);
                if (canAdd >= addCount)
                {
                    this.items[i].Count += addCount;
                    addCount = 0;
                    break;
                }
                else
                {
                    this.items[i].Count += canAdd;
                    addCount -= canAdd;
                }
            }
        }
        if (addCount > 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (this.items[i].ItemId == 0)
                {
                    this.items[i].ItemId = (ushort)itemId;
                    this.items[i].Count = addCount;
                    break;
                }
            }
        }
    }

    public void RemoveItem(int itemId, int count)
    {

    }

    /// <summary>
    /// ����
    /// </summary>
    public void Reset()
    {
        int i = 0;
        foreach (var kv in ItemManager.Instance.Items)
        {
            //�����ǰ���ߵ�����С�ڵ�ǰ��Ʒ�����ѵ�����
            //�Ǿ�ֱ�ӽ���ǰ��Ʒ��id��Ҳ���Ǽ�ֵ����������䵽��������
            if (kv.Value.Count <= kv.Value.Define.StackLimit)
            {
                this.items[i].ItemId = (ushort)kv.Key;
                this.items[i].Count = (ushort)kv.Value.Count;
            }
            else//����������ѵ�����
            {
                //��¼��ǰ��Ʒ������
                int count = kv.Value.Count;
                //ѭ���жϵ�ǰ�����Ƿ�������ѵ�����
                while (count > kv.Value.Define.StackLimit)
                {
                    //��䵱ǰ���ӣ����������������ѵ����Ƶ��������磺99����
                    this.items[i].ItemId = (ushort)kv.Key;
                    this.items[i].Count = (ushort)kv.Value.Define.StackLimit;
                    //�����ɺ󣬽�����һ�����ӣ����������¼��������ȥ���ѵ�����
                    i++;
                    count -= kv.Value.Define.StackLimit;
                }
                //ֱ��������֮�������¼����������С�����ѵ�����
                //���ʣ�����Ʒ����ȫ����䵽��������
                this.items[i].ItemId = (ushort)kv.Key;
                this.items[i].Count = (ushort)count;
            }
            i++;
        }
    }

    unsafe void Analyze(byte[] data)
    {
        //����һ��byte���͵�ָ�룬��ָ��data����ĵ�һλ���ڴ��ַ
        //�����C#��Ҫ��ָ�룬����Ҫ��ôд����Ϊ��C#���ڴ��Ƕ�̬�����
        //����������ǵ��ڴ�ָ��һ�鲻��ȥ�ı�
        fixed (byte* pt = data)
        {
            //�������������и���
            for (int i = 0; i < Unlocked; i++)
            {
                //sizeof(BagItem)�ǵ�ǰ��Ʒ���ڴ�����ռ���ֽ���
                //i * sizeof(BagItem) ��������ǵ�i����Ʒ������ڴ��е���ʼƫ��
                //Ҳ������Ҫ�������ڴ�
                //��ǰ���(BagItem*)(pt + i * sizeof(BagItem))�������
                //pt����ָ�򱳰�����ĵ�һλ�ĵ�ַ
                //pt + i��Ҫ�������ٸ��ֽڿ飬�������i * sizeof(BagItem)������Ҫ�������ֽڴ�С
                //����������ǴӴӵ�һλ��ַ��ʼ���������ٸ� BagItem��С�� �ֽڿ�
                //�����õ�������ڴ棬ת��(ǿת)�� BagItem* ���ͣ��洢����
                BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                //�����ǽ�����洢���ڴ棬���뱳���ĸ���
                //�ڽ��õõ��ĵ�ַ���ݣ��洢������ʱ��ֻ�Ǹı�����ֵ�������޸����ĵ�ַ
                //��Ҳ��ΪʲôҪ�ýṹ���ԭ��
                items[i] = *item;
            }
        }
    }

    unsafe public NBagInfo GetBagInfo()
    {
        //����ͬ��
        fixed (byte* pt = Info.Items)
        {
            for (int i = 0; i < Unlocked; i++)
            {
                BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                *item = items[i];
            }
        }
        return this.Info;
    }
}
