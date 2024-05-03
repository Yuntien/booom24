using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour {

    public GameObject wallL;
    public GameObject wallR;
    public GameObject wallU;
    public GameObject wallD;

    public SpriteRenderer moveSprite;

    public bool isTarget = false;

    //public Submodule submodule;



    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
    public void MarkAsTarget()
    {
        isTarget = true;
        ChangeColor(Color.red);
    }
}