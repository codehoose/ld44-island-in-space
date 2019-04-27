using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBehaviour : MonoBehaviour
{
    public float max = 10f;

    public Image gauge;
    public TextMeshProUGUI label;

    public void Set(float value)
    {
        var newValue = Mathf.Min(max, value);
        gauge.fillAmount = newValue / max;

        label.text = string.Format("{0:000}", value);
    }
}
