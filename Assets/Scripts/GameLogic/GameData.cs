// 鱼的类型
public enum FishTypes { Shrimp, Clownfish, SeaBream, YellowCroaker }

// 顾客订单数据
[System.Serializable]
public class CustomerOrder
{
    public string customerName; // 顾客名称
    public FishTypes requiredFish; // 需求鱼类型
    public int requiredCount; // 需求数量
}