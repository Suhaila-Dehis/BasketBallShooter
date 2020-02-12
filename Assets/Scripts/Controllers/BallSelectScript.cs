using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class BallSelectScript : MonoBehaviour
{
    List<Button> buttons = new List<Button>();
    private void Awake()
    {
        GetButtonsAndAddListListnetrs();
    }

    void GetButtonsAndAddListListnetrs()
    {
        GameObject[] btns = GameObject.FindGameObjectsWithTag("MenuBall");

        for (int i = 0; i < btns.Length; i++)
        {
            buttons.Add(btns[i].GetComponent<Button>());
            buttons[i].onClick.AddListener(() => SelectABall());
        }

    }
    public void SelectABall()
    {
        int index = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        if (GameManager.instance != null)
        {
            GameManager.instance.SetBallIndex(index);
        }
    }

}