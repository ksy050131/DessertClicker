using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Shop Panel")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button shopOpenButton;
    [SerializeField] private Button shopCloseButton;

    [Header("Tab Buttons")]
    [SerializeField] private Button upgradeTabButton;
    [SerializeField] private Button decoTabButton;

    [Header("Scroll Content")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform upgradeContent;
    [SerializeField] private RectTransform decoContent;

    [Header("Tab Colors")]
    [SerializeField] private Color activeTabColor = new Color(1f, 0.85f, 0.65f, 1f);
    [SerializeField] private Color inactiveTabColor = new Color(0.75f, 0.65f, 0.55f, 1f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        AutoFind();

        if (shopOpenButton != null) shopOpenButton.onClick.AddListener(OpenShop);
        if (shopCloseButton != null) shopCloseButton.onClick.AddListener(CloseShop);
        if (upgradeTabButton != null) upgradeTabButton.onClick.AddListener(ShowUpgradeTab);
        if (decoTabButton != null) decoTabButton.onClick.AddListener(ShowDecoTab);

        if (shopPanel != null) shopPanel.SetActive(false);
        ShowUpgradeTab();
    }

    private void AutoFind()
    {
        if (shopPanel == null)
        {
            Transform t = transform.Find("ShopPanel");
            if (t != null) shopPanel = t.gameObject;
        }
        if (shopOpenButton == null)
        {
            Transform t = transform.Find("ShopOpenButton");
            if (t != null) shopOpenButton = t.GetComponent<Button>();
        }
        if (shopCloseButton == null && shopPanel != null)
        {
            Transform t = shopPanel.transform.Find("CloseButton");
            if (t != null) shopCloseButton = t.GetComponent<Button>();
        }
        if (scrollRect == null && shopPanel != null)
        {
            Transform t = shopPanel.transform.Find("ScrollArea");
            if (t != null) scrollRect = t.GetComponent<ScrollRect>();
        }
        if (upgradeTabButton == null && shopPanel != null)
        {
            Transform t = shopPanel.transform.Find("TabBar/UpgradeTabButton");
            if (t != null) upgradeTabButton = t.GetComponent<Button>();
        }
        if (decoTabButton == null && shopPanel != null)
        {
            Transform t = shopPanel.transform.Find("TabBar/DecoTabButton");
            if (t != null) decoTabButton = t.GetComponent<Button>();
        }
        if (scrollRect != null)
        {
            // Content는 ScrollArea/Viewport 안에 위치
            Transform viewport = scrollRect.viewport != null
                ? scrollRect.viewport
                : scrollRect.transform.Find("Viewport");
            Transform searchParent = viewport != null ? viewport : scrollRect.transform;

            if (upgradeContent == null)
            {
                Transform t = searchParent.Find("UpgradeContent");
                if (t != null) upgradeContent = t.GetComponent<RectTransform>();
            }
            if (decoContent == null)
            {
                Transform t = searchParent.Find("DecoContent");
                if (t != null) decoContent = t.GetComponent<RectTransform>();
            }
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null) shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    public void ToggleShop()
    {
        if (shopPanel != null) shopPanel.SetActive(!shopPanel.activeSelf);
    }

    public void ShowUpgradeTab()
    {
        if (upgradeContent != null) upgradeContent.gameObject.SetActive(true);
        if (decoContent != null) decoContent.gameObject.SetActive(false);

        // ScrollRect의 content를 강화 탭으로 교체
        if (scrollRect != null && upgradeContent != null)
        {
            scrollRect.content = upgradeContent;
            scrollRect.normalizedPosition = new Vector2(0, 1); // 스크롤 맨 위로
        }

        UpdateTabVisuals(true);
    }

    public void ShowDecoTab()
    {
        if (upgradeContent != null) upgradeContent.gameObject.SetActive(false);
        if (decoContent != null) decoContent.gameObject.SetActive(true);

        // ScrollRect의 content를 데코 탭으로 교체
        if (scrollRect != null && decoContent != null)
        {
            scrollRect.content = decoContent;
            scrollRect.normalizedPosition = new Vector2(0, 1);
        }

        UpdateTabVisuals(false);
    }

    private void UpdateTabVisuals(bool upgradeSelected)
    {
        SetTabColor(upgradeTabButton, upgradeSelected ? activeTabColor : inactiveTabColor);
        SetTabColor(decoTabButton, upgradeSelected ? inactiveTabColor : activeTabColor);
    }

    private void SetTabColor(Button btn, Color color)
    {
        if (btn == null) return;
        Image img = btn.GetComponent<Image>();
        if (img != null) img.color = color;
    }
}
