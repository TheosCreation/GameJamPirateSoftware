using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class UpgradeScreen : UiPage
{
    [SerializeField] private UpgradeButton upgradePrefab;
    [SerializeField] private Transform layoutTransform; 
    [SerializeField] private List<ScalableUpgrade> availableUpgrades;
    private Dictionary<Rarity, float> rarityWeights = new Dictionary<Rarity, float>
{
    { Rarity.Common, 1.0f },
    { Rarity.Uncommon, 0.7f },
    { Rarity.Rare, 0.5f },
    { Rarity.Epic, 0.25f },
    { Rarity.Legendary, 0.1f }
};

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
            ScalableUpgrade upgrade = GetRandomUpgrade();

            UpgradeButton upgradeButton = Instantiate(upgradePrefab, layoutTransform);
            upgradeButton.GetComponent<Image>().color = upgrade.GetColorByRarity();

            upgradeButton.button.onClick.AddListener(() =>
            {
                LevelManager.Instance.Player.GiveItem(upgrade);
                SelectUpgrade();
            });
            upgradeButton.title.text = upgrade.title;
            upgradeButton.description.text = upgrade.description;
        }
    }
    private ScalableUpgrade GetRandomUpgrade()
    {
        float totalWeight = 0f;
        foreach (var upgrade in availableUpgrades)
        {
            totalWeight += rarityWeights[upgrade.rarity];
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var upgrade in availableUpgrades)
        {
            cumulativeWeight += rarityWeights[upgrade.rarity];
            if (randomValue <= cumulativeWeight)
            {
                return upgrade;
            }
        }

        return availableUpgrades[0]; // fallback
    }

    public void SelectUpgrade()
    {
        gameObject.SetActive(false);
        PauseManager.Instance.canUnpause = true;
        PauseManager.Instance.SetPaused(false);
    }

}