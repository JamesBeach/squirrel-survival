using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour {
	public float time;
    public bool paused = true;
	public Image avatar;

    public Image timeDispAnchor;
    public float timeDispKerning;

    private Animator minuteTens,
                     minuteOnes,
                     secondTens,
                     secondOnes;

	private LevelController lvlctrl;

	public float avatarStartPos;
	public float avatarEndPos;
    private float avStrPos;
    private float avEndPos;
    private float avatarPosDiff;

    private RectTransform cvtrans;

	public float timeLeft;
	private Vector2 avaPos;

	private float berryFactor;

    private bool spawning;
    private float nextSpawn;

	private void Start() {
		lvlctrl = FindObjectOfType<LevelController>();
		cvtrans = gameObject.GetComponent<RectTransform> ();

		berryFactor = 1f;

        avaPos = avatar.rectTransform.anchoredPosition;
        avStrPos = avatarStartPos * cvtrans.rect.width;
        avEndPos = avatarEndPos * cvtrans.rect.width;
//        Debug.Log(avaPos);
        avatarPosDiff = avStrPos - avEndPos;

        time = lvlctrl.LevelTime;
        timeLeft = time;

        updateAvatar();

        initTimeDisp();
        updateTimeDisp((int)timeLeft);

        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Tree Farm level")
        {
            spawning = true;
            nextSpawn = time;
        } else
        {
            spawning = false;
        }
	}

	void FixedUpdate() {
        //Debug.Log(timeLeft);
        if(timeLeft > time)
        {
            timeLeft = time;
        }
		if (timeLeft <= 0) {
			timeLeft = 0;
			lvlctrl.SendMessage ("GameOver", false);
		} else {
			if (!paused) {
				timeLeft = timeLeft - Time.deltaTime * berryFactor;
				updateAvatar ();
				updateTimeDisp ((int)timeLeft);
			}
		}
        //Debug.Log(timeLeft);

        if(spawning && timeLeft < nextSpawn)
        {
            Spawn();
            nextSpawn -= 0.5f;
        }
    }

    private void updateAvatar()
    {
        avaPos.x = avEndPos + ((timeLeft / time) * avatarPosDiff) + cvtrans.rect.x;
        avatar.rectTransform.anchoredPosition = avaPos;
    }

    private void initTimeDisp()
    {
        // set anchor image to colon
        {
            Animator anch = timeDispAnchor.GetComponent<Animator>();
            anch.SetBool("Visible", true);
            anch.SetInteger("Integer", (int) rotonum.ROTO_COLON);
        }

        Image x;
        Transform parent = this.GetComponentInParent<Transform>();
        Vector2 pos;

        // Instantiate minute display, tens place
        x = Instantiate<Image>(timeDispAnchor);
        x.transform.SetParent(parent);
        pos = timeDispAnchor.transform.localPosition;
        // pos.x = pos.x - width;
        pos.x -= 2 * timeDispKerning;
        x.transform.localPosition = pos;
        x.transform.localScale = timeDispAnchor.transform.localScale;

        minuteTens = x.GetComponent<Animator>();
        minuteTens.SetBool("Visible", true);

        // Instantiate minute display, ones place
        x = Instantiate<Image>(timeDispAnchor);
        x.transform.SetParent(parent);
        pos = timeDispAnchor.transform.localPosition;
        // pos.x = pos.x - width;
        pos.x -= timeDispKerning;
        x.transform.localPosition = pos;
        x.transform.localScale = timeDispAnchor.transform.localScale;

        minuteOnes = x.GetComponent<Animator>();
        minuteOnes.SetBool("Visible", true);

        // Instantiate second display, tens place
        x = Instantiate<Image>(timeDispAnchor);
        x.transform.SetParent(parent);
        pos = timeDispAnchor.transform.localPosition;
        // pos.x = pos.x - width;
        pos.x += timeDispKerning;
        x.transform.localPosition = pos;
        x.transform.localScale = timeDispAnchor.transform.localScale;

        secondTens = x.GetComponent<Animator>();
        secondTens.SetBool("Visible", true);

        // Instantiate second display, ones place
        x = Instantiate<Image>(timeDispAnchor);
        x.transform.SetParent(parent);
        pos = timeDispAnchor.transform.localPosition;
        // pos.x = pos.x - width;
        pos.x += 2 * timeDispKerning;
        x.transform.localPosition = pos;
        x.transform.localScale = timeDispAnchor.transform.localScale;

        secondOnes = x.GetComponent<Animator>();
        secondOnes.SetBool("Visible", true);
    }

    private void updateTimeDisp(int seconds)
    {
        int mins = seconds / 60;
        int secs = seconds % 60;
        minuteTens.SetInteger("Integer", mins / 10);
        minuteOnes.SetInteger("Integer", mins % 10);
        secondTens.SetInteger("Integer", secs / 10);
        secondOnes.SetInteger("Integer", secs % 10);
    }

	public void slowDown(){
		berryFactor = .333333f;
	}

	public void speedUp(){
		berryFactor = 1;
	}

    private enum rotonum
    {
        ROTO_COLON = 10,
        ROTO_SLASH = 11
    }

    public void addTime()
    {
        timeLeft += 0.5f;
    }

    private void Spawn()
    {
        int border = Random.Range(1, 3);
        EvilSquirrels squirrel = FindObjectOfType<EvilSquirrels>();
        if (border == 1)
        {
            Instantiate(squirrel, new Vector3(-22, Random.Range(-9, 18), 0), Quaternion.identity);
        }
        else if(border == 2)
        {
            Instantiate(squirrel, new Vector3(Random.Range(-22, 21), 18, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(squirrel, new Vector3(21, Random.Range(-9, 18), 0), Quaternion.identity);
        }
    }
}
