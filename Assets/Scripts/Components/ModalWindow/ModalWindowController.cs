using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.UI;

namespace HeavyDev
{
    public class ModalWindowController : MonoBehaviour
    {
        [SerializeField]
        float speed = 1f;

        [SerializeField]
        LeanTweenType ease = LeanTweenType.easeInOutBack;

        [SerializeField]
        public enum Position
        {
            TopLeft,
            Top,
            TopRight,
            BottomLeft,
            Bottom,
            BottomRight,
            Left,
            Center,
            Right
        };

        Position last;
        RectTransform rt;

        // Events
        public event Action OnWindowOpen;
        public event Action OnWindowClose;

        // User Actions
        public Button openButton;
        public event Action OnCloseButtonClick;
        public Button closeButton;
        public event Action OnOpenButtonClick;
        
        protected void Awake()
        {
            rt = GetComponent<RectTransform>();

            if (openButton != null)
                openButton.onClick.AddListener(() => { OnOpenButtonClick(); });

            if (closeButton != null)
                closeButton.onClick.AddListener(() => { OnCloseButtonClick(); });
        }

        public void Open(Position position = Position.Center)
        {
            MoveTo(position)
                .setOnComplete(() =>
                {
                    if (OnWindowOpen != null)
                    {
                        OnWindowOpen();
                    }
                });
        }

        public void Close(Position position = Position.Left)
        {
            MoveTo(last)
                .setOnComplete(() =>
                {
                    if (OnWindowClose != null)
                    {
                        OnWindowClose();
                    }
                });
        }

        public LTDescr MoveTo(Position position)
        {
            return LeanTween.moveLocal(gameObject, GetPosition(position), speed).setEase(ease);
        }

        public void SlideTo(Position position)
        {
            last = position;
            Open(position);
        }

        public void SetPosition(Position position)
        {
            last = position;
            gameObject.transform.localPosition = GetPosition(position);
        }

        public Vector2 GetPosition(Position position)
        {
            Vector2 p = new Vector2() { x = 0f, y = 0f };

            switch (position)
            {
                case Position.TopLeft:
                    p.x = -rt.rect.width;
                    p.y = rt.rect.height;
                    break;

                case Position.Left:
                    p.x = -rt.rect.width;
                    break;

                case Position.TopRight:
                    p.x = rt.rect.width;
                    p.y = rt.rect.height;
                    break;

                case Position.Right:
                    p.x = rt.rect.width;
                    break;

                case Position.Top:
                    p.y = rt.rect.height;
                    break;

                case Position.Bottom:
                    p.y = -rt.rect.height;
                    break;

                case Position.BottomLeft:
                    p.x = -rt.rect.width;
                    p.y = -rt.rect.height;
                    break;

                case Position.BottomRight:
                    p.x = rt.rect.width;
                    p.y = -rt.rect.height;
                    break;
            }

            return p;
        }
    }
}
