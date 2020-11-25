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
        public float tick = 0.0f;
        public float maxTick = 1.0f;
        public void Init(LocalObject obj)
        {
            this.followTarget = obj;
            this.transform.SetParent(CheatCanvas.instance.canvas.transform, false);
        }

        public void TickUpdator(System.Action onTick)
        {
            tick += Time.deltaTime;
            if(tick >= maxTick)
            {
                tick = 0;
                onTick?.Invoke();
            }
        }
        public void Update()
        {
            if (followTarget != null)
            {
                this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, followTarget.GetPosition() + worldOffset);
            }
           
        }

        public Text CreateDefaultText(int fontSize, string content = null)
        {
            GameObject textObject = new GameObject();
            var text = textObject.AddComponent<Text>();
            text.font = ResourceManager.inst.GetFont(Ln.GetCurrentLanguage().GetFontName());
            text.fontSize = 14;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            return text;
        }

        public class PlayerInfo : PositionSyncableUI
        {
            public LocalPlayerCharacter player ;
            public Text infoText;
            public static void Create(LocalObject target)
            {
                

                GameObject textObject = new GameObject();
                var playerInfo = textObject.AddComponent<PositionSyncableUI.PlayerInfo>();
                playerInfo.Init(target);
                playerInfo.player = target as LocalPlayerCharacter;
                playerInfo.infoText = playerInfo.CreateDefaultText(12); 
            }

            public void CooltimeUpdate()
            { 
                var QCoolTime = (this.player.GetSkillCooldown(SkillSlotIndex.Active1).HasValue == false) ?
                    0 
                    :
                    this.player.GetSkillCooldown(SkillSlotIndex.Active1).Value;
                var WCoolTime = (this.player.GetSkillCooldown(SkillSlotIndex.Active2).HasValue == false) ?
                    0 
                    :
                    this.player.GetSkillCooldown(SkillSlotIndex.Active2).Value;
                var ECoolTime = (this.player.GetSkillCooldown(SkillSlotIndex.Active3).HasValue == false) ?
                    0 
                    :
                    this.player.GetSkillCooldown(SkillSlotIndex.Active3).Value;
                var RCoolTime = (this.player.GetSkillCooldown(SkillSlotIndex.Active4).HasValue == false) ?
                    0 
                    :
                    this.player.GetSkillCooldown(SkillSlotIndex.Active4).Value;

                infoText.text = $"{QCoolTime.ToString("0.0")} {WCoolTime.ToString("0.0")} {ECoolTime.ToString("0.0")} {RCoolTime.ToString("0.0")}";
            }
            public void Update()
            {
                TickUpdator(CooltimeUpdate);
            }
        }
    }
}
