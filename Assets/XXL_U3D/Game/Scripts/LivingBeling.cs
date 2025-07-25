using UnityEngine;

namespace XXLFramework.Game
{
    public abstract class LivingBeling : MonoBehaviour
    {
        public float MaxHealth;
        // 添加当前生命值字段
        public float CurrentHealth;
        
        // 添加生命值变化事件
        public System.Action<float, float> OnHealthChanged;
        // 添加死亡事件
        public System.Action OnDied;
        
        // Start is called before the first frame update
        void Start()
        {
            // 初始化当前生命值为最大生命值
            CurrentHealth = MaxHealth;
        }
        
        // 添加受伤方法
        public virtual void TakeDamage(float damage)
        {
            if (damage <= 0) return;
            
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            
            // 触发生命值变化事件
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            
            // 检查是否死亡
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
        
        // 添加治疗方法
        public virtual void Heal(float amount)
        {
            if (amount <= 0) return;
            
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            
            // 触发生命值变化事件
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        // 添加死亡处理方法
        protected virtual void Die()
        {
            // 触发死亡事件
            OnDied?.Invoke();
        }
    }
}