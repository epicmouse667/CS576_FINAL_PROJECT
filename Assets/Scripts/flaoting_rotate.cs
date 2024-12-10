using UnityEngine;

public class FloatingRotate : MonoBehaviour
{
    [Header("浮动设置")]
    [Tooltip("浮动的幅度，即上下移动的距离")]
    public float amplitude = 0.5f;

    [Tooltip("浮动的频率，即浮动的速度")]
    public float frequency = 1f;

    [Header("旋转设置")]
    [Tooltip("旋转轴向")]
    public Vector3 rotationAxis = Vector3.forward;

    [Tooltip("旋转速度（度/秒）")]
    public float rotationSpeed = 20f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 实现浮动效果
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0, newY, 0);

        // 实现旋转效果
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
