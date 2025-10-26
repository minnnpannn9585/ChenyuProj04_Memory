using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("背包格子UI")]
    public List<InventorySlot> slots; // 必须设置4个格子（与Inventory中的fishTypesInInventory对应）

    private void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            FishType fishType = Inventory.Instance.fishTypesInInventory[i];
            slots[i].iconImage.sprite = fishType.fishSprite;
            //slots[i].iconImage.color = fishType.tintColor;
            slots[i].countText.text = "0";
        }

        // 监听背包更新事件
        Inventory.Instance.OnInventoryUpdated += UpdateInventoryUI;
    }

    // 更新背包UI显示
    private void UpdateInventoryUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            FishType fishType = Inventory.Instance.fishTypesInInventory[i];
            slots[i].countText.text = Inventory.Instance.GetFishCount(fishType).ToString();
        }
    }

    private void OnDestroy()
    {
        // 移除事件监听（防止空引用）
        if (Inventory.Instance != null)
            Inventory.Instance.OnInventoryUpdated -= UpdateInventoryUI;
    }
}

// 背包格子数据结构（用于Inspector赋值）
[System.Serializable]
public class InventorySlot
{
    public Image iconImage; // 鱼的图标
    public Text countText; // 数量文本
}
