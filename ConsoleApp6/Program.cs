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

namespace Blis.Client
{ 
    public class LocalPlayerCharacterCheatBehaviour : MonoBehaviour
    {  
        public static LocalPlayerCharacterCheatBehaviour instance
        {
            get
            {
                if (ss == null)
                {
                    var a = new GameObject();
                    var xx = a.AddComponent<LocalPlayerCharacterCheatBehaviour>();
                    ss = xx;
                    return ss;

                }
                return ss;
            }
        }
        private static LocalPlayerCharacterCheatBehaviour ss;
        public List<LocalPlayerCharacter> players = new List<LocalPlayerCharacter>();
        public List<LocalItemBox> itemBoxs = new List<LocalItemBox>();
        public List<LocalMonster> monsters = new List<LocalMonster>();
        public List<LocalMovableCharacter> playerAndMobs = new List<LocalMovableCharacter>();
        public Dictionary<int, List<Renderer>> MeshDatas = new Dictionary<int, List<Renderer>>();
        public Dictionary<int, List<Collider>> ColDatas = new Dictionary<int, List<Collider>>();

        public bool enableCheat = false;
        public bool enableGUI = false;
        public bool enableMonsterMaphack = false;
        public LocalPlayerCharacter mine;
 
        public void InitMine()
        {
            players.ForEach(x => {
                var id = x.ObjectId;
                if (id == PlayerController.inst.myObjectId)
                {
                    mine = x;
                }
            });
        }

        /// <summary>
        /// LocalPlayerCharacter Init에서 이걸 실행할것
        /// </summary>
        /// <param name="player"></param>
        public void Init()
        {
            players.Clear();
            itemBoxs.Clear();
            monsters.Clear();
            playerAndMobs.Clear();

            players.AddRange(FindObjectsOfType<LocalPlayerCharacter>());
            monsters.AddRange(FindObjectsOfType<LocalMonster>());
            itemBoxs.AddRange(FindObjectsOfType<LocalItemBox>());

            playerAndMobs.AddRange(players);
            playerAndMobs.AddRange(monsters);
        }


        Coroutine meshUpdateRoutine;

