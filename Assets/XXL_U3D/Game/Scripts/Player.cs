using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XXLFramework.Game;

/// <summary>
/// 玩家控制脚本，处理玩家移动、攻击和动画控制
/// </summary>
public class Player : LivingBeling
{
    [Header("Control Settings")]
    [Tooltip("虚拟摇杆")]
    public DynamicJoystick Joystick;
    
    [Tooltip("动画控制器")]
    public Animator Animator;
    
    [Header("Movement Settings")]
    [Tooltip("移动速度")]
    public float moveSpeed = 5f;
    
    [Tooltip("旋转平滑度")]
    public float rotationSmoothness = 10f;
    
    [Header("Attack Settings")]
    [Tooltip("敌人检测半径")]
    public float detectionRadius = 5f;
    
    [Tooltip("敌人图层掩码")]
    public LayerMask enemyLayerMask;
    
    
    
    
    private Rigidbody rb;
    private Vector3 movement;
    private bool isMoving = false;
    
    // 动画状态常量
    private const string Idle = "idle";
    private const string Walk = "walk";
    private const string Attack = "attack";
    private const string WalkAttack = "walkAttack";
    
    private Transform nearestEnemy = null;
    
    /// <summary>
    /// 初始化组件引用
    /// </summary>
    void Start()
    {
        // 调用基类Start方法初始化生命值和攻击属性
        base.Start();
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

    /// <summary>
    /// 每帧更新输入和状态检测
    /// </summary>
    void Update()
    {
        // 获取摇杆输入
        movement = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
        // 更新移动状态
        bool wasMoving = isMoving;
        isMoving = movement.magnitude > 0.1f;
        
        // 检测附近敌人
        DetectEnemies();
        
        // 自动攻击逻辑
        HandleAutoAttack();
        
        // 检查并更新动画状态
        CheckAndChangeAnimationState();
    }
    
    /// <summary>
    /// 物理更新，处理移动和旋转
    /// </summary>
    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// 处理角色移动
    /// </summary>
    private void HandleMovement()
    {
        // 保持Y轴速度不变
        Vector3 velocity = movement * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    /// <summary>
    /// 处理角色旋转
    /// </summary>
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
    
    /// <summary>
    /// 攻击方法
    /// </summary>
    public void AttackAction()
    {
        // 使用基类的Attack方法来处理冷却时间检查和事件触发
        Attack();
        
        if (isMoving)
        {
            Animator.Play(WalkAttack);
        }
        else
        {
            Animator.Play(Attack);
        }
        
        // 执行实际攻击逻辑
        PerformAttack();
    }
    
    /// <summary>
    /// 执行攻击伤害
    /// </summary>
    private void PerformAttack()
    {
        // 使用基类的targetLayerMask而不是enemyLayerMask
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRange/2, attackRange/2, targetLayerMask);
        foreach (var collider in hitColliders)
        {
            // 使用基类方法检查和造成伤害
            LivingBeling livingBeling = collider.GetComponent<LivingBeling>();
            if (livingBeling != null && livingBeling != this)
            {
                livingBeling.TakeDamage(attackDamage);
            }
        }
    }
    
    /// <summary>
    /// 检查并更新动画状态
    /// </summary>
    private void CheckAndChangeAnimationState()
    {
        // 如果正在移动
        if (isMoving)
        {
            // 检查当前动画状态，如果不是walk或walkAttack，则播放walk
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName(Walk) && !stateInfo.IsName(WalkAttack) && !stateInfo.IsName(Attack))
            {
                Animator.Play(Walk);
            }
        }
        else
        {
            // 如果没有移动，检查当前动画状态，如果不是idle或attack，则播放idle
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName(Idle) && !stateInfo.IsName(Attack))
            {
                Animator.Play(Idle);
            }
        }
    }
    
    /// <summary>
    /// 检测附近的敌人
    /// </summary>
    private void DetectEnemies()
    {
        // 使用enemyLayerMask进行敌人检测
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);
        
        // 找到最近的敌人
        nearestEnemy = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (var collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = collider.transform;
            }
        }
    }
    
    /// <summary>
    /// 处理自动攻击逻辑
    /// </summary>
    private void HandleAutoAttack()
    {
        // 如果附近有敌人且冷却时间已过
        // 使用基类的lastAttackTime变量和attackCooldown属性
        if (nearestEnemy != null && Time.time - lastAttackTime > attackCooldown)
        {
            // 转向敌人
            Vector3 directionToEnemy = (nearestEnemy.position - transform.position).normalized;
            directionToEnemy.y = 0; // 保持在水平面上
            if (directionToEnemy != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
            }
            
            // 执行攻击
            AttackAction();
            lastAttackTime = Time.time;
        }
    }
    
    /// <summary>
    /// 在Scene视图中可视化检测范围（仅在编辑器中显示）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 绘制敌人检测范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // 绘制攻击范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange/2, attackRange/2);
    }
}
