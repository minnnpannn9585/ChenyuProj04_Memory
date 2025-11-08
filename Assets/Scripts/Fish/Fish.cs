using UnityEngine;


public class Fish : MonoBehaviour
{
    public FishTypes type;
    private FishType fishType;
    private bool movingRight;
    public SpriteRenderer spriteRenderer;
    
    

    public void Initialize(FishType t, bool moveRight)
    {
        type = (FishTypes)t.index;
        fishType = t;
        movingRight = moveRight;
        
        // 应用鱼的视觉属性
        spriteRenderer.sprite = t.fishSprite;
        transform.GetChild(0).localScale = t.scale;
        
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