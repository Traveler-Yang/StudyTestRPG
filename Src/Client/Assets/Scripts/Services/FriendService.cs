using Network;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendService : Singleton<FriendService>, IDisposable
{
    public FriendService()
    {
        //MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnAddFriend);
    }
    public void Dispose()
    {
        //MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnAddFriend);
    }
}
