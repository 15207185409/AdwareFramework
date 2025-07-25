using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public DynamicJoystick Joystick;
    public Animator Animator;
    
    public float moveSpeed = 5f;
    public float rotationSmoothness = 10f; // 新增旋转平滑参数
    private Rigidbody rb;
    private Vector3 movement;
    private bool isMoving = false;
    // 动画状态常量
    private const string Idle = "idle";
    private const string Walk = "walk";
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 获取摇杆输入
        movement = new Vector3(Joystick.Horizontal,0, Joystick.Vertical);
        // 更新移动状态
        bool wasMoving = isMoving;
        isMoving = movement.magnitude > 0.1f;
        // 当移动状态变化时更新动画
        if (isMoving != wasMoving)
        {
            if (isMoving)
            {
                Animator.Play(Walk);
            }
            else
            {
                Animator.Play(Idle);
            }
        }
        
    }
    
    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // 保持Y轴速度不变
        Vector3 velocity = movement * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    private void HandleRotation()
    {
        if (!isMoving) return;
        
        // 计算目标朝向角度（XZ平面）
        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        
        // 平滑旋转朝向
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationSmoothness * Time.fixedDeltaTime
        );
    }
   
}