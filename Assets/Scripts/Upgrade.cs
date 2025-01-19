using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/ScalableUpgrade")]
public class ScalableUpgrade : ScriptableObject
{
    public string title = "";               // Title of the upgrade
    public string description = "";         // Description of the upgrade
    public UpgradeType type = UpgradeType.MaxHealth;           // Type of the upgrade
    public float baseValue = 1f;            // Base value of the upgrade
    public ScalingType scalingType = ScalingType.Linear;    // How the upgrade scales
    public float scalingFactor = 1f;        // Multiplier or factor for scaling

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
                effectiveValue = CalculateEffectiveValue(existingCount, player.sword.rotationSpeed);
                player.sword.rotationSpeed += effectiveValue;
                break;

            case UpgradeType.MovementSpeed:
                effectiveValue = CalculateEffectiveValue(existingCount, player.moveSpeed);
                player.moveSpeed += effectiveValue;
                break;
            
            case UpgradeType.SwordSize:
                player.sword.transform.localScale *= 1 + scalingFactor; // we know its percentage based
                break; 
            case UpgradeType.SwordCount:
                break;

            default:
                Debug.LogWarning("Unknown upgrade type.");
                break;
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