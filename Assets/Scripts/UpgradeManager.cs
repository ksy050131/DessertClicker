using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Definitions")]
    [SerializeField] private UpgradeData[] upgrades = new UpgradeData[]
    {
        new UpgradeData
        {
            upgradeName = "더 큰 한 입",
            description = "클릭당 점수 +1",
            baseCost = 10,
            costMultiplier = 1.15f,
            addClickPower = 1,
            addPerSecond = 0f
        },
        new UpgradeData
        {
            upgradeName = "젤리 공장",
            description = "초당 1점 자동 획득",
            baseCost = 50,
            costMultiplier = 1.2f,
            addClickPower = 0,
            addPerSecond = 1f
        }
    };

    [Header("UI Slot References")]
    [SerializeField] private UpgradeSlotUI[] upgradeSlots;

    [System.Serializable]
    public class UpgradeSlotUI
    {
        public TMP_Text nameText;
        public TMP_Text costText;
        public Button buyButton;
    }

    private void Start()
    {
        // 슬롯이 연결되지 않은 경우 자동으로 찾기
        if (upgradeSlots == null || upgradeSlots.Length == 0)
        {
            AutoFindSlots();
        }

        // 각 버튼에 구매 이벤트 연결
        for (int i = 0; i < upgradeSlots.Length && i < upgrades.Length; i++)
        {
            int index = i; // 클로저를 위한 로컬 복사
            if (upgradeSlots[i].buyButton != null)
            {
                upgradeSlots[i].buyButton.onClick.AddListener(() => TryPurchase(index));
            }
        }
        RefreshAllSlots();
    }

    /// <summary>
    /// 씬에서 UpgradeSlot 오브젝트를 자동으로 찾아 연결합니다.
    /// </summary>
private void AutoFindSlots()
    {
        upgradeSlots = new UpgradeSlotUI[upgrades.Length];
        
        // ScrollArea/Viewport/UpgradeContent 안에서 슬롯 탐색
        Transform contentPanel = transform.Find("ScrollArea/Viewport/UpgradeContent");
        if (contentPanel == null) contentPanel = transform.Find("ScrollArea/UpgradeContent");
        if (contentPanel == null) contentPanel = transform.Find("UpgradeContent");
        Transform searchRoot = contentPanel != null ? contentPanel : transform;
        
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgradeSlots[i] = new UpgradeSlotUI();
            
            Transform slotTransform = searchRoot.Find($"UpgradeSlot_{i}");
            if (slotTransform == null) continue;

            Transform nameT = slotTransform.Find("UpgradeName");
            if (nameT != null) upgradeSlots[i].nameText = nameT.GetComponent<TMP_Text>();

            Transform costT = slotTransform.Find("UpgradeCost");
            if (costT != null) upgradeSlots[i].costText = costT.GetComponent<TMP_Text>();

            Transform buyT = slotTransform.Find($"BuyButton_{i}");
            if (buyT != null) upgradeSlots[i].buyButton = buyT.GetComponent<Button>();
        }
    }

    private void Update()
    {
        // 매 프레임 버튼 활성화 상태를 갱신 (점수 변동 반영)
        RefreshButtonStates();
    }

    /// <summary>
    /// 업그레이드 구매를 시도합니다.
    /// </summary>
    public void TryPurchase(int index)
    {
        if (index < 0 || index >= upgrades.Length) return;

        UpgradeData data = upgrades[index];
        long cost = data.GetCurrentCost();

        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.SpendPoints(cost)) return;

        // 능력치 반영
        if (data.addClickPower > 0)
        {
            GameManager.Instance.AddClickPower(data.addClickPower);
        }
        if (data.addPerSecond > 0f)
        {
            GameManager.Instance.AddPerSecond(data.addPerSecond);
        }

        data.purchaseCount++;
        RefreshAllSlots();
    }

    /// <summary>
    /// 모든 슬롯의 표시 내용을 갱신합니다.
    /// </summary>
    private void RefreshAllSlots()
    {
        for (int i = 0; i < upgradeSlots.Length && i < upgrades.Length; i++)
        {
            UpgradeData data = upgrades[i];
            UpgradeSlotUI slot = upgradeSlots[i];

            if (slot.nameText != null)
            {
                slot.nameText.text = $"{data.upgradeName} (Lv.{data.purchaseCount})";
            }
            if (slot.costText != null)
            {
                slot.costText.text = $"Cost: {data.GetCurrentCost():N0}";
            }
        }
        RefreshButtonStates();
    }

    /// <summary>
    /// 점수에 따라 버튼 활성화/비활성화를 갱신합니다.
    /// </summary>
    private void RefreshButtonStates()
    {
        if (GameManager.Instance == null) return;

        for (int i = 0; i < upgradeSlots.Length && i < upgrades.Length; i++)
        {
            if (upgradeSlots[i].buyButton != null)
            {
                bool canAfford = GameManager.Instance.SweetPoints >= upgrades[i].GetCurrentCost();
                upgradeSlots[i].buyButton.interactable = canAfford;
            }
        }
    }
}
