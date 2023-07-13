using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarGameToTrue : MonoBehaviour
{
    [SerializeField] private MenuEventSystem menuEventSystem;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Animator anim_loadingScreen;
    
    public void toStart()
    {
        menuEventSystem.loadTimer = true;
        loadingScreen.SetActive(true);
        anim_loadingScreen.SetBool("isLoading", true);
    }
}
