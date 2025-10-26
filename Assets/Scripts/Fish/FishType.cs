using UnityEngine;

[CreateAssetMenu(fileName = "FishType", menuName = "FishGame/FishType")]
public class FishType : ScriptableObject
{
    [Header("基本属性")]
    public string fishName; // 鱼的名称
    public Sprite fishSprite; // 鱼的精灵图
    public float moveSpeed = 2f; // 移动速度
    //public int score = 10; // 捕获后得分
    
    [Header("视觉效果")]
    //public Color tintColor = Color.white; // 颜色 tint
    public Vector2 scale = Vector2.one; // 缩放比例
}