        public void Awake()
        {
            meshUpdateRoutine = StartCoroutine(UpdateMeshs());
            StartCoroutine(PlayerMinimapHook());
        }
        public void Log(object content)
        {
            GameUI.inst.ChattingUi.AddChatting("치트로그", "에비츄", content.ToString(), false, false, true);
        }
        public void EnableCheat()
        {
            foreach (var v in players)
                if (v.Nickname == "햄스터에비츄")
                    mine = v;
            enableCheat = true;
        }
        public void UpdateMeshList()
        {
            MeshDatas.Clear();
            ColDatas.Clear();
            try
            {
                foreach (var v in monsters)
                {
                    if (!MeshDatas.ContainsKey(v.ObjectId))
                    {
                        List<Renderer> mrs = new List<Renderer>();
                        List<Collider> cols = new List<Collider>();
                        mrs.AddRange(v.gameObject.GetComponents<Renderer>());
                        mrs.AddRange(v.gameObject.GetComponentsInParent<Renderer>());
                        mrs.AddRange(v.gameObject.GetComponentsInChildren<Renderer>());
                        cols.AddRange(v.gameObject.GetComponents<Collider>());
                        cols.AddRange(v.gameObject.GetComponentsInParent<Collider>());
                        cols.AddRange(v.gameObject.GetComponentsInChildren<Collider>());

                        
                     
                        MeshDatas.Add(v.ObjectId, mrs);
                        ColDatas.Add(v.ObjectId, cols); 
                    }
                }
                foreach (var v in players)
                {
                    if (!MeshDatas.ContainsKey(v.ObjectId))
                    {
                        List<Renderer> mrs = new List<Renderer>();
                        List<Collider> cols = new List<Collider>();
                        mrs.AddRange(v.gameObject.GetComponents<Renderer>());
                        mrs.AddRange(v.gameObject.GetComponentsInParent<Renderer>());
                        mrs.AddRange(v.gameObject.GetComponentsInChildren<Renderer>());
                        cols.AddRange(v.gameObject.GetComponents<Collider>());
                        cols.AddRange(v.gameObject.GetComponentsInParent<Collider>());
                        cols.AddRange(v.gameObject.GetComponentsInChildren<Collider>());
                        MeshDatas.Add(v.ObjectId, mrs);
                        ColDatas.Add(v.ObjectId, cols);
                    }
                }
            }
            catch (Exception e)
            {
                Log(".2" + e.Message);
            }
        }
        public IEnumerator UpdateMeshs()
        {
            while (true)
            {
                if (enableCheat)
                {
                    UpdateMeshList();
                    foreach (var v in MeshDatas)
                    {
                        if (v.Value.Count != 0)
                        {
                            foreach (var x in v.Value)
                            {
                                x.enabled = true;
                            }
                        }

                    }
                    foreach (var v in ColDatas)
                    {
                        if (v.Value.Count != 0)
                        {
                            foreach (var x in v.Value)
                            {
                                x.enabled = true;
                            }
                        }
                    }
                } 
                yield return new WaitForSeconds(1.0f);
            }
        }
        public void PlayerAndMonsterUpdate(bool toggle)
        {
            if (meshUpdateRoutine != null)
            {
                StopCoroutine(meshUpdateRoutine);
                meshUpdateRoutine = null;
            }

            monsters.Clear();
            monsters.AddRange(FindObjectsOfType<LocalMonster>());
            foreach (var v in monsters)
            {
                if (toggle == true)
                {
                    if (v != null)
                    {
                        if (v.IsAlive)
                        {
                            v.ShowMiniMapIcon(MiniMapIconType.None);
                        }
                    }

                }
                else
                {
                    if (v != null)
                    {
                        v.HideMiniMapIcon(MiniMapIconType.None);
                    }
                }
            }
            foreach (var v in players)
            {
                if (v != mine)
                    v.OnSight();
            }



            UpdateMeshList();
            foreach (var v in MeshDatas)
            {
                if (v.Value.Count != 0)
                {
                    foreach (var x in v.Value)
                    {
                        x.enabled = true;
                    }
                }

            }
            foreach (var v in ColDatas)
            {
                if (v.Value.Count != 0)
                {
                    foreach (var x in v.Value)
                    {
                        x.enabled = true;
                    }
                }
            }
            meshUpdateRoutine = StartCoroutine(UpdateMeshs());
        }
        public IEnumerator PlayerMinimapHook()
        { 
            while (true)
            {
                if (enableCheat)
                {
                    foreach (var v in players)
                    {
                        if (v != null)
                        {
                            v.ShowMiniMapIcon(MiniMapIconType.None);
                            v.ShowMapIcon(MiniMapIconType.None);
                            if (mine != v)
                            {
                                v.FloatingUi.Show();
                            }

                        }

                        yield return null;
                    }
                }
                yield return new WaitForSeconds(0.015f);
            }
        }
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Insert))
            {
                Log("Enable");
                EnableCheat();
            }
            if (Input.GetKeyUp(KeyCode.Delete))
            {
                enableGUI = !enableGUI;
            } 
            if (Input.GetKeyUp(KeyCode.CapsLock))
            {
                enableMonsterMaphack = !enableMonsterMaphack;
                Log($"Mob Update");
                PlayerAndMonsterUpdate(true);
                foreach (var v in monsters)
                {
                    if (v.MonsterType == MonsterType.Bear && v.IsAlive)
                    { 
                        GameObject minimapObj = MonoBehaviourInstance<GameUI>.inst.Minimap.UIMap.AddPing(PingType.Warning, v.GetPosition());
                        GameObject mapwindowObj = MonoBehaviourInstance<GameUI>.inst.MapWindow.UIMap.AddPing(PingType.Warning,  v.GetPosition());
                    }
                    else
                    {

                    }
                }
            } 
        }



        public void OnGUI()
        {
            if(enableGUI)
            {
                if (GUILayout.Button("CreateCanvas"))
                {
                    Log("Create Canvas");
                    try
                    {
                        CreateCanvas(); 
                    }
                    catch (Exception e)
                    {
                        Log("Error :" + e.Message);
                    }
                }
                if (GUILayout.Button("CreateTestText"))
                {
                    try
                    {
                        CreateTestText();
                    }
                    catch (Exception e)
                    {
                        Log("Error :" + e.Message);
                    } 
                }

                if (GUILayout.Button("컬라이더 테스트"))
                {
                    foreach (var value in ColDatas)
                    {
                        foreach(var v in value.Value)
                        {
                            Log(v.name +" : " + value.Value.GetType().Name);
                        } 
                    }
                }
            }
        }
        #region CanvasDrawer
        public Canvas canvas;

        public void CreateCanvas()
        { 
            if (canvas == null)
            {
                GameObject obj = new GameObject();
                canvas = obj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                CanvasScaler cs = obj.AddComponent<CanvasScaler>();
                cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                cs.referenceResolution = new Vector2(1920, 1080);
            }
        }


        public void CreateTestText()
        {
            Log("생성 텍스트");
            foreach(var v in monsters)
            {
                var cam = MobaCamera.inst;
                var targetCamera = MobaCamera.inst.transform.GetComponentInChildren<Camera>();

                Log(targetCamera == Camera.main); 
                 
                GameObject obj = new GameObject();
                Text text = obj.AddComponent<Text>();
                text.transform.SetParent(canvas.transform);
                text.transform.localPosition = new Vector3(0, 0, 0); 
                text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(targetCamera, v.GetPosition());
                text.text = v.MonsterType.ToString();
                text.fontSize = 12;   

                GameObject X = new GameObject();
                Image img = X.AddComponent<Image>();
                img.transform.SetParent(canvas.transform);
                img.transform.localPosition = new Vector3(0, 0, 0); 
                img.rectTransform.sizeDelta = new Vector2(50,50);
                img.rectTransform.position = RectTransformUtility.WorldToScreenPoint(targetCamera, v.GetPosition());
            }
 
        }
        #endregion
    }

}