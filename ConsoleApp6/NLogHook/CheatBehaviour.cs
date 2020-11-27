using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Diagnostics;

namespace Blis.Client.Cheat.NLogHook
{
    public class CheatBehaviour : Blis.Client.Cheat.CheatSingleton<CheatBehaviour>
    {   
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            GameDataDumper.instance.CreateInstance(true);
        } 
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Insert))
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    CheatMain.instance.CreateInstance();
                }
            }
            if (Input.GetKeyUp(KeyCode.Home))
            {
                gui = true; 
            }
        }
    }
}