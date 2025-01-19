using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [HideInInspector] public Button button;
    public TMP_Text title;
    public TMP_Text description;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}