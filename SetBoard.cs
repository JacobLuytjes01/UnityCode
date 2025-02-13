using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoard : MonoBehaviour
{
    public TileCode Prefab;
    public Vector2Int BoardSize;

    public Material BluePla;
    public Material Gree;
    public Material WallMat;
    public Material GoalMat;
    public Material RedEneMat;
    public Material RedEneAreaMat;
    public Material Ground;
    public Material EneNextSpotMat;
    public Renderer[] RendBoxes = new Renderer[4];

    TileCode OldBlock;
    TileCode NewBlock;
    int TileSet1;
    public Clicker OtherScript;

    public string TilesToPlace;
    public TileCode PlaBlock;
    public GameObject Button1;
    public Transform Cam;
    EneCode ene;
    LinkedList<TileCode> EneBlock = new LinkedList<TileCode>();

    void Start()
    {
        TilesToPlace = LevelManager.LevelString; // new line for levels

        TileCode[] NewBlockLine = new TileCode[LevelManager.LevelString.Length];
        TileCode[] OldBlockLine = new TileCode[LevelManager.LevelString.Length];

        int e = 0;
        int i = 0;
        int y = 0;

        //while (!TilesToPlace.Substring(e + i, 1).Equals("\0"))
        while (e + i + y < TilesToPlace.Length)
        {
            while (e + i + y < TilesToPlace.Length && !TilesToPlace.Substring(e + i + y, 1).Equals(" "))
            {
                NewBlock = Instantiate(Prefab, new Vector3(i, -e, 0), Prefab.transform.rotation);
                NewBlock.name = "Block: down " + e + " right " + i;

                //if ((BoardSize.y * e) + i + 2 <= BoardSize.x * BoardSize.y + 2 && TilesToPlace.Length > (BoardSize.y * e) + i + 2)
                if (TilesToPlace.Length > e + i + y)
                {
                    if (TilesToPlace.Substring(e + i + y, 1).ToUpper().Equals("O"))
                    {
                        NewBlock.GetComponent<Renderer>().material = WallMat;
                        NewBlock.tag = "CantMove";
                    }
                    else if (TilesToPlace.Substring(e + i + y, 1).ToUpper().Equals("P"))
                    {
                        PlaBlock = NewBlock;
                    }
                    else if (TilesToPlace.Substring(e + i + y, 1).ToUpper().Equals("E"))
                    {
                        y++;

                        ene = gameObject.AddComponent<EneCode>();
                        EneBlock.AddLast(NewBlock);
                        ene.RedEneMat = RedEneMat;
                        ene.RedEneAreaMat = RedEneAreaMat;
                        ene.Ground = Ground;
                        ene.EneNextSpotMat = EneNextSpotMat;
                        OtherScript.AllEnemies.AddLast(ene);

                        while (!TilesToPlace.Substring(e + i + y, 1).ToUpper().Equals("Z"))
                        {
                            ene.EnePath += TilesToPlace.Substring(e + i + y, 1).ToUpper();
                            y++;
                        }
                    }
                    else if (TilesToPlace.Substring(e + i + y, 1).ToUpper().Equals("G"))
                    {
                        NewBlock.GetComponent<Renderer>().material = GoalMat;
                        NewBlock.tag = "Finish";
                    }// end of new level maker
                }
                else
                {
                    NewBlock.gameObject.SetActive(false);
                    NewBlock.tag = "CantMove";
                }
                
                NewBlockLine[i] = NewBlock;

                if (OldBlock != null)
                {
                    OldBlock.RightBlock = NewBlock;
                    NewBlock.LeftBlock = OldBlock;
                }

                if (OldBlockLine[i] != null)
                {
                    OldBlockLine[i].DownBlock = NewBlock;
                    NewBlock.UpBlock = OldBlockLine[i];
                }

                OldBlock = NewBlock;
                i++;
            }
            OldBlock = null;

            for (int j = 0; j < i; j++)
            {
                OldBlockLine[j] = NewBlockLine[j];
            }

            y += i;
            i = 0;
            e++;
        }
        
        if (PlaBlock == null)
        {
            NewBlock.gameObject.SetActive(true);
            PlaBlock = NewBlock;
        }

        LinkedListNode<EneCode> CurrEne = OtherScript.AllEnemies.First;
        LinkedListNode<TileCode> CurrEneBlock = EneBlock.First;
        while (CurrEne != null)
        {
            CurrEne.Value.BlockToGo(CurrEneBlock.Value);
            CurrEne.Value.BlockNextMoveGlow();
            CurrEne = CurrEne.Next;
            CurrEneBlock = CurrEneBlock.Next;
        }

        PlaBlock.tag = "Player";
        PlaBlock.GetComponent<Renderer>().material = BluePla;
        OtherScript.TileBox = PlaBlock;

        if (PlaBlock.UpBlock != null)
        {
            RendBoxes[0] = PlaBlock.UpBlock.GetComponent<Renderer>();
            OtherScript.RendBoxes[0] = RendBoxes[0];
        }
        if (PlaBlock.DownBlock != null)
        {
            RendBoxes[1] = PlaBlock.DownBlock.GetComponent<Renderer>();
            OtherScript.RendBoxes[1] = RendBoxes[1];
        }
        if (PlaBlock.LeftBlock != null)
        {
            RendBoxes[2] = PlaBlock.LeftBlock.GetComponent<Renderer>();
            OtherScript.RendBoxes[2] = RendBoxes[2];
        }
        if (PlaBlock.RightBlock != null)
        {
            RendBoxes[3] = PlaBlock.RightBlock.GetComponent<Renderer>();
            OtherScript.RendBoxes[3] = RendBoxes[3];
        }

        bool EndState = false;
        bool CheckEndState = false;
        for (int j = 0; j < 4; j++)
        {
            if (RendBoxes[j] != null && !RendBoxes[j].CompareTag("CantMove"))
            {
                RendBoxes[j].gameObject.tag = "MovSqu";
                RendBoxes[j].material = Gree;
                EndState = false;
                CheckEndState = true;
            }
            else if (RendBoxes[j] != null && CheckEndState == false)
            {
                EndState = true;
            }
        }

        if (EndState == true)
        {
            Button1.SetActive(true);
        }
        Cam.position = new Vector3(PlaBlock.transform.position.x, PlaBlock.transform.position.y, -10);
    }
}