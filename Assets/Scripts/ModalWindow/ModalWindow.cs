using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

namespace HeavyDev
{
    public class ModalWindow : MonoBehaviour
    {
        float speed = 1f;
        LeanTweenType ease = LeanTweenType.easeInOutBack;
        public enum Position { TopLeft, Top, TopRight, BottomLeft, Bottom, BottomRight, Left, Center, Right };
        Position last;
        RectTransform rt;

        // Events
        public Action WindowOpened;
        public Action WindowClosed;

        protected void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        protected void Open(Position position = Position.Center)
        {
            MoveTo(position)
                .setOnComplete(() =>
                {
                    if (WindowOpened != null)
                    {
                        WindowOpened();
                    }
                });
        }

        protected void Close(Position position = Position.Left)
        {
            MoveTo(last)
                .setOnComplete(() =>
                {
                    if (WindowClosed != null)
                    {
                        WindowClosed();
                    }
                });
        }

        protected LTDescr MoveTo(Position position)
        {
            return LeanTween.moveLocal(gameObject, GetPosition(position), speed).setEase(ease);
        }

        protected void SlideTo(Position position)
        {
            last = position;
            Open(position);
        }

        protected void SetPosition(Position position)
        {
            last = position;
            gameObject.transform.localPosition = GetPosition(position);
        }
        
        protected Vector2 GetPosition(Position position)
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
