using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

public class MainMenuController : MonoBehaviour {
    /********** FIELDS **********/
    private GameManager manager;
    [SerializeField]
    private List<string> levels;             // the list of available levels (configured in inspector)
    [SerializeField]
    private GameObject levelListEntry;     // a prefab for an entry in a list of levels in the menu
    private GameObject survivalMenu;       // the survival mode level list gameobject

    // Use this for initialization
    void Start () {
        manager = GameManager.instance;

        if ((survivalMenu = GameObject.Find("Survival Menu")) == null)
        {
            Debug.LogError("Warning: GameManager did not find a GameObject named \"Survival Menu\" in the scene");
        }
        else
        {
            survivalMenu.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCampaignButtonClick()
    {
        manager.StartCampaign(levels);
    }

    public void OnSurvivalButtonClick()
    {
        survivalMenu.SetActive(true);
        Transform content = survivalMenu.transform.Find("Scroll View/Viewport/Content");

        int i = 0;
        foreach (string lvl in levels)
        {
            GameObject listing = Instantiate(levelListEntry, content);
            listing.GetComponent<LevelListSelection>().name = lvl;
            RectTransform t = (RectTransform)listing.transform;
            Vector2 pos = t.localPosition;
            pos.y -= t.rect.height * i;
            listing.transform.localPosition = pos;
            ++i;
        }
    }
}
