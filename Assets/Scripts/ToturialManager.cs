using System.Collections;
using System.Collections.Generic;
using Conversa.Demo.Scripts;
using UnityEngine;

public class ToturialManager : Singleton<ToturialManager>
{
    // Start is called before the first frame update
    public GameObject plugOut;
    public GameObject plugTarget;
    public GameObject dragDeepRepairPlayer;

    public GameObject clickToFix;
    public GameObject pressScrew;
    private DragDrop2D dragDrop2D;
    private MazeGenerator maze;
    private Screw[] screw;

    void Start()
    {
        
        
    }
     void HandlePlugClicked()
    {
        plugOut.SetActive(false);
        ConversationController.Instance.ContinueConversation();
    }
     void HandlePlugTarget()
    {
        plugTarget.SetActive(false);
        ConversationController.Instance.ContinueConversation();
    }
      void HandlePlayerClicked()
    {
        dragDeepRepairPlayer.SetActive(false);
        ConversationController.Instance.ContinueConversation();
    }
       void HandleFixClicked()
    {
        clickToFix.SetActive(false);
        ConversationController.Instance.ContinueConversation();
    }
        void HandleScrewPressed()
    {
        pressScrew.SetActive(false);
        ConversationController.Instance.ContinueConversation();
    }
    public void ActivePlugOut()
    {
        
        dragDrop2D = GameObject.FindObjectOfType<DragDrop2D>();
        if(dragDrop2D!=null)
        {
            plugOut.SetActive(true);
            dragDrop2D.OnPlugClicked.AddListener(HandlePlugClicked);
        }
        
         
    }
    public void ActivePlugTarget()
    {
        dragDrop2D = GameObject.FindObjectOfType<DragDrop2D>();
        if(dragDrop2D!=null)
        {
            plugTarget.SetActive(true);
        
            dragDrop2D.OnPlugClicked.AddListener(HandlePlugTarget);
        }
          
        
        //如果玩家连接了任何一个模块，disable，调用下面

    }
    public void ActiveDragDeepRepairPlayer()
    {
        maze = GameObject.FindObjectOfType<MazeGenerator>();
        if(maze!=null)
        {
            dragDeepRepairPlayer.SetActive(true);
         
            maze.OnClickPlayer.AddListener(HandlePlayerClicked);

        }       

    }
    public void ActiveClickToFix()
    {
        maze = GameObject.FindObjectOfType<MazeGenerator>();
        if(maze!=null)
        {
            clickToFix.SetActive(true);
         
            maze.OnClickSubMoule.AddListener(HandleFixClicked);

        }            
      //如果玩家点击了任何一个模块，那么disable，调用下

    }
    public void ActivePressScrewt()
    {
        screw=GameObject.FindObjectsOfType<Screw>();
        if(screw!=null)
        {
            pressScrew.SetActive(true);
            foreach(Screw s in screw)
            {
                //screw.
                s.OnStartRemove.AddListener(HandleScrewPressed);

            }

        }

    }

}
