using DG.Tweening;
using Suntail;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    //����RectTransform
    [SerializeField] private RectTransform handleRectTransform;

    //���ֹر���ɫ
    [SerializeField] private Color _handleOffColor;

    //���ر����ر���ɫ
    [SerializeField] private Color _backgroundOffColor;

    //����ʱ��
    [SerializeField] private float _duration = 0.5f;

    //���ر�������ֵ�ͼƬ
    private Image _handleImage, _backgroundImage;

    //���ر�������ֵ�Ĭ����ɫ
    private Color _handleColor, _backgroundColor;

    //�����ƶ���λ��
    private Vector2 _handlePosition;

    private Toggle _toggle;


    private void Awake()
    {

        _toggle = GetComponent<Toggle>();
        _handleImage = handleRectTransform.GetComponent<Image>();
        _backgroundImage = handleRectTransform.parent.GetComponent<Image>();

        _handleColor = _handleImage.color;
        _backgroundColor = _backgroundImage.color;

        _handlePosition = handleRectTransform.anchoredPosition;

        _toggle.onValueChanged.AddListener(OnSwitch);

        //����Ϸ��������ʱ��� _toggle ����ĳ�ʼѡ��״̬������ѡ��״̬��ִ��OnSwitch��������
        //if (_toggle.isOn)
        //{
        //    OnSwitch(true);
        //}
    }

    //���ƿ��ض�����������
    public void OnSwitch(bool on)
    {
        handleRectTransform.DOAnchorPos(on ? _handlePosition : -_handlePosition, _duration).SetEase(Ease.InOutBack);

        _backgroundImage.DOColor(on ? _backgroundColor : _backgroundOffColor, _duration).SetEase(Ease.InOutBack);

        _handleImage.DOColor(on ? _handleColor : _handleOffColor, _duration).SetEase(Ease.InOutBack);
    }

    //����������� _toggle �� onValueChanged �¼�����ֹ�ڶ��������ٵ�����¼������� OnSwitch ����������Ǳ�ڵĴ������Դ�˷�
    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}