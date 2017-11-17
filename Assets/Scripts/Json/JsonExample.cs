using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonExample : MonoBehaviour
{
    string data;

    // Use this for initialization
    void Start()
    {
        TextAsset asset = (TextAsset)Resources.Load("example");
        data = asset.text;

        EmployeeData employee = JsonConvert.DeserializeObject<EmployeeData>(data);
        Debug.Log(employee.dpn_profile.FirstName + "," + employee.dpn_profile.LastName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
