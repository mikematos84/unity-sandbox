using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HeavyDev
{
    public class ModalWindowView : ModalWindow, IPointerClickHandler
    {
        void Start()
        {
            WindowOpened += OnWindowOpened;
            WindowClosed += OnWindowClosed;

            SetPosition(Position.Top);
            Open();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Close();
        }

        private void OnWindowClosed()
        {
            Debug.Log("[Modal] Closed");
        }

        private void OnWindowOpened()
        {
            Debug.Log("[Modal] Open");
        }

        public void OnDisable()
        {
            WindowOpened -= OnWindowOpened;
            WindowClosed -= OnWindowClosed;
        }
    }
}
