using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public struct UiButton
{
    public Button button;
    public string clickFunction;
    public UiPage[] parameters;
}