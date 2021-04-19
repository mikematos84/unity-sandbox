using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSceneController : SceneController {

    protected override void LoadSceneData()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        Debug.Log(this);
    }
}
