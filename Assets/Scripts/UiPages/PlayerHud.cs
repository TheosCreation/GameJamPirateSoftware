using System;
using TMPro;
using UnityEngine;

public class PlayerHud : UiPage
{
    [SerializeField] private TMP_Text m_roundCounter;
    public UiBar healthBar;

    public void UpdateRoundCounter(int round)
    {
        if (m_roundCounter != null)
        {
            m_roundCounter.text = round.ToString();
        }
    }
}