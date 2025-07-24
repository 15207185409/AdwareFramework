using System;
/*******************************************************************
* Copyright(c) 2020 DefaultCompany
* All rights reserved.
*
* 文件名称: Free_Camera.cs
* 简要描述:
* 
* 创建日期: 2020/09/10
* 作者:     罗祖德
* 说明:  
******************************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace XXLFramework
{
    [RequireComponent(typeof(CharacterController))]
    public class Free_Camera : MonoBehaviour
    {
        /// <summary>
        /// 是否启用碰撞
        /// </summary>
        public bool IsCollision = true;

        /// <summary>
        /// 移动速度
        /// </summary>
        public float Moving_Speed = 0.3f;
        private float MovingSpeed;

        /// <summary>
        /// 缩放变量
        /// </summary>
        private float m_distance = 10f;

        /// <summary>
        /// 移动向量
        /// </summary>
        private Vector3 _movePos = Vector3.zero;

        /// <summary>
        /// 角色控制器
        /// </summary>
        private CharacterController m_CharacterController;

        /// <summary>
        /// 上下速度
        /// </summary>
        private float _speedScale = 30f;

        /// <summary>
        /// 是否允许滚轮缩放
        /// </summary>
        public bool m_bIsWheel = true;

        /// <summary>
        /// 滚轮移动速度
        /// </summary>
        public float WheelMoveSpeed = 30;


        //-----------------------------------------------常用參數-------------
        /// <summary>
        /// 是否锁定旋转
        /// </summary>
        public bool m_bool_isLockRotate = false;

        /// <summary>
        /// 是否锁定纵向移动
        /// </summary>
        public bool m_bool_isLockVecticalMove = false;

        /// <summary>
        /// 是否锁定横向移动
        /// </summary>
        public bool m_bool_isLockHorizontalMove = false;

        /// <summary>
        /// 是否锁定上下移动
        /// </summary>
        public bool m_bool_isLockQEMove = false;

        /// <summary>
        /// 是否使用重力
        /// </summary>
        public bool UseGravity = false;
        /// <summary>
        /// 重力加速度
        /// </summary>
        public float GravitySpeed = 3;
        //-------------------------------------------------------------------

        private void Awake()
        {
            m_RotY = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            m_RotX = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        }

        void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            // 加速
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                MovingSpeed = Moving_Speed * 5;
            }
            else
            {
                MovingSpeed = Moving_Speed;
            }
            //if (IsPointerOverGameObject(Input.mousePosition) == false)
            {
                if (Input.GetMouseButtonDown(1) && !m_bool_isLockRotate)
                {
                    m_RotY = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    m_RotX = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
                }
                // 鼠标右键点下控制相机旋转
                if (Input.GetMouseButton(1) && !m_bool_isLockRotate)
                {
                    LookRotation();
                }

                // 鼠标中键点下场景缩放
                if (m_bIsWheel && Input.GetAxis("Mouse ScrollWheel") != 0 && !Input.GetMouseButton(2))
                {              
                    m_distance = Input.GetAxis("Mouse ScrollWheel") * WheelMoveSpeed;

                    Vector3 scal = new Vector3(0, 0, m_distance);

                    scal = transform.TransformPoint(scal) - transform.transform.position;

                    m_CharacterController.Move(scal);
                }
            }

            float delta_x = 0, delta_y = 0, delta_z = 0;

            if (!m_bool_isLockVecticalMove)
                delta_x = Input.GetAxis("Vertical") * MovingSpeed;

            if (!m_bool_isLockHorizontalMove)
                delta_y = Input.GetAxis("Horizontal") * MovingSpeed;

            if (m_bool_isLockVecticalMove == false)
            {
                if (Input.GetKey(KeyCode.PageUp) || Input.GetKey(KeyCode.Q))
                {
                    delta_z = MovingSpeed * _speedScale / 40;
                }
                else if (Input.GetKey(KeyCode.PageDown) || Input.GetKey(KeyCode.E))
                {
                    delta_z = -MovingSpeed * _speedScale / 40;
                }
            }

            _movePos = new Vector3(delta_y, delta_z, delta_x);

            if (UseGravity)
            {
                _movePos.y = 0;
            }

            _movePos = transform.TransformPoint(_movePos) - transform.transform.position;

        }

        void FixedUpdate()
        {
            if (Stopped)
                return;
            if (UseGravity)
            {
                _movePos.y = Physics.gravity.y * GravitySpeed;
            }
            Vector3 targetPos;

            if (!IsCollision)
            {
                targetPos = transform.position + _movePos;
            }
            else
            {
                targetPos = _movePos * Time.fixedDeltaTime * _speedScale;
            }

            if (IsClamp)
            {
                float moveX = targetPos.x;
                float moveY = targetPos.y;
                float moveZ = targetPos.z;

                if (moveClampX != Vector2.zero)
                    moveX = Mathf.Clamp(moveX, moveClampX.x, moveClampX.y);

                if (moveClampY != Vector2.zero)
                    moveY = Mathf.Clamp(moveY, moveClampY.x, moveClampY.y);

                if (moveClampZ != Vector2.zero)
                    moveZ = Mathf.Clamp(moveZ, moveClampZ.x, moveClampZ.y);

                targetPos = new Vector3(moveX, moveY, moveZ);
            }

            if (!IsCollision)
            {
                transform.position = targetPos;
            }
            else
            {
                m_CharacterController.Move(targetPos);
            }

            // if (_movePos.magnitude > 0)
            // {
            //     _speedScale += 0.2f;
            // }
            // else
            // {
            //     _speedScale = 30f;
            // }
        }


        #region clamp
        private Vector2 moveClampX = Vector2.zero;
        private Vector2 moveClampY = Vector2.zero;
        private Vector2 moveClampZ = Vector2.zero;

        private bool IsClamp = false;

        public void SetMoveClamp(Vector2 xClamp, Vector2 yClamp, Vector2 zClamp)
        {
            IsClamp = true;

            moveClampX = xClamp;
            moveClampY = yClamp;
            moveClampZ = zClamp;
        }

        public void OffMoveClamp()
        {
            IsClamp = false;

            moveClampX = Vector2.zero;
            moveClampY = Vector2.zero;
            moveClampZ = Vector2.zero;
        }


        #endregion clamp

        /// <summary>
        /// x灵敏度
        /// </summary>
        public float XSensitivity = 2f;

        /// <summary>
        /// y灵敏度
        /// </summary>
        public float YSensitivity = 2f;

        /// <summary>
        /// 是否启用平滑
        /// [球形插值]
        /// </summary>
        public bool smooth = true;

        /// <summary>
        /// 平滑速率 
        /// 越大插值越快
        /// </summary>
        public float smoothTime = 15f;

        /// <summary>
        /// 旋转角度修正
        /// </summary>
        public bool clampVerticalRotation = true;

        /// <summary>
        /// 纵向旋转最小值
        /// </summary>
        public float MinimumX = -60F;

        /// <summary>
        /// 纵向旋转最大值
        /// </summary>
        public float MaximumX = 60F;

        /// <summary>
        ///  旋转X
        /// </summary>
        private Quaternion m_RotX;

        /// <summary>
        /// 旋转Y
        /// </summary>
        private Quaternion m_RotY;

        private Quaternion m_Rot;

        /// <summary>
        /// 视角旋转
        /// </summary>
        public void LookRotation()
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_RotY *= Quaternion.Euler(0f, yRot, 0f);
            m_RotX *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
            {
                m_RotX = ClampRotationAroundXAxis(m_RotX);
            }

            m_Rot = Quaternion.Euler(m_RotY.eulerAngles + m_RotX.eulerAngles);

            if (smooth)
            {
                this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, m_Rot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                this.transform.localRotation = m_Rot;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        /// <summary>
        /// 是否正在启用定位
        /// </summary>
        [SerializeField] private bool Stopped;
        /// <summary>
        /// 定位
        /// </summary>
        public void OnLocate(Vector3 targetPos, Vector3 targetEuler, float duration = 0)
        {
            if (duration <= 0)
            {
                Stopped = true;

                transform.position = targetPos;
                transform.rotation = Quaternion.Euler(targetEuler);

                m_RotY = Quaternion.Euler(0, targetEuler.y, 0);
                m_RotX = Quaternion.Euler(targetEuler.x, 0, 0);

                StartCoroutine(SetStartMove());
            }
            else
            {
                Stopped = true;

                transform.transform.DOMove(targetPos, duration);

                transform.DORotate(targetEuler, duration);

                if (IEWait2Action != null)
                {
                    StopCoroutine(IEWait2Action);

                    IEWait2Action = null;
                }

                IEWait2Action = StartCoroutine(Wait2Action(duration, () => { Stopped = false; }));

                m_RotY = Quaternion.Euler(0, targetEuler.y, 0);
                m_RotX = Quaternion.Euler(targetEuler.x, 0, 0);
            }

        }

        private Coroutine IEWait2Action = null;

        IEnumerator Wait2Action(float waitTime, Action action)
        {
            yield return new WaitForSecondsRealtime(waitTime);

            action?.Invoke();
        }

        /// <summary>
        /// 等待当前帧结束 关闭定位
        /// </summary>
        /// <returns></returns>
        IEnumerator SetStartMove()
        {
            yield return new WaitForFixedUpdate();

            Stopped = false;
        }

        /// <summary>
        /// 检测是否点击UI
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        private bool IsPointerOverGameObject(Vector2 mousePosition)
        {
            //创建一个点击事件
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击UI
            EventSystem.current.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count > 0)
            {
				foreach (var item in raycastResults)
				{
					if (item.gameObject.GetComponent<RectTransform>()!=null)
					{
                        return true;
					}
				}
                return false;
            }
            else
            {
                return false;
            }
        }

    }

}