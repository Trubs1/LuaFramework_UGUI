﻿///Copyright (c) 2019 WangQiang(279980661@qq.com)
///description: 面板管理器
///author:Trubs (WQ)
///Date:2019/03/05

#define DirectLoadResourceMode //直接加载资源模式,ui预制等无需build资源,即改即见(若要在编辑器内测试bundle方式加载,需注释此行)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;


namespace LuaFramework
{
    public class PanelManager : Manager
    {

        public GameObject CreatePanelBySync(string panelName, LuaTable table)
        {
            GameObject prefab;

#if UNITY_EDITOR && DirectLoadResourceMode
            //Resources.LoadAssetAtPath被废弃了,而这个是UnityEditor下的,打包的时候会报错,暂时先这样吧,通过这样子预处理完美解决        
            prefab = (GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI/Panels/" + panelName + ".prefab", typeof(GameObject));
#else
            prefab = ResManager.LoadAssetSync<GameObject>("prefabs_ui_panels", panelName);
#endif

            if (prefab == null)
            {
                Debug.LogError("~~~~CreatePanelSync faile::>> " + panelName + " " + prefab);
                return null;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = panelName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            var behaviour = go.GetComponent<LuaBehaviour>();
            behaviour.Initiate(table);
            return go;
        }


        private Transform parent;

        Transform Parent
        {
            get
            {
                if (parent == null)
                {
                    GameObject go = GameObject.Find("Canvas");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }

        public void ClosePanel(string name)
        {
            var panelName = name + "Panel";
            var panelObj = Parent.Find(panelName);
            if (panelObj == null) return;
            Destroy(panelObj.gameObject);
        }
    }
}