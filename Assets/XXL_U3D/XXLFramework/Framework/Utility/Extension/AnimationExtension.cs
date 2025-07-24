using System.Collections;
using UnityEngine;
using System;
using DG.Tweening;

namespace XXLFramework
{
	public static class AnimationExtension
	{
		//获取当前正在播放的动画
		public static string GetCurrentPlayingAnimationName(this Animation animation)
		{
			foreach (AnimationState state in animation)
			{
				if (animation.IsPlaying(state.name))
					return state.name;
			}
			return null;
		}

		//得到一段AnimationClip的帧数
		public static float GetAnimationClipTotalFrame(this AnimationClip clip)
		{
			return clip.length / (1 / clip.frameRate);
		}

		//得到动画播放片段当前帧
		public static int GetAnimationCurrentFrame(this Animation animation)
		{
			var animationName = GetCurrentPlayingAnimationName(animation);
			if (animationName != null)
			{
				var currentTime = animation[animationName].normalizedTime;
				float totalFrame = animation[animationName].clip.GetAnimationClipTotalFrame();
				return (int)(Mathf.Floor(totalFrame * currentTime) % totalFrame);
			}
			return -1;
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="clip">动画片段</param>
		/// <param name="OnFinish">播放完毕回调</param>
		public static void Play(this Animation self, AnimationClip clip, Action OnFinish = null)
		{
			self.Play(clip, 0, 0, 0, OnFinish);
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="clip">动画片段</param>
		/// <param name="OnFinish">播放完毕回调</param>
		public static void Play(this Animation self, AnimationClip clip, float speed, Action OnFinish = null)
		{
			float duration = clip.length / speed;

			self.Play(clip, 0, duration, duration, OnFinish);
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="clipName">动画片段名字</param>
		/// <param name="OnFinish">播放完毕回调</param>
		public static void Play(this Animation self, string clipName, Action OnFinish = null)
		{
			self.Play(clipName, 0, 0, 0, OnFinish);
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="self">动画组件</param>
		/// <param name="clip">动画片段</param>
		/// <param name="startTime">动画开始时间</param>
		/// <param name="endTime">动画结束时间</param>
		/// <param name="duration">动画持续时间</param>
		/// <param name="finishEvent">播放完毕回调</param>
		public static void Play(this Animation self, AnimationClip clip, float startTime = 0, float endTime = 0, float duration = 0, Action finishEvent = null)
		{
			if (clip == null)
			{
				Debug.LogError($"动画为空");
				return;
			}
			clip.legacy = true;
			if (self.GetClip(clip.name) == null)
			{
				self.AddClip(clip, clip.name);
			}
			if (endTime <= 0)
			{
				endTime = clip.length;
			}

			if (duration <= 0)
			{
				duration = clip.length - startTime;
			}

			float speed = clip.length / (endTime - startTime);
			Debug.Log($"{self.name}播放{clip.name}动画片段，动画从:{startTime}处开始播放,{endTime}处结束，动画速度:{speed},总共持续时间{duration} ");

			AnimationState animationState = self[clip.name];
			animationState.time = startTime;
			animationState.speed = speed;
			self.Play(clip.name);

			DOVirtual.DelayedCall(duration, () =>
			{
				self.Stop();
				Debug.Log($"{clip.name}播放完成");
				finishEvent?.Invoke();
			});
		}

		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name="self">动画组件</param>
		/// <param name="clipName">动画片段</param>
		/// <param name="startTime">动画开始时间</param>
		/// <param name="endTime">动画结束时间</param>
		/// <param name="duration">动画持续时间</param>
		/// <param name="finishEvent">播放完毕回调</param>
		public static void Play(this Animation self, string clipName, float startTime = 0, float endTime = 0, float duration = 0, Action finishEvent = null)
		{
			self.Play(self.GetClip(clipName), startTime, endTime, duration, finishEvent);
			// if (string.IsNullOrEmpty(clipName))
			// {
			// 	Debug.LogError($"动画为空");
			// 	return;
			// }
			// AnimationClip clip = self.GetClip(clipName);
			// if (clip == null)
			// {
			// 	Debug.LogError($"Animation {self.name}不存在动画{clipName}");
			// 	return;
			// }
			// clip.legacy = true;
			// if (endTime <= 0)
			// {
			// 	endTime = clip.length;
			// }
			//
			// if (duration <= 0)
			// {
			// 	duration = clip.length - startTime;
			// }
			//
			// float speed = (endTime - startTime) / duration;
			// Debug.Log($"{self.name}播放{clip.name}动画片段，动画从:{startTime}处开始播放,{endTime}处结束，动画速度:{speed},总共持续时间{duration} ");
			//
			// AnimationState animationState = self[clip.name];
			// animationState.time = startTime;
			// animationState.speed = speed;
			// self.Play(clip.name);
			//
			// DOVirtual.DelayedCall(duration, () =>
			// {
			// 	self.Stop();
			// 	Debug.Log($"{clip.name}播放完成");
			// 	finishEvent?.Invoke();
			// });
		}

		public static AnimationStateInfo Pause(this Animation animation)
		{
			var animationName = GetCurrentPlayingAnimationName(animation);
			if (animationName != null)
			{
				var time = animation[animationName].time;
				var speed = animation[animationName].speed;
				var state = new AnimationStateInfo(animationName, time, speed);
				animation.Stop(animationName);
				return state;
			}
			return null;
		}
		public static void ResumePlay(this Animation animation, string name, float speed = 1f, AnimationStateInfo state = null)
		{
			if (state != null && name == state.name)
			{
				var animationName = state.name;
				animation[animationName].time = state.time;
				animation[animationName].speed = state.speed;
				animation.Play(animationName);
			}
			else
			{
				animation.PlayAnimationWithSpeed(name, speed);
			}
		}
		/// <summary>
		/// 动画倒置播放
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="name"></param>
		public static void ResumePlay(this Animation animation, string name)
		{
			if (animation != null)
			{
				animation[name].time = animation[name].length;
				animation[name].speed = -1;
				animation.Play(name);
			}
		}

		public static void PlayAnimationWithSpeed(this Animation animation, string animationName, float speed)
		{
			animation[animationName].speed = speed;
			animation.CrossFade(animationName);
		}

	}

	public class AnimationStateInfo
	{
		public string name;
		public float time;
		public float speed;

		public AnimationStateInfo(string name, float time, float speed)
		{
			this.name = name;
			this.time = time;
			this.speed = speed;
		}
	}
}