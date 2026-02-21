using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public string upgradeName = "New Upgrade";
    public string description = "";
    public long baseCost = 10;
    public float costMultiplier = 1.15f;
    public int addClickPower = 1;
    public float addPerSecond = 0f;
    
    [HideInInspector]
    public int purchaseCount = 0;

    /// <summary>
    /// 현재 구매 비용을 계산합니다. (baseCost * costMultiplier^purchaseCount)
    /// </summary>
    public long GetCurrentCost()
    {
        return (long)(baseCost * Mathf.Pow(costMultiplier, purchaseCount));
    }
}
