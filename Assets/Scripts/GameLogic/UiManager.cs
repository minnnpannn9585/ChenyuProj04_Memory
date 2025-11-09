using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // 面板引用
    public GameObject startPanel;
    public GameObject orderPanel;
    public GameObject fishingPanel;
    public GameObject sellingPanel;
    public GameObject resultPanel;

    // 订单面板元素（保留，供玩家记忆需求）
    public Transform orderContent;
    public GameObject orderItemPrefab;

    // 捕鱼面板元素
    public Slider timerSlider;
    public TextMeshProUGUI shrimpText;
    public TextMeshProUGUI clownfishText;
    public TextMeshProUGUI seaBeamText;
    public TextMeshProUGUI yellowCroakerText;

    // 售卖面板元素（核心修改）
    public TextMeshProUGUI currentCustomerText; // 仅显示顾客名称
    public TextMeshProUGUI selectedFishText; // 显示当前选中的鱼类型
    public TextMeshProUGUI selectedCountText; // 显示当前选择的数量
    // 鱼种类选择按钮（4个，对应4种鱼）
    public Button shrimpButton;
    public Button clownfishButton;
    public Button seaBeamButton;
    public Button yellowCroakerButton;

    private int currentSelectedCount = 1;
    private FishTypes currentSelectedFish; // 当前选中的鱼类型
    
    

    // 结果面板元素
    //public TextMeshProUGUI resultText;
    public Button restartButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 绑定按钮事件
        restartButton.onClick.AddListener(OnRestartGame);
        // 绑定鱼种类选择按钮事件
        shrimpButton.onClick.AddListener(() => OnSelectFishType(FishTypes.Shrimp));
        clownfishButton.onClick.AddListener(() => OnSelectFishType(FishTypes.Clownfish));
        seaBeamButton.onClick.AddListener(() => OnSelectFishType(FishTypes.SeaBeam));
        yellowCroakerButton.onClick.AddListener(() => OnSelectFishType(FishTypes.YellowCroaker));
        
    }

    // 显示开始面板（不变）
    public void ShowStartPanel()
    {
        HideAllPanels();
        startPanel.SetActive(true);
    }

    // 订单面板（显示需求，供玩家记忆）
    public void ShowOrderPanel(List<CustomerOrder> orders)
    {
        HideAllPanels();
        orderPanel.SetActive(true);

        foreach (Transform child in orderContent) Destroy(child.gameObject);

        foreach (var order in orders)
        {
            GameObject item = Instantiate(orderItemPrefab, orderContent);
            TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
            // 明确显示需求（玩家需要记住）
            text.text = $"{order.customerName}: \n{order.requiredCount} * {GetFishName(order.requiredFish)}";
        }

        Invoke(nameof(StartFishing), 7f); // 延长至7秒，给玩家记忆时间
    }

    // 捕鱼面板（不变）
    public void ShowFishingPanel()
    {
        HideAllPanels();
        fishingPanel.SetActive(true);
        UpdateBackpackUI(GameManager.Instance.backpack);
    }

    // 售卖面板（隐藏需求，仅显示顾客）
    public void ShowSellingPanel()
    {
        HideAllPanels();
        sellingPanel.SetActive(true);
        currentSelectedCount = 1; // 重置数量
    }

    // 显示当前售卖的顾客（仅显示名称，不显示需求）
    public void ShowSellingUI(CustomerOrder order)
    {
        currentCustomerText.text = $"{order.customerName}"; // 只显示顾客是谁
        // 默认选中第一种鱼（玩家可手动切换）
        currentSelectedFish = FishTypes.Shrimp;
        UpdateSelectedFishText(); // 更新选中鱼的显示
        UpdateSelectedCountText(); // 更新数量显示
    }

    // 选择鱼类型（核心新增方法）
    private void OnSelectFishType(FishTypes type)
    {
        currentSelectedFish = type;
        UpdateSelectedFishText(); // 反馈选中的鱼类型
    }

    // 更新选中鱼类型的显示
    private void UpdateSelectedFishText()
    {
        selectedFishText.text = $"{GetFishName(currentSelectedFish)}";
    }

    // 显示结果面板（不变）
    public void ShowResultPanel(bool isWin)
    {
        HideAllPanels();
        resultPanel.SetActive(true);
        //resultText.text = isWin ? "win" : "lose";
    }

    private void HideAllPanels()
    {
        startPanel.SetActive(false);
        orderPanel.SetActive(false);
        fishingPanel.SetActive(false);
        sellingPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // 其他UI更新方法（不变）
    public void UpdateFishingTimer(float normalizedTime)
    {
        timerSlider.value = normalizedTime;
    }

    public void UpdateBackpackUI(Dictionary<FishTypes, int> backpack)
    {
        shrimpText.text = $"shrimp: {backpack[FishTypes.Shrimp]}";
        clownfishText.text = $"clownfish: {backpack[FishTypes.Clownfish]}";
        seaBeamText.text = $"seabeam: {backpack[FishTypes.SeaBeam]}";
        yellowCroakerText.text = $"yellowcroakker: {backpack[FishTypes.YellowCroaker]}";
    }

    // 数量调整（不变）
    public void OnIncreaseCount()
    {
        int maxCount = GameManager.Instance.backpack[currentSelectedFish];
        currentSelectedCount = Mathf.Min(currentSelectedCount + 1, maxCount);
        UpdateSelectedCountText();
    }

    public void OnDecreaseCount()
    {
        currentSelectedCount = Mathf.Max(currentSelectedCount - 1, 1);
        UpdateSelectedCountText();
    }

    private void UpdateSelectedCountText()
    {
        selectedCountText.text = $"{currentSelectedCount}";
    }

    // 确认售卖（不变）
    public void OnConfirmSelling()
    {
        GameManager.Instance.CheckSelling(currentSelectedFish, currentSelectedCount);
    }

    // 鱼名称映射（不变）
    private string GetFishName(FishTypes type)
    {
        switch (type)
        {
            case FishTypes.Shrimp: return "Shrimp";
            case FishTypes.Clownfish: return "Clownfish";
            case FishTypes.SeaBeam: return "SeaBeam";
            case FishTypes.YellowCroaker: return "YellowCroaker";
            default: return "";
        }
    }

    // 按钮事件（不变）
    public void OnStartGame()
    {
        GameManager.Instance.SwitchState(GameState.Order);
    }

    public void OnRestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    private void StartFishing()
    {
        GameManager.Instance.SwitchState(GameState.Fishing);
    }
}