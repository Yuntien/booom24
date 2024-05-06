using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowObjController : Singleton<ShowObjController>
{
    [SerializeField] private GameObject showObjCanvas;
    [SerializeField] private GameObject objPic;

    public void ShowObj(string name)
    {
        Sprite sprite = Resources.Load<Sprite>($"Art/Obj/{name}");
        objPic.GetComponent<Image>().sprite = sprite;
        showObjCanvas.SetActive(true);
    }

    public void HideObj()
    {
        showObjCanvas.SetActive(false);
    }
}
