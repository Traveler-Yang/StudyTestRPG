using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public delegate void CloseHandler(UIWindow sender, UIWindowResult result);
    public event CloseHandler OnClose; // ���ڹر��¼�

    public virtual System.Type Type { get { return this.GetType(); } }

    public GameObject Root;

    /// <summary>
    /// UI���ڵ�Ĭ��ѡ������
    /// </summary>
    public enum UIWindowResult
    {
        None,
        Yes,
        No,
    }

    /// <summary>
    /// �ر�UI����
    /// </summary>
    /// <param name="result"></param>
    public void Close(UIWindowResult result = UIWindowResult.None)
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Close);
        UIManager.Instance.Close(this.Type);
        if(this.OnClose != null)
            this.OnClose(this, result);
        this.OnClose = null; // ����¼����ģ������ڴ�й©
    }

    /// <summary>
    /// �ر�UI���ڵ�ȡ����ť����¼�
    /// </summary>
    public virtual void OnCloseClick()
    {
        this.Close();
    }

    /// <summary>
    /// �ر�UI���ڵ�ȷ�ϰ�ť����¼�
    /// </summary>
    public virtual void OnYesClick()
    {
        this.Close(UIWindowResult.Yes);
    }

    public virtual void OnNoClick()
    {
        this.Close(UIWindowResult.No);
        this.Close();
    }
}
