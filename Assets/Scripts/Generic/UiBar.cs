using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UiBar : MonoBehaviour
{
    private Slider barSlider;
    public TMP_Text text;
  
    private void Awake()
    {
        barSlider = GetComponent<Slider>();
        text = GetComponentInChildren<TMP_Text>();
        UpdateBar(1);
    }

    public void UpdateBar(float percentage, string _text = "")
    {
        barSlider.value = percentage;
        if (text == null) return;
        text.text = _text; 
    }
}
