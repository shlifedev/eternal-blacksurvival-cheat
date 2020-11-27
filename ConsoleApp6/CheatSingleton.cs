using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Blis.Client.Cheat
{
    public class CheatSingleton<T> : MonoBehaviour where T : CheatSingleton<T>
    {
        public static bool gui;
        public bool enable = true;
        public static T instance
        {
            get
            { 
                if (singleTon == null)
                {
                    var obj = new GameObject();
                    var component = obj.AddComponent<T>();
                    singleTon = component;
                    return singleTon; 
                }
                return singleTon;
            }
        }
        private static T singleTon;
 
        public void CreateInstance(bool dontDestroy = false) {
            if(dontDestroy)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        public void Toggle()
        {
            enable = !enable;
        } 

        public void ToggleGUI()
        {
            gui = !gui;
        }

        public void Log(object content)
        {
            GameUI.inst.ChattingUi.AddChatting("치트로그", "에비츄", content.ToString(), false, false, true);
        }
    }
}
