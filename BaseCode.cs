using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class BaseCode : MonoBehaviour
{
    float PowerNum;
    public TMP_Text PowerText;
    int PowerMult = 1;

    int RecruitBasePrice = 10;
    public TMP_Text RecruitText;
    public Button RecruitButton;

    public Button SacrificeButton;
    public TMP_Text SacrificeButtonText;

    public TMP_Text RecruitText10X;
    public Button RecruitButton10X;

    public Button SacrificeButton10X;
    public TMP_Text SacrificeButtonText10X;

    int CultistsNum;
    public TMP_Text CultistsText;

    float PoliceNum;
    float PolicePercentGain = 10;
    int PoliceDisappearMult = 1;
    public TMP_Text PoliceText;
    public Button PoliceButton;
    public TMP_Text PoliceButtonText;
    public int PolicePowerNum = 2;

    int BoonsNum = 0;
    int BoonsGain = 1;
    public TMP_Text BoonsText;

    public Button[] SpecialWayButton;
    public TMP_Text[] SpecialWayText;
    public Button[] UpgradesButton;
    public String[] UpgradesText;
    public TMP_Text FlavorText;
    int TheChoice;

    int[] UpgradeCostNums = { 20, 50, 150 };
    float[] UpgradePowerNums = { 1, 1, 1 };

    int AutoClickerNum = 0;
    int AutoClickerPrice = 10;
    float AutoClickerTimer;
    public Button AutoClickerButton;
    public TMP_Text AutoClickerButtonText;
    public TMP_Text AutoClickerText2;
    int W = 0;

    private void FixedUpdate()
    {
        if (AutoClickerTimer >= .5f)
        {
            PowerNum += AutoClickerNum * (1f + CultistsNum/10f);
            AutoClickerTimer -= .5f;
        }
        AutoClickerTimer += Time.fixedDeltaTime;

        UpdateText();
    }

    void UpdateText() 
    {
        PowerText.text = "Power: " + Math.Round(PowerNum, 1);
        if (PowerNum >= RecruitBasePrice * (1 + CultistsNum)) // Mathf.Pow(CultistsNum, 2)
        {
            RecruitButton.interactable = true;
        }
        else
        {
            RecruitButton.interactable = false;
        }
        RecruitText.text = "Recruit Cultist Price: " + (RecruitBasePrice * (1 + CultistsNum)) + " Power";
        CultistsText.text = "Cultists: " + CultistsNum;

        if (PoliceNum >= 0.2f)
        {
            PoliceNum -= Time.fixedDeltaTime * PoliceDisappearMult;
        }
        else
        {
            PoliceNum = 0;
        }
        if (CultistsNum >= 1 && PoliceNum < 100 - PolicePercentGain)
        {
            SacrificeButton.interactable = true;
        }
        else
        {
            SacrificeButton.interactable = false;
        }
        PoliceText.text = "Police Notice: " + Math.Round(PoliceNum, 2) + "%";

        BoonsText.text = "Boons: " + BoonsNum;

        if (PowerNum >= Mathf.Pow(10, PolicePowerNum))
        {
            PoliceButton.interactable = true;
        }
        else
        {
            PoliceButton.interactable = false;
        }

        if (BoonsNum >= 10)
        {
            for (int i = 0; i < SpecialWayButton.Length; i++)
            {
                SpecialWayButton[i].interactable = true;
            }
        }
        else
        {
            for (int i = 0; i < SpecialWayButton.Length; i++)
            {
                SpecialWayButton[i].interactable = false;
            }
        }

        for (int i = 0; i < UpgradesButton.Length; i++)
        {
            if (BoonsNum >= Mathf.Pow(UpgradeCostNums[i], UpgradePowerNums[i]))
            {
                UpgradesButton[i].interactable = true;
            }
            else
            {
                UpgradesButton[i].interactable = false;
            }
        }

        if (PowerNum >= AutoClickerPrice)
        {
            AutoClickerButton.interactable = true;
        }
        else
        {
            AutoClickerButton.interactable = false;
        }
        AutoClickerButtonText.text = "AutoClicker\nPrice: " + AutoClickerPrice + " Power";
        AutoClickerText2.text = "Auto: " + Math.Round((AutoClickerNum * (1f + CultistsNum / 10f)), 2);


        
        if (PowerNum >= RecruitNum10x())
        {
            RecruitButton10X.interactable = true;
        }
        else
        {
            RecruitButton10X.interactable = false;
        }
        W = 0;
        RecruitText10X.text = "Recruit 10 Cultist Price: " + RecruitNum10x() + " Power";

        if (CultistsNum >= 10 && PoliceNum < 100 - PolicePercentGain * 10)
        {
            SacrificeButton10X.interactable = true;
        }
        else
        {
            SacrificeButton10X.interactable = false;
        }

        W = 0;
    }

    public void PowerUp() 
    {
        PowerNum += (1 + CultistsNum) * PowerMult;
        UpdateText();
    }
    public void Recruit()
    {
        PowerNum -= RecruitBasePrice * (1 + CultistsNum);
        CultistsNum++;
        UpdateText();
    }
    public void Sacrifice()
    {
        CultistsNum--;
        BoonsNum += BoonsGain;
        PoliceNum += PolicePercentGain;
        UpdateText();
    }

    public void Recruit10X()
    {
        PowerNum -= RecruitNum10x();
        CultistsNum += 10;
        W = 0;
        UpdateText();
    }
    int RecruitNum10x()
    {
        for (int i = 0; i < 10; i++)
        {
            W += (RecruitBasePrice * ((1 + i + CultistsNum)));
        }
        return W;
    }
    public void Sacrifice10X()
    {
        CultistsNum -= 10;
        BoonsNum += BoonsGain * 10;
        PoliceNum += PolicePercentGain * 10;
        UpdateText();
    }

    public void PoliceButtonCode()
    {
        PowerNum -= Mathf.Pow(10, PolicePowerNum);
        PolicePowerNum++;
        PoliceDisappearMult *= 2;
        UpdateText();
        PoliceButtonText.text = "Police Care Less Price: " + Mathf.Pow(10, PolicePowerNum) + " Power";
    }

    public void CultChoice(int Choice)
    {
        TheChoice = Choice;
        FlavorText.gameObject.SetActive(false);
        for (int i = 0; i < SpecialWayButton.Length; i++)
        {
            SpecialWayButton[i].gameObject.SetActive(false);
            UpgradesButton[i].gameObject.SetActive(true);
        }
        if (TheChoice == 1)
        {
            TheChoice = 0;
            //SpecialWayText[0].text = UpgradesText[0];
            SpecialWayText[0].text = UpgradesText[0] + UpgradeCostNums[0] + " Boons";
            SpecialWayText[1].text = UpgradesText[1] + UpgradeCostNums[1] + " Boons";
            SpecialWayText[2].text = UpgradesText[2] + UpgradeCostNums[2] + " Boons";
        }
        else if (TheChoice == 2)
        {
            TheChoice = 3;
            SpecialWayText[0].text = UpgradesText[3] + UpgradeCostNums[0] + " Boons";
            SpecialWayText[1].text = UpgradesText[4] + UpgradeCostNums[1] + " Boons";
            SpecialWayText[2].text = UpgradesText[5] + UpgradeCostNums[2] + " Boons";
        }
        else if (TheChoice == 3)
        {
            TheChoice = 6;
            SpecialWayText[0].text = UpgradesText[6] + UpgradeCostNums[0] + " Boons";
            SpecialWayText[1].text = UpgradesText[7] + UpgradeCostNums[1] + " Boons";
            SpecialWayText[2].text = UpgradesText[8] + UpgradeCostNums[2] + " Boons";
        }
        UpdateText();
    }

    public void Upgrade1()
    {
        BoonsNum -= (int)Mathf.Pow(UpgradeCostNums[0], UpgradePowerNums[0]);
        UpgradePowerNums[0] += .5f;

        PolicePercentGain /= 2;
        SacrificeButtonText.text = "Sacrifice Cultist\nGain: " + BoonsGain + " Boon\nGain: " + PolicePercentGain + "% Notice";
        SacrificeButtonText10X.text = "Sacrifice 10 Cultist\nGain: " + (BoonsGain*10) + " Boon\nGain: " + (PolicePercentGain*10) + "% Notice";
        SpecialWayText[0].text = UpgradesText[TheChoice] + (int)Mathf.Pow(UpgradeCostNums[0], UpgradePowerNums[0]) + " Boons";
        UpdateText();
    }
    public void Upgrade2()
    {
        //BoonsNum -= UpgradeCostNums[1];
        BoonsNum -= (int)Mathf.Pow(UpgradeCostNums[1], UpgradePowerNums[1]);
        UpgradePowerNums[1] += .5f;

        BoonsGain *= 2;
        SacrificeButtonText.text = "Sacrifice Cultist\nGain: " + BoonsGain + " Boon\nGain: " + PolicePercentGain + "% Notice";
        SacrificeButtonText10X.text = "Sacrifice 10 Cultist\nGain: " + (BoonsGain * 10) + " Boon\nGain: " + (PolicePercentGain * 10) + "% Notice";
        SpecialWayText[1].text = UpgradesText[TheChoice+1] + (int)Mathf.Pow(UpgradeCostNums[1], UpgradePowerNums[1]) + " Boons";
        UpdateText();
    }
    public void Upgrade3()
    {
        BoonsNum -= (int)Mathf.Pow(UpgradeCostNums[2], UpgradePowerNums[2]);
        UpgradePowerNums[2] += .5f;

        PowerMult *= 2;
        SpecialWayText[2].text = UpgradesText[TheChoice + 2] + (int)Mathf.Pow(UpgradeCostNums[2], UpgradePowerNums[2]) + " Boons";
        UpdateText();
    }

    public void AutoClickerUp()
    {
        PowerNum -= AutoClickerPrice;
        AutoClickerNum++;
        AutoClickerPrice += AutoClickerNum;
        UpdateText();
    }
}