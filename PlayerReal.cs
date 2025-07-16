using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerReal : MonoBehaviour
{
    public Transform[] Objects;
    public Transform[] ObjectsTasks;
    public Transform[] ObjectsTools;
    public Vector3[] OgPos;
    int Placement = 1;
    public Slider Slide;
    public Transform SlideBox;
    float x;
    float ResetTimer = 1;
    int y;
    float RunningTime;
    bool SpinTime;
    Vector3 NewPos;
    bool WaterInc;

    public int CurrLevel;
    public int[] Levels;
    public RawImage[] Pics;
    public PlayerReal TheScript;
    public Transform ProgressBar;
    public GameObject Button1;
    public GameObject Button2;
    public Texture[] ReplaceImages;
    public ParticleSystem PS;
    public Transform PSPos;

    void Start()
    {
        Levels = new int[Selector.FinalNumF.Length];
        for (int i = 0; i < Pics.Length; i++)
        {
            if (Selector.FinalNumF.Length > i)
            {
                Pics[i].texture = ReplaceImages[Selector.FinalNumF[i]];

                Levels[i] = Selector.FinalNumF[i] - 1;
            }
            else
            {
                Pics[i].gameObject.SetActive(false);
            }
        }

        if (Levels[CurrLevel] == 1)
        {
            ObjectsTasks[1].gameObject.SetActive(true);
        }
        ObjectsTools[Levels[CurrLevel]].gameObject.SetActive(true);
        NewPos = OgPos[Levels[CurrLevel]];
    }

    void Update()
    {
        x += Time.deltaTime;
        Slide.value = (.5f * Mathf.Sin(2 * x)) + .5f;
        SlideBox.Rotate(new Vector3(0, 0, 100 * Time.deltaTime));

        if (SpinTime == true)
        {
            if (RunningTime <= .99f)
            {
                RunningTime += Time.deltaTime;
                ObjectsTools[Levels[CurrLevel]].Rotate(new Vector3(Time.deltaTime * 360, 0, 0), Space.World);
            }
            else
            {
                RunningTime = 0;
                ObjectsTools[Levels[CurrLevel]].rotation = Quaternion.Euler(OgPos[Levels[CurrLevel]]);
                SpinTime = false;
            }
        }
        else if (Levels[CurrLevel] == 0)
        {
            ObjectsTools[Levels[CurrLevel]].rotation = Quaternion.RotateTowards(ObjectsTools[Levels[CurrLevel]].rotation, Quaternion.Euler(NewPos), Time.deltaTime * 200);
            ProgressBar.localScale = new Vector3(0.05f + RunningTime / 2, .3f, .3f);

            if (WaterInc == true)
            {
                RunningTime += Time.deltaTime;
                NewPos = new Vector3(0, 0, 45);
            }
            else
            {
                if (RunningTime > 0.01f)
                {
                    RunningTime -= Time.deltaTime;
                }
                else
                {
                    RunningTime = 0;
                }
            }
        }
        else if (Levels[CurrLevel] == 2)
        {
            ObjectsTools[Levels[CurrLevel]].rotation = Quaternion.RotateTowards(ObjectsTools[Levels[CurrLevel]].rotation, Quaternion.Euler(NewPos), Time.deltaTime * 100);

            if (ObjectsTools[Levels[CurrLevel]].rotation.eulerAngles.z >= 89)
            {
                NewPos = OgPos[Levels[CurrLevel]];
            }
        }

        if (Input.GetButtonDown("Left"))
        {
            if (Placement != 0)
            {
                Placement--;
                transform.position = new Vector3(Objects[Placement].position.x, transform.position.y, transform.position.z);
            }
        }
        else if (Input.GetButtonDown("Right"))
        {
            if (Placement != 2)
            {
                if (Placement == 0)
                {
                    NewPos = new Vector3(0, 0, 105);
                    WaterInc = false;
                }

                Placement++;
                transform.position = new Vector3(Objects[Placement].position.x, transform.position.y, transform.position.z);
            }
        }

        if (Placement == 0 && Levels[CurrLevel] == 0)
        {
            if (Input.GetButton("Fire1"))
            {
                WaterInc = true;
                //RunningTime += Time.deltaTime;
                //NewPos = new Vector3(0, 0, 45);
            }
            else
            {
                WaterInc = false;
                /*
                if (RunningTime > 0.01f)
                {
                    RunningTime -= Time.deltaTime;
                }
                else
                {
                    RunningTime = 0;
                }
                */
            }

            if (Input.GetButtonUp("Fire1"))
            {
                NewPos = new Vector3(0, 0, 105);
            }
            
            //ProgressBar.localScale = new Vector3(0.05f + RunningTime/2, .3f, .3f);

            if (RunningTime >= 2)
            {
                WinSet();
            }
        }
        else if (Placement == 1 && Levels[CurrLevel] == 1)
        {
            if (Input.GetButtonDown("Fire1") && ResetTimer >= 1)
            {
                SpinTime = true;
                ResetTimer = 0;
                if (Slide.value >= .37f && Slide.value <= .63f)
                {
                    y += 5;
                    ProgressBar.localScale = new Vector3(ProgressBar.localScale.x + .5f, .3f, .3f);
                }
                else
                {
                    ProgressBar.localScale = new Vector3(ProgressBar.localScale.x + .1f, .3f, .3f);
                    y += 1;
                }
                if (y >= 10)
                {
                    WinSet();
                }
            }
            ResetTimer += Time.deltaTime;
        }
        else if (Placement == 2 && Levels[CurrLevel] == 2)
        {
            if (Input.GetButtonDown("Fire1") && ResetTimer >= .1f)
            {
                ResetTimer = 0;
                y++;
                NewPos = new Vector3(0, 0, 90);
                ProgressBar.localScale = new Vector3(ProgressBar.localScale.x + .2f, .3f, .3f);
            }
            if (y == 5)
            {
                WinSet();
            }
            ResetTimer += Time.deltaTime;
        }
    }

    void WinSet()
    {
        Instantiate(PS, PSPos.position, PSPos.rotation).Play();
        if (CurrLevel < Levels.Length - 1)
        {
            ObjectsTasks[0].gameObject.SetActive(false);
            ObjectsTasks[Levels[CurrLevel]].gameObject.SetActive(false);
            ObjectsTools[Levels[CurrLevel]].gameObject.SetActive(false);
            ObjectsTools[Levels[CurrLevel]].rotation = Quaternion.Euler(OgPos[Levels[CurrLevel]]);

            Pics[CurrLevel].rectTransform.anchoredPosition = new Vector2(Pics[CurrLevel].rectTransform.anchoredPosition.x, 60);
            Pics[CurrLevel].color = new Color(.8f, .8f, .8f);
            CurrLevel++;
            Pics[CurrLevel].rectTransform.anchoredPosition = new Vector2(Pics[CurrLevel].rectTransform.anchoredPosition.x, 80);
            Pics[CurrLevel].color = new Color(1, 1, 1);

            if (Levels[CurrLevel] == 1)
            {
                ObjectsTasks[Levels[CurrLevel]].gameObject.SetActive(true);
            }
            ObjectsTools[Levels[CurrLevel]].gameObject.SetActive(true);
            RunningTime = 0;
            y = 0;
            NewPos = OgPos[Levels[CurrLevel]];
            SpinTime = false;
            ProgressBar.localScale = new Vector3(.05f, .3f, .3f);
        }
        else
        {
            Pics[CurrLevel].rectTransform.anchoredPosition = new Vector2(Pics[CurrLevel].rectTransform.anchoredPosition.x, 60);
            Pics[CurrLevel].color = new Color(.8f, .8f, .8f);
            ObjectsTasks[Levels[CurrLevel]].gameObject.SetActive(false);
            ObjectsTools[Levels[CurrLevel]].gameObject.SetActive(false);
            ProgressBar.localScale = new Vector3(.05f, .3f, .3f);
            Button1.SetActive(true);
            Button2.SetActive(true);

            TheScript.enabled = false;
        }
        
    }
}