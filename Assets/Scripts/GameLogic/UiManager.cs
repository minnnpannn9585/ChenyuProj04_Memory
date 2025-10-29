using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // 面板引用
    public GameObject startPanel;
    public GameObject orderPanel;
    public GameObject fishingPanel;
    public GameObject sellingPanel;
    public GameObject resultPanel; // 胜利/失败面板

    // 订单面板元素
    public Transform orderContent; // 显示3个订单的父物体
    public GameObject orderItemPrefab; // 单个订单UI预制体

    // 捕鱼面板元素
    public Slider timerSlider; // 倒计时进度条
    public TextMeshProUGUI shrimpText; // 虾数量
    public TextMeshProUGUI clownfishText; // 小丑鱼数量
    public TextMeshProUGUI seaBeamText; // 海鲷数量
    public TextMeshProUGUI yellowCroakerText; // 黄花鱼数量

    // 售卖面板元素
    public TextMeshProUGUI currentCustomerText; // 当前顾客
    public TextMeshProUGUI requiredFishText; // 需求鱼信息
    public TextMeshProUGUI selectedCountText; // 选择的数量
    private int currentSelectedCount = 1; // 当前选择数量
    private FishTypes currentSelectedFish; // 当前选择的鱼类型

    // 结果面板元素
    public TextMeshProUGUI resultText; // 胜利/失败文本
    public Button restartButton; // 重新开始按钮

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 绑定重新开始按钮事件
        restartButton.onClick.AddListener(OnRestartGame);
    }

    // 显示开始面板
    public void ShowStartPanel()
    {
        HideAllPanels();
        startPanel.SetActive(true);
    }

    // 显示订单面板（3个顾客）
    public void ShowOrderPanel(List<CustomerOrder> orders)
    {
        HideAllPanels();
        orderPanel.SetActive(true);

        // 清空旧订单
        foreach (Transform child in orderContent) Destroy(child.gameObject);

        // 显示3个顾客的订单
        foreach (var order in orders)
        {
            GameObject item = Instantiate(orderItemPrefab, orderContent);
            TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
            text.text = $"{order.customerName}：{order.requiredCount}条{GetFishName(order.requiredFish)}";
        }

        // 5秒后进入捕鱼阶段
        Invoke(nameof(StartFishing), 5f);
    }

    // 显示捕鱼面板
    public void ShowFishingPanel()
    {
        HideAllPanels();
        fishingPanel.SetActive(true);
        UpdateBackpackUI(GameManager.Instance.backpack); // 初始化背包显示
    }

    // 显示售卖面板
    public void ShowSellingPanel()
    {
        HideAllPanels();
        sellingPanel.SetActive(true);
        currentSelectedCount = 1; // 重置选择数量
    }

    // 显示当前售卖的顾客信息
    public void ShowSellingUI(CustomerOrder order)
    {
        currentCustomerText.text = $"顾客：{order.customerName}";
        requiredFishText.text = $"需求：{order.requiredCount}条{GetFishName(order.requiredFish)}";
        currentSelectedFish = order.requiredFish; // 默认选择需求的鱼
        UpdateSelectedCountText();
    }

    // 显示结果面板（胜利/失败）
    public void ShowResultPanel(bool isWin)
    {
        HideAllPanels();
        resultPanel.SetActive(true);
        resultText.text = isWin ? "胜利！所有订单完成！" : "失败！订单错误！";
    }

    // 隐藏所有面板
    private void HideAllPanels()
    {
        startPanel.SetActive(false);
        orderPanel.SetActive(false);
        fishingPanel.SetActive(false);
        sellingPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // 更新捕鱼倒计时
    public void UpdateFishingTimer(float normalizedTime)
    {
        timerSlider.value = normalizedTime;
    }

    // 更新背包UI显示
    public void UpdateBackpackUI(Dictionary<FishTypes, int> backpack)
    {
        shrimpText.text = $"虾：{backpack[FishTypes.Shrimp]}";
        clownfishText.text = $"小丑鱼：{backpack[FishTypes.Clownfish]}";
        seaBeamText.text = $"海鲷：{backpack[FishTypes.SeaBeam]}";
        yellowCroakerText.text = $"黄花鱼：{backpack[FishTypes.YellowCroaker]}";
    }

    // 增加选择数量
    public void OnIncreaseCount()
    {
        int maxCount = GameManager.Instance.backpack[currentSelectedFish];
        currentSelectedCount = Mathf.Min(currentSelectedCount + 1, maxCount);
        UpdateSelectedCountText();
    }

    // 减少选择数量
    public void OnDecreaseCount()
    {
        currentSelectedCount = Mathf.Max(currentSelectedCount - 1, 1);
        UpdateSelectedCountText();
    }

    // 更新选择数量文本
    private void UpdateSelectedCountText()
    {
        selectedCountText.text = $"选择：{currentSelectedCount}条";
    }

    // 确认售卖
    public void OnConfirmSelling()
    {
        GameManager.Instance.CheckSelling(currentSelectedFish, currentSelectedCount);
    }

    // 获取鱼的中文名称
    private string GetFishName(FishTypes type)
    {
        switch (type)
        {
            case FishTypes.Shrimp: return "虾";
            case FishTypes.Clownfish: return "小丑鱼";
            case FishTypes.SeaBeam: return "海鲷";
            case FishTypes.YellowCroaker: return "黄花鱼";
            default: return "";
        }
    }

    // 开始游戏按钮
    public void OnStartGame()
    {
        GameManager.Instance.SwitchState(GameManager.GameState.Order);
    }

    // 重新开始按钮
    public void OnRestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    // 进入捕鱼阶段
    private void StartFishing()
    {
        GameManager.Instance.SwitchState(GameManager.GameState.Fishing);
    }
}