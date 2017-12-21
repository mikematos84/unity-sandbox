using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HeavyDev
{
    public class ModalWindowView : ModalWindowController
    {
        ModalWindowModel model;
        
        void SetData(object obj)
        {
            this.model = (ModalWindowModel)obj;
        }

        void Start()
        {
            OnWindowOpen += HandleWindowOpen;
            OnWindowClose += HandleWindowClose;
            OnOpenButtonClick += HandleOpenButtonClick;
            OnCloseButtonClick += HandleCloseButtonClick;

            SetPosition(Position.Bottom);
            Open();
        }

        private void HandleOpenButtonClick()
        {
            Debug.Log("Open Button Clicked");
            Open();
        }

        private void HandleCloseButtonClick()
        {
            Debug.Log("Close Button Clicked");
            Close();
        }
        
        public void OnDisable()
        {
            OnWindowOpen -= HandleWindowOpen;
            OnWindowClose -= HandleWindowClose;
            OnOpenButtonClick -= HandleOpenButtonClick;
            OnCloseButtonClick -= HandleCloseButtonClick;
        }
        
        private void HandleWindowOpen()
        {
            Debug.Log("[Modal] Open");
        }

        private void HandleWindowClose()
        {
            Debug.Log("[Modal] Close");
        }

        
    }
}
