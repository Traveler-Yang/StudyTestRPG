using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIWindow
{
    public Toggle toggleMusic;//���ֿ���
    public Toggle toggleSound;//��Ч����

    public Slider sliderMusic;//���ֻ���
    public Slider sliderSound;//��Ч����

    private bool isInit = false;

    private void Start()
    {
        isInit = true;

        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;

        isInit = false;
    }

    /// <summary>
	/// ���ؽ�ɫѡ��ť
	/// </summary>
	public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");//�л�����Ϊ��ɫѡ��
        SoundManager.Instance.PlaySound(SoundDefine.Music_Select);//���Ž�ɫѡ������
        UserService.Instance.SendGameLeave();//���ͽ�ɫ�뿪����Ϣ��������
    }

    /// <summary>
    /// ���÷ֱ���
    /// </summary>
    public void OnClickSetResolution()
    {

    }

    /// <summary>
    /// �˳���Ϸ��ť
    /// </summary>
    public void OnClickExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }

    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Btn_1);
        PlayerPrefs.Save();
        base.OnYesClick();
    }

    public void MusicVolume(float vol)
    {
        if (isInit) return; // �����ڳ�ʼ��ʱ�����¼�
        Config.MusicVolume = (int)vol;
        PlaySound();
    }

    public void MusicToggle(bool on)
    {
        if (isInit) return; // �����ڳ�ʼ��ʱ�����¼�
        Config.MusicOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Btn_1);
    }

    public void SoundVolume(float vol)
    {
        if (isInit) return; // �����ڳ�ʼ��ʱ�����¼�
        Config.SoundVolume = (int)vol;
        PlaySound();
    }

    public void SoundToggle(bool on)
    {
        if (isInit) return; // �����ڳ�ʼ��ʱ�����¼�
        Config.SoundOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Btn_1);
    }

    float lastPlay = 0;
    private void PlaySound()
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}
