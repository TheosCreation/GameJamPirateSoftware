using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class UpgradeScreen : UiPage
{
    [SerializeField] private UpgradeButton upgradePrefab;
    [SerializeField] private Transform layoutTransform; 
    [SerializeField] private List<ScalableUpgrade> availableUpgrades;

    protected override void OnEnable()
    {
        // Clear previous buttons if necessary
        foreach (Transform child in layoutTransform)
        {
            Destroy(child.gameObject);
        }

        // Spawn 3 buttons with random upgrades
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, availableUpgrades.Count); 
            ScalableUpgrade upgrade = availableUpgrades[randomIndex];

            UpgradeButton upgradeButton = Instantiate(upgradePrefab, layoutTransform);

            upgradeButton.button.onClick.AddListener(() =>
            {
                LevelManager.Instance.Player.GiveItem(upgrade);
                SelectUpgrade();
            });
            upgradeButton.title.text = upgrade.title;
            upgradeButton.description.text = upgrade.description;
        }
    }

    public void SelectUpgrade()
    {
        gameObject.SetActive(false);
        PauseManager.Instance.canUnpause = true;
        PauseManager.Instance.SetPaused(false);
    }

}