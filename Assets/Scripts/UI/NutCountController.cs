using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NutCountController : MonoBehaviour {
    public int digits;      // quantities that can't be displayed with this many digits will be truncated
    public Image anchor;    // the first digit; addition digits will be made to the right of this one
    public float kerning;     // distance of digits from each other

    private Image[] dig_img;
    private Animator[] dig_anim;
    private float digDist;

    private Image slash;
    private Image[] goal_img;
    private Animator[] goal_anim;

	// Use this for initialization
	void Start () {
        dig_img = new Image[digits];
        dig_anim = new Animator[digits];

        digDist = (anchor.rectTransform.rect.width / 2) + kerning;

        dig_img[0] = anchor;

        Vector3 pos;

        for (int i = 1; i < dig_img.Length; ++i)
        {
            Image img = Instantiate<Image>(anchor);
            img.transform.SetParent(this.transform);
            pos = anchor.rectTransform.anchoredPosition;
            pos.x += digDist * i;
            img.rectTransform.anchoredPosition = pos;
            img.rectTransform.localScale = anchor.rectTransform.localScale;
            dig_img[i] = img;
        }

        for (int i = 0; i < dig_img.Length; ++i)
        {
            Animator anim = dig_img[i].GetComponent<Animator>();
            anim.SetInteger("Integer", 0);
            anim.SetBool("Visible", true);
            dig_anim[i] = anim;
        }

        // set up minimum nut digits

        LevelController lvlctrl = FindObjectOfType<LevelController>();
        if (lvlctrl == null)
        {
            Debug.Log("NutCountController did not find a CampaignLevelController!");
        }
        else
        {
            int goal = lvlctrl.MinimumNuts;
            if (goal > 0)
            {
                int nDigits = 0;
                {
                    int i = 0;
                    do
                    {
                        ++nDigits;
                        ++i;
                    } while (goal / Mathf.Pow(10, i) >= 1);
                }

                { // create slash
                    slash = Instantiate<Image>(anchor);
                    slash.transform.SetParent(this.transform);
                    pos = anchor.rectTransform.anchoredPosition;
                    pos.x += digDist * dig_img.Length;
                    slash.rectTransform.anchoredPosition = pos;
                    slash.rectTransform.localScale = anchor.rectTransform.localScale;
                    Animator anim = slash.GetComponent<Animator>();
                    anim.SetInteger("Integer", (int)rotonum.ROTO_SLASH);
                    anim.SetBool("Visible", true);
                }

                // create goal numbers
                goal_img = new Image[nDigits];
                goal_anim = new Animator[nDigits];
                int value = goal;
                for (int i = 0; i < nDigits; ++i)
                {
                    Image img = Instantiate<Image>(anchor);
                    img.transform.SetParent(this.transform);
                    pos = anchor.rectTransform.anchoredPosition;
                    pos.x += digDist * (dig_img.Length + 1 + i);
                    img.rectTransform.anchoredPosition = pos;
                    img.rectTransform.localScale = anchor.rectTransform.localScale;
                    goal_img[i] = img;

                    Animator anim = img.GetComponent<Animator>();

                    int divisor = (int)Mathf.Pow(10, nDigits - 1 - i);
                    int dig = value / divisor;
                    anim.SetInteger("Integer", dig);
                    anim.SetBool("Visible", true);
                    goal_anim[i] = anim;

                    value -= dig * divisor;
                }
            }
        }
	}

    public void updateCount(int count)
    {
        for (int i = 0; i < digits; ++i)
        {
            int divisor = (int)Mathf.Pow(10, digits - 1 - i);
            int dig = count / divisor;
            dig_anim[i].SetInteger("Integer", dig);
            count -= dig * divisor;
        }
    }

    private enum rotonum
    {
        ROTO_0      = 0,
        ROTO_1      = 1,
        ROTO_2      = 2,
        ROTO_3      = 3,
        ROTO_4      = 4,
        ROTO_5      = 5,
        ROTO_6      = 6,
        ROTO_7      = 7,
        ROTO_8      = 8,
        ROTO_9      = 9,
        ROTO_COLON  = 10,
        ROTO_SLASH  = 11
    }
}
