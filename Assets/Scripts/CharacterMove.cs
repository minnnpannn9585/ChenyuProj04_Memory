using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float screenEdgePadding = 0.5f; // 屏幕边缘内缩距离

    private Vector2 screenBounds; // 屏幕边界
    private float playerHalfWidth;
    private float playerHalfHeight;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();
        playerHalfWidth = col.bounds.extents.x;
        playerHalfHeight = col.bounds.extents.y;
    }

    private void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, 
            Screen.height, 
            Camera.main.transform.position.z
        ));
    }

    private void FixedUpdate()
    {
        // 获取移动输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical).normalized * speed * Time.fixedDeltaTime;

        // 移动玩家
        rb.MovePosition(rb.position + movement);

        // 限制在屏幕内
        RestrictToScreen();
    }

    private void RestrictToScreen()
    {
        Vector3 clampedPos = transform.position;
        // X轴限制（左右边界）
        clampedPos.x = Mathf.Clamp(
            clampedPos.x, 
            -screenBounds.x + playerHalfWidth + screenEdgePadding, 
            screenBounds.x - playerHalfWidth - screenEdgePadding
        );
        // Y轴限制（上下边界）
        clampedPos.y = Mathf.Clamp(
            clampedPos.y, 
            -screenBounds.y + playerHalfHeight + screenEdgePadding, 
            screenBounds.y - playerHalfHeight - screenEdgePadding
        );
        transform.position = clampedPos;
    }

    // 碰撞检测（捕获鱼）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Fish>(out Fish fish))
        {
            // 通知背包捕获鱼
            Inventory.Instance.AddFish(fish.GetFishType());
            // 销毁鱼
            Destroy(fish.gameObject);
        }
    }
}
