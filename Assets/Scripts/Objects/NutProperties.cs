using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

/// <summary>
/// This class is used to identify a nut as a particular type (in the GameManagement ItemSchedules NutType enumeration) and track
/// other properties, should they become relevant. This class's functionality replaces the previously-used Unity tags.
/// </summary>
public class NutProperties : MonoBehaviour {
    [SerializeField]
    private NutType _type;
    
    public NutType type
    {
        get { return _type; }
    }

    public Vector2 worldPosition
    {
        get { return this.transform.position; }
    }
}
