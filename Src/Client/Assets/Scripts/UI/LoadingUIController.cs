using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUIController : MonoBehaviour
{
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    /// <summary>
    /// ����Loading��������ʾ
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        progressSlider.value = progress;
        //progressText.text = Mathf.RoundToInt(progress * 100) + "%";
        progressText.text = string.Format("������... {0}%", Mathf.RoundToInt(progress * 100));
    }

    //public void ShowComplete()
    //{
    //    progressText.text = "������ɣ������������...";
    //}
}
