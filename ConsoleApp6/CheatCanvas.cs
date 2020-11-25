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
        public void InitDelay()
        {
            GameObject canvasObject = new GameObject();
            this.canvas = canvasObject.AddComponent<Canvas>();
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            Log("Create Canvas Instance");
            InitPlayerSkillCooltime();
        }
        public void Awake()
        {
            Invoke("InitDelay", 2.0f);

        }


        public void InitPlayerSkillCooltime()
        {
            Log("Init Skill Cooltime Viewer");
            var data = new GameObject();
            var x = data.AddComponent<PositionSyncableUI.PlayerInfo>();
            x.Init(CheatMain.instance.mine);
        }
    }
}
