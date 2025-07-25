using UnityEngine;
using System.Collections;

namespace XXLFramework.Game
{
    public class Enemy : LivingBeling
    {
        [Header("Enemy Settings")]
        public float moveSpeed = 2f;
        public float attackRange = 1.5f;
        public float attackDamage = 5f;
        public float attackCooldown = 1.5f;
        public Transform playerTarget;
        
        [Header("Patrol Settings")]
        public bool enablePatrol = true;
        public float patrolRadius = 5f;
        public float patrolSpeed = 1f;
        public float patrolChangeInterval = 3f;
        
        private Rigidbody rb;
        private Animator animator;
        private Vector3 patrolCenter;
        private Vector3 patrolTarget;
        private float lastAttackTime;
        private float patrolTimer;
        private bool isAttacking = false;
        
        // 动画状态常量
        private const string Idle = "idle";
        private const string Walk = "walk";
        private const string Attack = "attack";
        
        protected override void Start()
        {
            base.Start(); // 调用基类Start方法初始化生命值和攻击属性
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            patrolCenter = transform.position;
            SetNewPatrolTarget();
            
            // 尝试找到玩家对象
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTarget = player.transform;
            }
            
            // 注册事件
            OnHealthChanged += OnHealthChange;
            OnDied += OnEnemyDied;
        }
        
        void Update()
        {
            if (playerTarget != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
                
                // 如果玩家在攻击范围内，攻击玩家
                if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown && !isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
                // 如果玩家在检测范围内但不在攻击范围，追击玩家
                else if (distanceToPlayer <= detectionRadius)
                {
                    ChasePlayer();
                }
                // 否则巡逻
                else if (enablePatrol)
                {
                    Patrol();
                }
            }
            else if (enablePatrol)
            {
                Patrol();
            }
        }
        
        void FixedUpdate()
        {
            // 移动逻辑已在Update中处理，这里可以添加更复杂的物理移动
        }
        
        void ChasePlayer()
        {
            if (playerTarget == null || isAttacking) return;
            
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            direction.y = 0;
            
            // 移动朝向玩家
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
            
            // 旋转朝向玩家
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
            
            // 播放行走动画
            if (animator != null)
            {
                animator.Play(Walk);
            }
        }
        
        IEnumerator AttackPlayer()
        {
            if (playerTarget == null) yield break;
            
            isAttacking = true;
            lastAttackTime = Time.time;
            
            // 播放攻击动画
            if (animator != null)
            {
                animator.Play(Attack);
            }
            
            // 等待动画播放到攻击帧时造成伤害
            yield return new WaitForSeconds(0.5f);
            
            // 检查玩家是否仍在攻击范围内
            if (playerTarget != null && Vector3.Distance(transform.position, playerTarget.position) <= attackRange)
            {
                Player player = playerTarget.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(attackDamage); // 使用基类属性
                }
            }
            
            // 等待攻击动画完成
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }
        
        void Patrol()
        {
            if (isAttacking) return;
            
            patrolTimer += Time.deltaTime;
            
            // 检查是否需要设定新的巡逻目标点
            if (patrolTimer >= patrolChangeInterval || Vector3.Distance(transform.position, patrolTarget) < 0.5f)
            {
                SetNewPatrolTarget();
                patrolTimer = 0;
            }
            
            // 移动到巡逻目标点
            Vector3 direction = (patrolTarget - transform.position).normalized;
            direction.y = 0;
            
            rb.velocity = new Vector3(direction.x * patrolSpeed, rb.velocity.y, direction.z * patrolSpeed);
            
            // 旋转朝向移动方向
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
            }
            
            // 播放行走动画
            if (animator != null)
            {
                animator.Play(Walk);
            }
        }
        
        void SetNewPatrolTarget()
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            patrolTarget = patrolCenter + new Vector3(randomCircle.x, 0, randomCircle.y);
        }
        
        // 检测范围（用于可视化）
        public float detectionRadius = 10f; // 使用public以便在Inspector中调整
        
        // 在Scene视图中可视化检测范围和攻击范围
        private void OnDrawGizmosSelected()
        {
            // 绘制检测范围
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            
            // 绘制攻击范围
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        
        // 重写死亡方法
        protected override void Die()
        {
            base.Die(); // 调用基类Die方法触发事件
            OnEnemyDied(); // 执行Enemy特有的死亡逻辑
        }
        
        // 生命值变化回调
        private void OnHealthChange(float currentHealth, float maxHealth)
        {
            // 可以在这里添加受伤效果，比如播放受伤动画
            if (animator != null)
            {
                animator.SetTrigger("Hurt"); // 假设有受伤动画触发器
            }
        }
        
        // 死亡回调
        private void OnEnemyDied()
        {
            // 播放死亡动画并销毁对象
            if (animator != null)
            {
                animator.SetTrigger("Die"); // 假设有死亡动画触发器
                Destroy(gameObject, 1f); // 1秒后销毁
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
