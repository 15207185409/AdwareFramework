using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XXLFramework
{
    public class Raycast3DManager : MonoBehaviour
    {
        private static Raycast3DManager mInstance;
        public static Raycast3DManager Instance
        {
            get
            {
                if (!mInstance)
                {
                    mInstance = FindObjectOfType<Raycast3DManager>();
                }

                if (!mInstance)
                {
                    mInstance = new GameObject(typeof(Raycast3DManager).Name).AddComponent<Raycast3DManager>();
                    DontDestroyOnLoad(mInstance);
                }
                return mInstance;
            }
        }

        /// <summary>
        /// Layer of ray.
        /// </summary>
        public LayerMask m_layerMask = 1;

        /// <summary>
        /// Max distance of ray.
        /// </summary>
        public float m_maxDistance = 100;

        /// <summary>
        /// Camera to ray.
        /// </summary>
        public Camera m_rayCamera;

        public GameObject m_hoveredGO;
        public enum HoverState
        {
            NONE,
            HOVER,
        }
        public HoverState m_hoverState = HoverState.NONE;

        // 鼠标单击、双击判断的间隔时间
        protected float m_doubleClickInterval = 0.1f;
        protected int m_clickCount = 0;
        protected bool m_isDetectingClick = false;
        // 通过鼠标按下和弹起的位置判断鼠标是否处于拖动状态
        public Vector3 m_mouseDownPos;

        /// <summary>
        /// 鼠标移入对象事件（触发一次）
        /// </summary>
        public Action<RaycastHit> m_OnPointerEnterAction;
        /// <summary>
        /// 鼠标移入对象事件（持续触发）
        /// </summary>
        public Action<RaycastHit> m_OnPointerHoverAction;
        /// <summary>
        /// 鼠标移出对象事件
        /// </summary>
        public Action m_OnPointerExitAction;
        /// <summary>
        /// 鼠标点击对象事件，int=0左键按下，int=1右键按下
        /// </summary>
        public Action<RaycastHit, int> m_OnPointerDownAction;
        /// <summary>
        /// 鼠标点击对象以后抬起事件，int=0左键按下，int=1右键按下
        /// </summary>
        public Action<RaycastHit, int> m_OnPointerUpAction;
        /// <summary>
        /// 鼠标双击模型事件，目前只支持左键双击
        /// </summary>
        public Action<RaycastHit, int> m_OnPointerDoubleClickAction;
        /// <summary>
        /// 鼠标拖拽结束以后的抬起，int=0左键，int=1右键
        /// </summary>
        public Action<RaycastHit, int> m_OnPointerEndDragAction;
        /// <summary>
        /// 鼠标持续按下且悬浮在该对象，int=0左键持续按下，int=1右键持续按下
        /// </summary>
        public Action<RaycastHit, int> m_OnPointerDowningHoverAction;
        /// <summary>
        /// 鼠标点击空白区域（非射线检测模型）事件
        /// </summary>
        public Action m_OnPointerDowningNoHoverAction;
        /// <summary>
        /// 鼠标点击空白区域抬起事件
        /// </summary>
        public Action m_OnPointerUpNoHoverAction;
        /// <summary>
        /// 鼠标点击到UI上的事件
        /// </summary>
        public Action m_IsPointerOnUIAction;

        #region 暂未引用的事件
        public Action<float> m_OnMouseScrollWheelAction;
        public Action m_NoPosInViewportAction;
        #endregion

        protected virtual void Start()
        {
            if (!m_rayCamera)
            {
                m_rayCamera = Camera.main;
            }
        }

        protected virtual void Update()
        {
            // 注意：会检测到带有Collider的3D GameObject
            //// 如果光标在UI上，则直接返回
            if (IsPointerOverGameObject())
            {
                return;
            }
            if (IsPointerOnUI("UI"))
            {
                m_IsPointerOnUIAction?.Invoke();
                return;
            }

            Vector3 mousePos = Input.mousePosition;

            RaycastHit hitInfo = new RaycastHit();
            Ray ray = m_rayCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hitInfo, m_maxDistance, m_layerMask))
            {
                if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                {
                    // 先恢复上次选择物体
                    if (m_hoveredGO != hitInfo.collider.gameObject)
                    {
                        OnPointerExit(m_hoveredGO);
                        m_hoverState = HoverState.NONE;
                    }

                    if (m_hoverState == HoverState.NONE)
                    {
                        OnPointerEnter(hitInfo);
                        m_hoveredGO = hitInfo.collider.gameObject;
                    }

                    m_hoverState = HoverState.HOVER;
                }
                else
                {
                    // 先恢复上次选择物体
                    if (m_hoveredGO != hitInfo.collider.gameObject)
                    {
                        OnPointerExit(m_hoveredGO);
                        m_hoverState = HoverState.NONE;
                    }

                    if (m_hoverState == HoverState.NONE)
                    {
                        OnPointerEnter(hitInfo);
                        m_hoveredGO = hitInfo.collider.gameObject;
                    }

                    m_hoverState = HoverState.HOVER;

                    if (Input.GetMouseButton(0))
                    {
                        OnPointerDowningHover(hitInfo, 0);
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        OnPointerDowningHover(hitInfo, 1);
                    }
                }
            }
            else
            {
                if (m_hoverState == HoverState.HOVER)
                {
                    OnPointerExit(m_hoveredGO);
                }
                m_hoverState = HoverState.NONE;
                m_hoveredGO = null;

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    m_mouseDownPos = Input.mousePosition;
#if UNITY_EDITOR
                    Debug.Log("点空气！！！！");
#endif
                }
            }



            if (m_hoverState == HoverState.HOVER)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_mouseDownPos = Input.mousePosition;
                    if (IsPointerOnUI("UI"))
                    {
                        return;
                    }

                    m_clickCount++;
                    if (!m_isDetectingClick)
                    {
                        StartCoroutine(DetectClick(hitInfo, 0));
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (IsPointerOverGameObject())
                    {
                        return;
                    }
                    OnPointerUp(hitInfo, 0);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    m_mouseDownPos = Input.mousePosition;
                    OnPointerDown(hitInfo, 1);
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    if (IsPointerOverGameObject())
                    {
                        return;
                    }
                    OnPointerUp(hitInfo, 1);
                }
                else
                {
                    OnPointerHover(hitInfo);
                }
            }
            else if (m_hoverState == HoverState.NONE)
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
#if UNITY_EDITOR
                    Debug.Log("持续按下空气！！！！");
