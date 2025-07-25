using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XXLFramework.Game;

public class Player : LivingBeling
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
    private const string Attack = "attack";
    private const string WalkAttack = "walkAttack";
    
    // 敌人检测相关字段
    public float detectionRadius = 5f;
    public LayerMask enemyLayerMask;
    private bool isEnemyNearby = false;
    private Transform nearestEnemy = null;
    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    
     // 攻击相关字段
    public float attackRange = 2f;
    public float attackDamage = 10f;
    
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
        
        // 检测附近敌人
        DetectEnemies();
        
        // 自动攻击逻辑
        HandleAutoAttack();
        
        // 检查并更新动画状态
        CheckAndChangeAnimationState();
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
    
    // 攻击方法
    public void AttackAction()
    {
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
    
    // 执行攻击伤害
    private void PerformAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRange/2, attackRange/2, enemyLayerMask);
        foreach (var collider in hitColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
    
    // 检查并更新动画状态
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
    
    // 检测附近的敌人
    private void DetectEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);
        isEnemyNearby = hitColliders.Length > 0;
        
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
    
    // 处理自动攻击逻辑
    private void HandleAutoAttack()
    {
        // 如果附近有敌人且冷却时间已过
        if (isEnemyNearby && nearestEnemy != null && Time.time - lastAttackTime > attackCooldown)
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
    
    // 在Scene视图中可视化检测范围（仅在编辑器中显示）
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