using Common.Data;
using Models;
using SkillBridge.Message;
using Services;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id, item);

                Debug.LogFormat("ItemManager Init: [{0}]", item);
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        public ItemDefine GetItem(int itemId)
        {
            return null;
        }

        bool OnItemNotify(NStatus status)
        {
            if (status.Action == StatusAction.Add)
            {
                this.AddItem(status.Id, status.Value);
            }
            if (status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
            return true;
        }

        /// <summary>
        /// �����Ʒ
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        void AddItem(int itemId, int count)
        {
            Item item = null;
            //�жϵ��߱�����û�������Ʒ
            //����У���ֱ����������
            //���û�У��򹹽�һ���µģ�����ӵ�����
            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Count += count;
            }
            else
            {
                item = new Item(itemId, count);
                this.Items.Add(itemId, item);
            }
            //�����ɺ����Ҫ��ӵ�������
            BagManager.Instance.AddItem(itemId, count);
        }
        /// <summary>
        /// �Ƴ���Ʒ
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        void RemoveItem(int itemId, int count)
        {
            //�жϱ�����û�������Ʒ
            //���û�У���ֱ�ӷ��أ�ʲô������
            if (!this.Items.ContainsKey(itemId))
            {
                return;
            }
            //������������Ʒ��ȡ����
            //���ж��Ƿ���С��Ҫ�Ƴ�������
            //���С�ڣ��򷵻�
            Item item = this.Items[itemId];
            if (item.Count < count)
                return;
            //�����С�ڣ����ȥҪ�Ƴ�������
            item.Count -= count;
            //�����ɺ����Ҫ��ӵ�������
            BagManager.Instance.RemoveItem(itemId, count);
        }

        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }
    }
}
