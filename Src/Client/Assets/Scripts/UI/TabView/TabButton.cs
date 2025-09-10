using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public Sprite activeImage;//����ʱ��ͼƬ
    private Sprite normalImage;//����ʱ��ͼƬ

    public TabView tabView;//�����ť��������

    public int tabIndex = 0;//��ť������
    public bool selected = false;

    private Image tabImage;//��ǰͼƬ

    void Start()
    {
        //����ǰ��ť���ϵĵ�ͼƬȡ��������ֵ������ʱ��ͼƬ
        tabImage = GetComponent<Image>();
        normalImage = tabImage.sprite;

        //����ǰ���ϵİ�ť���һ������¼�
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="select"></param>
    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }

    void OnClick()
    {
        this.tabView.SelectTab(this.tabIndex);
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Btn_1);
    }
}