#endif
                    m_OnPointerDowningNoHoverAction?.Invoke();
                }
                else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
#if UNITY_EDITOR
                    Debug.Log("空气抬起！！！！");
#endif
                    m_OnPointerUpNoHoverAction?.Invoke();
                }
            }
        }

        public virtual bool IsPointerOnUI(string UILayerName)
        {
            bool isPointerOnUI = false;
            if (IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                for (int i = 0; i < results.Count; ++i)
                {
                    if (results[i].gameObject.layer == LayerMask.NameToLayer(UILayerName))
                    {
                        isPointerOnUI = true;
                        break;
                    }
                }
            }

            return isPointerOnUI;
        }

        protected IEnumerator DetectClick(RaycastHit hitInfo, int button)
        {
            m_isDetectingClick = true;
            yield return new WaitForSeconds(m_doubleClickInterval);

            if (m_clickCount >= 2)
            {
                OnPointerDoubleClick(hitInfo, button);
            }
            else if (m_clickCount == 1)
            {
                OnPointerDown(hitInfo, button);
            }

            m_clickCount = 0;
            m_isDetectingClick = false;
        }

        /// <summary>
        /// Detect if Mouse is in specified camera-viewport
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public bool isPosInViewport(Camera camera, Vector3 screenPosition)
        {
            bool isIn = camera.pixelRect.Contains(screenPosition);
            return isIn;
        }

        /// <summary>
        /// 鼠标移入对象（仅触发一次）
        /// </summary>
        /// <param name="hitInfo"></param>
        protected virtual void OnPointerEnter(RaycastHit hitInfo)
        {
#if UNITY_EDITOR
            Debug.LogFormat("Pointer enter: {0}", hitInfo.collider.gameObject.name);
#endif
            m_OnPointerEnterAction?.Invoke(hitInfo);
        }

        /// <summary>
        /// 鼠标持续移入对象
        /// </summary>
        /// <param name="hitInfo"></param>
        protected virtual void OnPointerHover(RaycastHit hitInfo)
        {
#if UNITY_EDITOR
            Debug.LogFormat("Pointer Hover: {0}", hitInfo.collider.gameObject.name);
#endif
            m_OnPointerHoverAction?.Invoke(hitInfo);
        }

        /// <summary>
        /// 鼠标移出对象
        /// </summary>
        /// <param name="exitGameObject"></param>
        protected virtual void OnPointerExit(GameObject exitGameObject)
        {
            if (!exitGameObject)
            {
                return;
            }

#if UNITY_EDITOR
            Debug.LogFormat("Pointer exit: {0}", exitGameObject.name);
#endif
            m_OnPointerExitAction?.Invoke();
        }

        protected virtual void OnPointerDown(RaycastHit hitInfo, int button)
        {
#if UNITY_EDITOR
            Debug.LogFormat("Pointer [{0}] down on: {1}", button, hitInfo.collider.gameObject.name);
#endif
            m_OnPointerDownAction?.Invoke(hitInfo, button);
        }

        protected virtual void OnPointerUp(RaycastHit hitInfo, int button)
        {
            // 判断鼠标是否处于拖动状态
            Vector3 deltaVec = Input.mousePosition - m_mouseDownPos;
            if (deltaVec.sqrMagnitude >= 4)
            {
                m_OnPointerEndDragAction?.Invoke(hitInfo, button);
#if UNITY_EDITOR
                Debug.LogFormat("Pointer [{0}] up with dragged.", button);
#endif
            }

#if UNITY_EDITOR
            Debug.LogFormat("Pointer [{0}] up on: {1}", button, hitInfo.collider.gameObject.name);
#endif

            m_OnPointerUpAction?.Invoke(hitInfo, button);
        }


        protected virtual void OnPointerDoubleClick(RaycastHit hitInfo, int button)
        {
#if UNITY_EDITOR
            Debug.LogFormat("Pointer [{0}] double click on: {1}", button, hitInfo.collider.gameObject.name);
#endif
            m_OnPointerDoubleClickAction?.Invoke(hitInfo, button);
        }

        /// <summary>
        /// 持续按下且悬浮
        /// </summary>
        protected virtual void OnPointerDowningHover(RaycastHit hitInfo, int button)
        {
#if UNITY_EDITOR
            Debug.LogFormat("Pointer [{0}] Downing Hover with dragged.", button);
#endif
            m_OnPointerDowningHoverAction?.Invoke(hitInfo, button);
        }

        protected void Clear()
        {
            m_hoveredGO = null;
            m_hoverState = HoverState.NONE;

            m_clickCount = 0;
            m_isDetectingClick = false;
        }

        private bool IsPointerOverGameObject()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}