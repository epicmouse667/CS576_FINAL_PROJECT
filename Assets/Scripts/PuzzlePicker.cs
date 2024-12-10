using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞的对象是否为玩家
        if (other.CompareTag("Player"))
        {
            // 调用游戏管理器的方法增加计数
            GameManager.Instance.IncrementPuzzleCount();

            // 销毁拼图块
            Destroy(gameObject);
        }
    }
}
