using UnityEngine;

namespace XXLFramework.Game
{
    public class Log : LivingBeling
    {
        private Animator animator;
        
        void Start()
        {
            // 调用父类Start方法初始化生命值
            base.Start();
            animator = GetComponent<Animator>();
            
            // 注册生命值变化事件
            OnHealthChanged += OnHealthChange;
            OnDied += OnLogDied;
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
        private void OnLogDied()
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
        
        // 重写父类的Die方法
        protected override void Die()
        {
            base.Die(); // 调用父类的Die方法触发事件
            OnLogDied(); // 执行Log特有的死亡逻辑
        }
    }
}