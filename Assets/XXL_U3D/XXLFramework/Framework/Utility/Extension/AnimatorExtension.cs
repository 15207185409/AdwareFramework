using UnityEngine;
using System;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;

namespace XXLFramework
{
	public static class AnimatorExtension
	{
		public static Dictionary<Animator, Tween> DicAnimatorCallbacks = new Dictionary<Animator, Tween>();


		public static void Play(this Animator self, AnimationClip clip,float playSpeed = 1, Action OnFinish = null)
		{
			if (clip == null)
			{
				Debug.LogError($"动画为空");
				return;
			}
			else
			{
				self.enabled = true;
				Debug.Log($"Animator名称{self.name}动画名称{clip.name},动画时长{clip.length}");
			}
			float clipLength = clip.length;
			if (playSpeed == 0)
			{
				Debug.LogError("播放速度不能为0");
				return;
			}
			self.speed = playSpeed;
			
			if (playSpeed > 0)
			{
				self.Play(clip.name, 0, 0);
				self.Update(0);
			}
			else
			{
				self.Play(clip.name, 0, 1);
				self.Update(1);
			}
			if (DicAnimatorCallbacks.ContainsKey(self))
			{
				DicAnimatorCallbacks[self].Kill();
				DicAnimatorCallbacks.Remove(self);
			}

			Tween tween = DOVirtual.DelayedCall(clipLength / (Mathf.Abs(playSpeed)), () =>
			{
				OnFinish?.Invoke();
			});
			DicAnimatorCallbacks.Add(self, tween);

		}

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="clipName">动画片段名字</param>
		/// <param name="OnFinish">播放完毕回调</param>
		public static void Play(this Animator self, string clipName, float playSpeed = 1, Action OnFinish = null)
		{
			if (string.IsNullOrEmpty(clipName))
			{
				Debug.LogError("动画名称为空");
				return;
			}
			else
			{
				self.enabled = true;
				Debug.Log($"Animation名称{self.name}动画名称{clipName}");
			}

			var clips = self.runtimeAnimatorController.animationClips.ToList();
			AnimationClip clip = clips.Find(item => item.name.Equals(clipName));

			float clipLength = clip.length;
			if (playSpeed == 0)
			{
				Debug.LogError("播放速度不能为0");
				return;
			}

			self.speed = playSpeed;
			self.Play(clip.name, 0, 0);

			if (DicAnimatorCallbacks.ContainsKey(self))
			{
				DicAnimatorCallbacks[self].Kill();
				DicAnimatorCallbacks.Remove(self);
			}

			Tween tween = DOVirtual.DelayedCall(clipLength / (Mathf.Abs(playSpeed)), () =>
			{
				OnFinish?.Invoke();
			});
			DicAnimatorCallbacks.Add(self, tween);
		}
		
			public static void RemoveCallback(this Animator self)
		{
			if (DicAnimatorCallbacks.ContainsKey(self))
			{
				DicAnimatorCallbacks[self].Kill();
				DicAnimatorCallbacks.Remove(self);
			}
		}

	}


}