using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Start, Order, Fishing, Selling, Win, Lose }
    public GameState currentState;

    public GameObject spawnManager;

    // 固定3个顾客的配置列表（在Inspector手动添加3个订单）
    [Header("固定3个顾客的订单配置（必须添加3个）")]
    public List<CustomerOrder> configuredOrders = new List<CustomerOrder>();
    public List<CustomerOrder> orders = new List<CustomerOrder>(); // 当前使用的订单

    public Dictionary<FishTypes, int> backpack = new Dictionary<FishTypes, int>();
    private int currentSellingIndex = 0; // 当前售卖的顾客索引

    public float fishingTime = 30f; // 捕鱼时间
    private float currentFishingTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 初始化背包（4种鱼）
        backpack.Add(FishTypes.Shrimp, 0);
        backpack.Add(FishTypes.Clownfish, 0);
        backpack.Add(FishTypes.SeaBeam, 0);
        backpack.Add(FishTypes.YellowCroaker, 0);
    }

    private void Start()
    {
        // 校验订单数量（必须是3个）
        if (configuredOrders.Count != 3)
        {
            Debug.LogError("请在Inspector中配置3个顾客订单！当前数量：" + configuredOrders.Count);
            return;
        }
        SwitchState(GameState.Start);
    }

    private void Update()
    {
        if (currentState == GameState.Fishing)
        {
            currentFishingTime -= Time.deltaTime;
            UIManager.Instance.UpdateFishingTimer(currentFishingTime / fishingTime);
            if (currentFishingTime <= 0)
            {
                SwitchState(GameState.Selling);
                UIManager.Instance.ShowSellingUI(orders[currentSellingIndex]);
            }
        }
    }

    public void SwitchState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.Start:
                UIManager.Instance.ShowStartPanel();
                break;
            case GameState.Order:
                // 加载配置好的3个订单
                orders = new List<CustomerOrder>(configuredOrders);
                UIManager.Instance.ShowOrderPanel(orders);
                break;
            case GameState.Fishing:
                currentFishingTime = fishingTime;
                UIManager.Instance.ShowFishingPanel();
                //StartCoroutine(SpawnFishRoutine());
                break;
            case GameState.Selling:
                currentSellingIndex = 0; // 重置售卖索引
                UIManager.Instance.ShowSellingPanel();
                break;
            case GameState.Win:
                UIManager.Instance.ShowResultPanel(true);
                break;
            case GameState.Lose:
                UIManager.Instance.ShowResultPanel(false);
                break;
        }
    }

    // 捕鱼逻辑
    public void AddFishToBackpack(FishTypes type)
    {
        backpack[type]++;
        UIManager.Instance.UpdateBackpackUI(backpack);
    }

    // 售卖检查（验证3个顾客订单）
    public void CheckSelling(FishTypes selectedFish, int selectedCount)
    {
        CustomerOrder currentOrder = orders[currentSellingIndex];
        if (selectedFish == currentOrder.requiredFish && selectedCount == currentOrder.requiredCount)
        {
            backpack[selectedFish] -= selectedCount;
            currentSellingIndex++;

            // 检查是否完成所有3个订单
            if (currentSellingIndex >= orders.Count)
            {
                SwitchState(GameState.Win); // 全部完成则胜利
            }
            else
            {
                UIManager.Instance.ShowSellingUI(orders[currentSellingIndex]);
            }
        }
        else
        {
            SwitchState(GameState.Lose); // 错误则失败
        }
    }

    // 生成鱼（随机4种类型）
    //public GameObject fishPrefab;
    //private System.Collections.IEnumerator SpawnFishRoutine()
    //{
    //    while (currentState == GameState.Fishing)
    //    {
    //        SpawnFish();
    //        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
    //    }
    //}

    //private void SpawnFish()
    //{
    //    GameObject fish = Instantiate(fishPrefab, transform);
    //    float yPos = Random.Range(-3f, 3f); // 垂直位置范围（根据相机调整）
    //    fish.transform.position = new Vector3(10f, yPos, 0); // 从右侧生成

    //    Fish fishComp = fish.GetComponent<Fish>();
    //    fishComp.type = (FishTypes)Random.Range(0, 4); // 随机4种鱼类型

    //    Rigidbody2D rb = fish.GetComponent<Rigidbody2D>();
    //    rb.velocity = new Vector2(-Random.Range(2f, 5f), 0); // 向左移动
    //}

    // 重新开始游戏（重置背包和状态）
    public void RestartGame()
    {
        // 重置背包
        backpack[FishTypes.Shrimp] = 0;
        backpack[FishTypes.Clownfish] = 0;
        backpack[FishTypes.SeaBeam] = 0;
        backpack[FishTypes.YellowCroaker] = 0;
        // 重新进入订单阶段
        SwitchState(GameState.Order);
    }
}