using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public struct UiButton
{
    public Button button;
    public string clickFunction;
    [SerializeField] private Parameter[] parameters;

    public Parameter[] Parameters => parameters;
}

[Serializable]
public class Parameter
{
    public string name;
    public object value;
}