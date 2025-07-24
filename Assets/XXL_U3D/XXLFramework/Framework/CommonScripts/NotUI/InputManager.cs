using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace XXLFramework
{
    public enum InputTargetType
    {
        UI,
        Object3D,
        None
    }

    [Serializable]
    public class InputEventArgs
    {
        public InputTargetType targetType;
        public GameObject targetObject;
        public Vector2 position;
        public Vector3 worldPosition;
        public RaycastHit? hitInfo;

        public override string ToString()
        {
            return $"InputEventArgs: targetType={targetType}, targetObject={targetObject?.name}, position={position}, worldPosition={worldPosition}, hitInfo={hitInfo}";
        }
    }

    public class MyUnityEvent : UnityEvent<InputEventArgs> { }

    public class InputManager : MonoSingleton<InputManager>
    {
        [Header("Detection Settings")]
        [SerializeField] private LayerMask interactableLayers = ~0;

        public UnityEvent<InputEventArgs> OnClick = new MyUnityEvent();
        public UnityEvent<InputEventArgs> OnDoubleClick = new MyUnityEvent();
        public UnityEvent<InputEventArgs> OnDragStart = new MyUnityEvent();
        public UnityEvent<InputEventArgs> OnDrag = new MyUnityEvent();
        public UnityEvent<InputEventArgs> OnDragEnd = new MyUnityEvent();
        public UnityEvent<InputEventArgs> OnMouseEnter = new MyUnityEvent();
        public UnityEvent<InputEventArgs> OnMouseExit = new MyUnityEvent();

        [Header("Settings")]
        [SerializeField] private float dragThreshold = 10f;
        [SerializeField] private float doubleClickInterval = 0.3f;

        private bool isDragging;
        private Vector2 dragStartPosition;
        [SerializeField]
        private InputEventArgs currentEventArgs;
        private Camera mainCamera;

        private float lastClickTime = 0f;
        private bool isWaitingForSecondClick = false;

        // Hover 状态
        private GameObject currentHoverObject = null;
        private GameObject previousHoverObject = null;
        private InputTargetType currentHoverType = InputTargetType.None;
        private InputTargetType previousHoverType = InputTargetType.None;

        [SerializeField]
        private bool isClick; //判断是否在判断单击和双击
        
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleHover();
            HandleInput();
        }

        private void HandleHover()
        {
            if (isClick)
            {
                return;
            }
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();
            GameObject hoverObject = null;
            InputTargetType hoverType = InputTargetType.None;
            Vector3 worldPos = Vector3.zero;
            RaycastHit? hitInfo = null;

            if (isOverUI)
            {
                hoverObject = DetectUIObject();
                hoverType = InputTargetType.UI;
            }
            else
            {
                if (Detect3DObject(out RaycastHit hit))
                {
                    hoverObject = hit.collider.gameObject;
                    hoverType = InputTargetType.Object3D;
                    worldPos = hit.point;
                    hitInfo = hit;
                }
            }

            currentEventArgs = new InputEventArgs
            {
                targetType = hoverType,
                targetObject = hoverObject,
                position = Input.mousePosition,
                worldPosition = worldPos,
                hitInfo = hitInfo
            };

            if (hoverObject != previousHoverObject || hoverType != previousHoverType)
            {
                // 移出事件
                if (previousHoverObject != null)
                {
                    OnMouseExit.Invoke(new InputEventArgs
                    {
                        targetType = previousHoverType,
                        targetObject = previousHoverObject,
                        position = Input.mousePosition,
                        worldPosition = Vector3.zero,
                        hitInfo = null
                    });
                }

                // 移入事件
                if (hoverObject != null)
                {
                    OnMouseEnter.Invoke(currentEventArgs);
                }

                previousHoverObject = hoverObject;
                previousHoverType = hoverType;
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick().Forget();
            }

            if (isDragging && Input.GetMouseButton(0))
            {
                HandleDragging();
            }

            if (Input.GetMouseButtonUp(0))
            {
                HandleMouseUp();
            }
        }

        private async UniTaskVoid HandleMouseClick()
        {
            isClick = true;
            float timeSinceLastClick = Time.time - lastClickTime;

            if (isWaitingForSecondClick && timeSinceLastClick < doubleClickInterval)
            {
                isWaitingForSecondClick = false;
                lastClickTime = 0;
                OnDoubleClick.Invoke(currentEventArgs);
                return;
            }

            lastClickTime = Time.time;
            isWaitingForSecondClick = true;

            await UniTask.Delay(TimeSpan.FromSeconds(doubleClickInterval));

            if (isWaitingForSecondClick)
            {
                OnClick.Invoke(currentEventArgs);
                isWaitingForSecondClick = false;
            }

            isClick = false;
        }

        private void HandleDragging()
        {
            float currentDistance = Vector2.Distance(dragStartPosition, Input.mousePosition);
            if (currentDistance >= dragThreshold)
            {
                OnDragStart.Invoke(currentEventArgs);
                isDragging = false;
            }

            OnDrag.Invoke(currentEventArgs);
        }

        private void HandleMouseUp()
        {
            isDragging = false;
            OnDragEnd.Invoke(currentEventArgs);
        }

        private GameObject DetectUIObject()
        {
            if (EventSystem.current == null) return null;

            PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                results.Sort((a, b) => b.depth.CompareTo(a.depth));
                return results[0].gameObject;
            }

            return null;
        }

        private bool Detect3DObject(out RaycastHit hit)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, 1000, interactableLayers);
        }
    }
}
