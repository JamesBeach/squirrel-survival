using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameManagement;

public class LevelListSelection : MonoBehaviour, ISelectHandler {
    private Text _name;
    new public string name
    {
        get
        {
            return _name.text;
        }
        set
        {
            _name.text = value;
        }
    }

    private GameManager manager;

    private void Awake()
    {
        _name = GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        manager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect(BaseEventData eventData)
    {
        manager.StartSurvivalLevel(name);
    }
}
