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
        public virtual void Init(LocalObject obj)
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

        public void UpdatePos()
        {
            if (followTarget != null)
            {
                this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, followTarget.GetPosition() + worldOffset);
            }
            else
            {
                CheatCanvas.instance.Log("FollowTarget Cannot null..");
            }

        }
        public virtual void Update()
        {
            UpdatePos();
        }

        public Text CreateDefaultText(Transform parent, int fontSize = 14, string content = null)
        {
            GameObject textObject = new GameObject();
            var text = textObject.AddComponent<Text>();
            text.font = ResourceManager.inst.GetFont(Ln.GetCurrentLanguage().GetFontName());
            text.fontSize = fontSize;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = content; 
            textObject.transform.SetParent(parent,false);
            textObject.transform.localPosition = Vector3.zero;
            return text;
        }

        public class PlayerInfo : PositionSyncableUI
        {
            public LocalPlayerCharacter player ;
            public Text infoText;
            public override void Init(LocalObject target)
            { 
                try
                {
                    if(target == null)
                    {
                        CheatMain.instance.Log("target cannot null!");
                        return;
                    } 
                    base.Init((LocalObject)target);
                    player = target as LocalPlayerCharacter;
                    if (player == null)
                    {
                        CheatMain.instance.Log("parse failed!");
                        return;
                    }
                    infoText = CreateDefaultText(this.transform, 14);
                }
                catch (Exception e)
                {
                    CheatCanvas.instance.Log(e.Message); 
                }
            }

            public void CooltimeUpdate()
            { 
                if(player == null)
                {
                    CheatMain.instance.Log("Player cannot null..");
                    return;
                }
                try
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
                     
                    string infoStr = "";
                    if (QCoolTime <= 0)
                        infoStr += "<color=green>Q</color> ";
                    else if(this.player.GetSkillLevel(SkillSlotIndex.Active1) == 0 || QCoolTime > 0)
                        infoStr += "<color=red>Q</color> ";

                    if (WCoolTime <= 0)
                        infoStr += "<color=green>W</color> ";
                    else if (this.player.GetSkillLevel(SkillSlotIndex.Active2) == 0 || QCoolTime > 0)
                        infoStr += "<color=red>W</color> ";

                    if (ECoolTime <= 0)
                        infoStr += "<color=green>E</color> ";
                    else if (this.player.GetSkillLevel(SkillSlotIndex.Active3) == 0 || QCoolTime > 0)
                        infoStr += "<color=red>E</color> ";

                    if (RCoolTime <= 0)
                        infoStr += "<color=green>R</color> ";
                    else if (this.player.GetSkillLevel(SkillSlotIndex.Active4) == 0 || QCoolTime > 0)
                        infoStr += "<color=red>R</color> ";


                    if (infoText == null)
                    {
                        CheatMain.instance.Log("infotext cannot null.."); 
                        return;
                    }
                    else
                    {
                        infoText.text = infoStr;
                    }
                }
                catch (Exception e)
                {
                    CheatCanvas.instance.Log(e.Message); 
                }
            }

            public void OnGUI()
            {
                if (this.player != null)
                {
                    if (this.player == CheatMain.instance.mine)
                    {
                        GUILayout.Label(this.player.GetType().Name);
                        GUILayout.Label(this.transform.position.ToString());
                        GUILayout.Label((this.infoText != null).ToString());
                        GUILayout.Label(this.player.GetType().Name);

                    }
                }
            }
            public override void Update()
            {
                base.Update();
                TickUpdator(CooltimeUpdate);
            }
        }
    }
}
