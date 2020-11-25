using Blis.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using BIFog;
using Blis.Client.UI;
using Blis.Common;
using Blis.Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Blis.Client.Cheat
{
    public class CheatCanvas : CheatSingleton<CheatCanvas>
    {
        public Canvas canvas;
        public void Awake()
        {
            GameObject canvasObject = new GameObject();
            this.canvas = canvasObject.AddComponent<Canvas>();
            var scaler = canvasObject.AddComponent<CanvasScaler>(); 
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920,1080); 
            Log("Create Canvas Instance");
        }
         

        public void Update()
        {
            if(Input.GetKeyUp(KeyCode.Home))
            {
                Log("Create Texts");
                foreach(var player in CheatMain.instance.players)
                {
                    if(CheatMain.instance.IsMine(player) == false)
                    {
                        PositionSyncableUI.PlayerInfo.Create(player);
                    }
                }
            }
        }
    }
}
