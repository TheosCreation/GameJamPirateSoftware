using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.ProbeTouchupVolume;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/ScalableUpgrade")]
public class ScalableUpgrade : ScriptableObject
{
    public string title = "";               // Title of the upgrade
    public string description = "";         // Description of the upgrade
    public UpgradeType type = UpgradeType.MaxHealth;           // Type of the upgrade
    public float baseValue = 1f;            // Base value of the upgrade
    public ScalingType scalingType = ScalingType.Linear;    // How the upgrade scales
    public float scalingFactor = 1f;        // Multiplier or factor for scaling
    public Rarity rarity = Rarity.Common;
    public void ApplyUpgrade(PlayerController player, int existingCount)
    {
        float effectiveValue = 0f;

        switch (type)
        {
            case UpgradeType.BonusDamageToBosses:
                break;

            case UpgradeType.MaxHealth:
                effectiveValue = CalculateEffectiveValue(existingCount, player.maxHealth);
                player.maxHealth += effectiveValue;
                break;

            case UpgradeType.AttackSpeedBoost:
                foreach (Sword sword in player.swords)
                {
                    effectiveValue = CalculateEffectiveValue(existingCount, sword.rotationSpeed);
                    sword.rotationSpeed += effectiveValue;
                }
                break;

            case UpgradeType.MovementSpeed:
                effectiveValue = CalculateEffectiveValue(existingCount, player.moveSpeed);
                player.moveSpeed += effectiveValue;
                break;
            
            case UpgradeType.SwordSize:
                foreach (Sword sword in player.swords)
                {
                    Vector3 additionalScale = Vector3.zero;
                    additionalScale.x = CalculateEffectiveValue(existingCount, sword.transform.localScale.x);
                    additionalScale.y = CalculateEffectiveValue(existingCount, sword.transform.localScale.y);

                    sword.transform.localScale += additionalScale;
                }
                break;
            case UpgradeType.SwordCount:
                player.AddSword();
                break;
  

            default:
                Debug.LogWarning("Unknown upgrade type.");
                break;
        }
        player.Heal(Mathf.Infinity); // fulll healtyh
    }

    public Color GetColorByRarity()
    {
        switch (rarity)
        {
            case Rarity.Common:
                return Color.gray;
            case Rarity.Uncommon:
                return Color.green;
            case Rarity.Rare:
                return Color.blue;
            case Rarity.Epic:
                return new Color(0.5f, 0, 0.5f); // Purple
            case Rarity.Legendary:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    private float CalculateEffectiveValue(int existingCount, float currentValue = 0)
    {
        switch (scalingType)
        {
            case ScalingType.Linear:
                if(existingCount == 0)
                {
                    return baseValue;
                }
                return scalingFactor;
            case ScalingType.Hyperbolic:
                if (existingCount == 0)
                    return baseValue;
                return baseValue * (1 - Mathf.Pow(0.5f, existingCount));
            case ScalingType.Exponential:
                if (existingCount == 0)
                    return baseValue;
                return baseValue * Mathf.Pow(1 + scalingFactor, existingCount - 1);
            case ScalingType.Percentage:
                // Apply percentage increase based on the original value
                return currentValue * scalingFactor;

            default:
                return baseValue;
        }
    }
}

public enum ScalingType
{
    Linear,       // Base + Factor * (Stacks - 1)
    Hyperbolic,   // Base * (1 - 0.5^Stacks)
    Exponential,   // Base * (1 + Factor)^(Stacks - 1)
    Percentage
}

public enum UpgradeType
{
    BonusDamageToBosses, // could we do bosses
    MaxHealth,
    AttackSpeedBoost,
    MovementSpeed,
    SwordSize,
    SwordCount
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
