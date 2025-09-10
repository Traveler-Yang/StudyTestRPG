using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour
{
    public Image mainImage;
    public Image secondImage;

    public TextMeshProUGUI mainText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ������Ʒͼ��
    /// </summary>
    /// <param name="iconName">ͼ�����ƣ�ID��</param>
    /// <param name="text">��Ʒ����</param>
    public void SetMainIcon(string iconName, string text)
    {
        this.mainImage.overrideSprite = Resloader.Load<Sprite>(iconName);
        if (this.mainText != null) this.mainText.text = text;
    }
}
