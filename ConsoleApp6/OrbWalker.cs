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
    public class OrbWalker : CheatSingleton<OrbWalker>
    {
        public enum ECharacterCode
        {
            Jackie = 1,
            Aya = 2,
            Fiora = 3,
            Magnus = 4,
            Zahir = 5,
            Nadine = 6,
            Hyunwoo = 7,
            Hart = 8,
            Isol = 9,
            LIDailin = 10,
            Yuki = 11,
            Hyejin = 12,
            Xiukai = 13,
            Chiara = 14,
            Sisella = 15,
            Silvia = 16,
            Adrina = 17,
            Shoichi = 18
        }
        /// <summary>
        /// 다음 공격 대기시간
        /// </summary>
        public float attackTick = 0.0f;
        /// <summary>
        /// 공격 애니메이션 및 딜레이 발생시 true
        /// </summary>
        public bool attackSwitch = false;

        /// <summary>
        /// 프레임 대기 틱
        /// </summary>
        public int frameTickMax = 3;
        /// <summary>
        /// 프레임 현재 틱
        /// </summary>
        public int frameTick= 0;
        public bool alreadyMoveTo = false;
        /// <summary>
        /// 시간 보정값
        /// </summary>
        public float f;

        /// <summary>
        /// 오브워크
        /// </summary>
        /// 
        public float GetProjectileWait(ECharacterCode characterCode)
        {
            var mine = CheatMain.instance.mine;
            if (characterCode == ECharacterCode.Nadine)
            {
                var weaponType = mine.GetEquipWeaponMasteryType();
                var time = Blis.Common.NadineSkillNormalAttackData.inst.NormalAttackDelay[(int)weaponType];
                return time;
            }
            else if (characterCode == ECharacterCode.Aya)
            {
                var weaponType = mine.GetEquipWeaponMasteryType();
                var time = Blis.Common.AyaSkillNormalAttackData.inst.NormalAttackDelay[(int)weaponType];
                return time;
            }
            else if (characterCode == ECharacterCode.Hyejin)
            {
                var weaponType = mine.GetEquipWeaponMasteryType();
                var time = Blis.Common.HyejinSkillData.inst.NormalAttackDelay[(int)weaponType];
                return time;
            }
            else if (characterCode == ECharacterCode.Hart)
            {
                var weaponType = mine.GetEquipWeaponMasteryType();
                var time = Blis.Common.HartSkillNormalAttackData.inst.NormalAttackDelay;
                return time;
            }
            else if (characterCode == ECharacterCode.Silvia)
            {
                var weaponType = mine.GetEquipWeaponMasteryType();
                var time = Blis.Common.SilviaSkillHumanData.inst.NormalAttackDelay[weaponType];
                return time;
            }
            else if (characterCode == ECharacterCode.Chiara)
            {
                var weaponType = mine.GetEquipWeaponMasteryType();
                var time = Blis.Common.ChickenSkillNormalAttackData.inst.AttackDelay;
                return time;
            }
            return -1;

        }
        public bool IsOrbWalkSupport(ECharacterCode characterCode)
        {
            var f = GetProjectileWait(characterCode);
            if (f <= 0)
            {
                return false;
            }
            {
                return true;
            }
        }

        public void Update()
        {
            if (enable)
            {
                OrbWalk();  
            }
            
        }
    

        private bool UseSkill()
        {
            var b = PlayerController.inst.PlayerSkill.IsPlaying(SkillSlotIndex.Active1);
            var f = PlayerController.inst.PlayerSkill.IsPlaying(SkillSlotIndex.Active2);
            var d = PlayerController.inst.PlayerSkill.IsPlaying(SkillSlotIndex.Active3);
            var e = PlayerController.inst.PlayerSkill.IsPlaying(SkillSlotIndex.Active4); 
            if (b || f || d || e) 
                return true;
          
            return false;
        }
        public void OrbWalk()
        {
          
            if(alreadyMoveTo)
            {
                frameTick++;
                if(frameTickMax == frameTick)
                {
                    alreadyMoveTo = false;
                    frameTick = 0;
                }
            }
            var mine = CheatMain.instance.mine;
            if (Input.GetKey(KeyCode.Space))
            {
                //공격 애니메이션 여부 체크
                var isAttack = PlayerController.inst.PlayerSkill.IsPlaying(SkillSlotIndex.Attack);
                var isUseSkill = UseSkill();
               
                float projectileLaunchWait =0;

                var weaponType = mine.GetEquipWeaponMasteryType();
                projectileLaunchWait = GetProjectileWait((ECharacterCode)mine.CharacterCode);


                if(isUseSkill)
                {
                    PlayerController.Instance.OnMousePressed(GameInputEvent.Move, GameInput.inst.GetMousePosition());
                    attackTick = 0;
                    attackSwitch = false;
                    return;
                }
                //어택 스위치가 꺼져있는경우
                //어택 스위치는 공격애니메이션 시작시 켜짐.
                if (attackSwitch == false)
                {
                    //이동시도
                    if (!isAttack)
                    {
                        if (alreadyMoveTo == false)
                        {
                            PlayerController.Instance.OnMousePressed(GameInputEvent.MoveToAttack, GameInput.inst.GetMousePosition());
                            alreadyMoveTo = true;
                        }
                    }
                    else
                        attackSwitch = true;
                }
                else if (attackSwitch)
                {
                    //float maxWaitTick = ClientService.inst.MyPlayer.Character.Stat.AttackDelay;
                    // - projectileLaunchWait;

                    float moveCancleableTime = projectileLaunchWait * ClientService.inst.MyPlayer.Character.Stat.AttackDelay; 
                    float maxWaitTick = ClientService.inst.MyPlayer.Character.Stat.AttackDelay - moveCancleableTime;
                  
                    //투사체 발사 대기시간 끝나면 플레이어 이동처리

                    if (attackTick > moveCancleableTime && attackTick < maxWaitTick)
                    {
                        if (alreadyMoveTo == false)
                        {
                            PlayerController.Instance.OnMousePressed(GameInputEvent.Move, GameInput.inst.GetMousePosition());
                            alreadyMoveTo = true;
                        }
                    }
                    else if (attackTick > maxWaitTick)
                    {
                        Log(maxWaitTick);
                        attackTick = 0;
                        attackSwitch = false;
                        return;
                    }
                    attackTick += Time.deltaTime;
                }

       
            } 
            if (Input.GetKeyUp(KeyCode.Space))
            {
                attackTick = 0;
                attackSwitch = false;
            } 
            if (Input.GetKey(KeyCode.G))
            {
                PlayerController.Instance.OnMousePressed(GameInputEvent.Move, GameInput.inst.GetMousePosition());

            } 
        }
    }
}
