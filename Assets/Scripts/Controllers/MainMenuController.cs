using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Animator mainAnim, ballAnim;


public void PlayGame()
    {
        SceneManager.LoadScene("Game Play");      
    }


    public void SelectBall(){
        mainAnim.Play("FadeOut");
        ballAnim.Play("FadeIn");
    }

    public void BackBall()
    {
        mainAnim.Play("FadeIn");
        ballAnim.Play("FadeOut");
    }

}