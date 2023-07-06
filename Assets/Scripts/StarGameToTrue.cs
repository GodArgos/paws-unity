using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGameToTrue : MonoBehaviour
{
    [SerializeField] private MenuEventSystem menuEventSystem;
    
    public void toStart()
    {
        menuEventSystem.startGame = true;
    }
}
