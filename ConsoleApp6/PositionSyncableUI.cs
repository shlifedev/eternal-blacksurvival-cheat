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
    public class PositionSyncableUI : MonoBehaviour
    {
        public LocalObject followTarget;
        public Vector3 worldOffset;

        public void Init(LocalObject obj)
        {
            this.followTarget = obj;
            this.transform.SetParent(CheatCanvas.instance.canvas.transform, false);
        }
        public void Update()
        {
            if (followTarget != null)
            {
                this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, followTarget.GetPosition() + worldOffset);
            }
        }
    }
}
