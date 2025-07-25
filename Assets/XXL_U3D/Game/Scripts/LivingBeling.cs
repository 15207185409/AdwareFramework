using UnityEngine;

namespace XXLFramework.Game
{
    /// <summary>
    /// 生物实体基类，包含生命值、攻击等基础功能
    /// </summary>
    public abstract class LivingBeling : MonoBehaviour
    {
        [Header("Health Settings")]
        [Tooltip("最大生命值")]
        public float MaxHealth;
        
        [Tooltip("当前生命值")]
        [SerializeField]
        private float currentHealth;
        public float CurrentHealth => currentHealth;
        
        [Header("Attack Settings")]
        [Tooltip("攻击伤害值")]
        public float attackDamage = 10f;
        
        [Tooltip("攻击范围")]
        public float attackRange = 1f;
        
        [Tooltip("目标图层掩码")]
        public LayerMask targetLayerMask;
        
        [Tooltip("攻击冷却时间")]
        public float attackCooldown = 1f;
        
        protected float lastAttackTime = 0f;
        
        // 添加生命值变化事件
        public System.Action<float, float> OnHealthChanged;
        // 添加死亡事件
        public System.Action OnDied;
        
        // 添加攻击事件
        public System.Action OnAttack;
        // 添加受伤事件
        public System.Action<float, Vector3> OnTakeDamage;
        // 添加治疗事件
        public System.Action<float> OnHeal;
        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            // 初始化当前生命值为最大生命值
            currentHealth = MaxHealth;
        }
        
        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="damage">伤害值</param>
        public virtual void TakeDamage(float damage)
        {
            if (damage <= 0) return;
            
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            
            // 触发生命值变化事件
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
            
            // 触发受伤事件
            OnTakeDamage?.Invoke(damage, transform.position);
            
            // 检查是否死亡
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        /// <summary>
        /// 治疗
        /// </summary>
        /// <param name="amount">治疗量</param>
        public virtual void Heal(float amount)
        {
            if (amount <= 0) return;
            
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            
            // 触发生命值变化事件
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
            
            // 触发治疗事件
            OnHeal?.Invoke(amount);
        }
        
        /// <summary>
        /// 死亡处理
        /// </summary>
        protected virtual void Die()
        {
            // 触发死亡事件
            OnDied?.Invoke();
        }
        
        /// <summary>
        /// 攻击
        /// </summary>
        public virtual void Attack()
        {
            // 检查攻击冷却时间
            if (Time.time - lastAttackTime < attackCooldown)
                return;
            
            // 触发攻击事件
            OnAttack?.Invoke();
            lastAttackTime = Time.time;
        }
        
        /// <summary>
        /// 检测范围内的目标
        /// </summary>
        /// <param name="radius">检测半径</param>
        /// <returns>检测到的碰撞体数组</returns>
        protected Collider[] DetectTargets(float radius)
        {
            return Physics.OverlapSphere(transform.position, radius, targetLayerMask);
        }
        
        /// <summary>
        /// 执行攻击
        /// </summary>
        /// <param name="damage">伤害值</param>
        /// <param name="direction">攻击方向</param>
        protected virtual void PerformAttack(float damage, Vector3 direction)
        {
            Collider[] hitColliders = Physics.OverlapSphere(
                transform.position + direction.normalized * attackRange/2, 
                attackRange/2, 
                targetLayerMask
            );
            
            foreach (var collider in hitColliders)
            {
                LivingBeling livingBeling = collider.GetComponent<LivingBeling>();
                if (livingBeling != null && livingBeling != this)
                {
                    livingBeling.TakeDamage(damage);
                }
            }
        }
        
        /// <summary>
        /// 检查是否存活
        /// </summary>
        public bool IsAlive => currentHealth > 0;
    }
}