using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EneCode : MonoBehaviour
{
    public TileCode TileBox;
    public TileCode[] ObjAroundTheObj = new TileCode[20];
    public Material RedEneMat;
    public Material RedEneAreaMat;
    public Material Ground;
    public Material EneNextSpotMat;

    public string EnePath;
    int NumOnPath = 0;
    int d = 0;

    public void BlockToGo(TileCode BlockMove) // is access by SetBoard
    {
        if (BlockMove != null && !BlockMove.CompareTag("CantMove"))
        {
            if (TileBox != null)
            {
                TileBox.tag = "Untagged";
                TileBox.GetComponent<Renderer>().material = Ground; // was previously in if(TileBox != null)
            }
            if (ObjAroundTheObj[1] != null)
            {
                for (int i = 0; i < ObjAroundTheObj.Length; i++)
                {
                    if (ObjAroundTheObj[i] != null && !ObjAroundTheObj[i].CompareTag("MovSqu") && !ObjAroundTheObj[i].CompareTag("CantMove") && !ObjAroundTheObj[i].CompareTag("Enemy") && !ObjAroundTheObj[i].CompareTag("Finish"))
                    {
                        ObjAroundTheObj[i].GetComponent<Renderer>().material = Ground;
                        ObjAroundTheObj[i].tag = "Untagged";
                        ObjAroundTheObj[i] = null;
                    }

                }
            }
            TileBox = BlockMove;
            BlockMove.tag = "Enemy";
            BlockMove.GetComponent<Renderer>().material = RedEneMat;

            TileFinder(BlockMove);

            for (int i = 0; i < d; i++)
            {
                if (i <= 3)
                {
                    TileFinder(ObjAroundTheObj[i]);
                }

                if (ObjAroundTheObj[i] != null && !ObjAroundTheObj[i].CompareTag("CantMove") && !ObjAroundTheObj[i].CompareTag("Enemy"))
                {
                    /*
                    if (ObjAroundTheObj[i].CompareTag("Player"))
                    {
                        Debug.Log("d");
                    }*/
                    // test for player detection

                    ObjAroundTheObj[i].GetComponent<Renderer>().material = RedEneAreaMat;
                    ObjAroundTheObj[i].tag = "eneArea";
                }
            }
            d = 0;
        }
    }

    public void TileFinder(TileCode AroundBlocks)
    {
        if (AroundBlocks != null)
        {
            ObjAroundTheObj[d] = AroundBlocks.DownBlock;
            ObjAroundTheObj[d + 1] = AroundBlocks.LeftBlock;
            ObjAroundTheObj[d + 2] = AroundBlocks.RightBlock;
            ObjAroundTheObj[d + 3] = AroundBlocks.UpBlock;
            d += 4;
        }
    }

    public void EneMove() // used by Clicker
    {
        if (EnePath.Substring(NumOnPath, 1).Equals("D"))
        {
            BlockToGo(TileBox.DownBlock);
            NumOnPath++;
        }
        else if (EnePath.Substring(NumOnPath, 1).Equals("L"))
        {
            BlockToGo(TileBox.LeftBlock);
            NumOnPath++;
        }
        else if (EnePath.Substring(NumOnPath, 1).Equals("R"))
        {
            BlockToGo(TileBox.RightBlock);
            NumOnPath++;
        }
        else if (EnePath.Substring(NumOnPath, 1).Equals("U"))
        {
            BlockToGo(TileBox.UpBlock);
            NumOnPath++;
        }

        if (NumOnPath >= EnePath.Length)
        {
            NumOnPath = 0;
        }
        BlockNextMoveGlow();
    }

    public void BlockNextMoveGlow()
    {
        TileCode BlockToGlow = null;
        if (EnePath.Substring(NumOnPath, 1).Equals("D"))
        {
            BlockToGlow = TileBox.DownBlock;
        }
        else if (EnePath.Substring(NumOnPath, 1).Equals("L"))
        {
            BlockToGlow = TileBox.LeftBlock;
        }
        else if (EnePath.Substring(NumOnPath, 1).Equals("R"))
        {
            BlockToGlow = TileBox.RightBlock;
        }
        else if (EnePath.Substring(NumOnPath, 1).Equals("U"))
        {
            BlockToGlow = TileBox.UpBlock;
        }

        if (BlockToGlow != null && !BlockToGlow.CompareTag("CantMove"))
        {
            BlockToGlow.GetComponent<Renderer>().material = EneNextSpotMat;
        }
    }
}