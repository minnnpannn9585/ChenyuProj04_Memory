using UnityEngine;


public class Fish : MonoBehaviour
{
    public FishTypes type;
    private FishType fishType;
    private bool movingRight;
    public SpriteRenderer spriteRenderer;

    public void Initialize(FishType type, bool moveRight)
    {
        fishType = type;
        movingRight = moveRight;
        
        // 应用鱼的视觉属性
        spriteRenderer.sprite = type.fishSprite;
        transform.GetChild(0).localScale = type.scale;
        
        // 如果向左移动，翻转精灵
        if (!movingRight)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Update()
    {
        // 移动逻辑
        float direction = movingRight ? -1 : 1;
        transform.Translate(Vector2.right * direction * fishType.moveSpeed * Time.deltaTime);
    }
    
    public FishType GetFishType()
    {
        return fishType;
    }
}