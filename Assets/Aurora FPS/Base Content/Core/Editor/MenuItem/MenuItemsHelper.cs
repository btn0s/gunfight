/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;
using AuroraFPSRuntime.AI;

namespace AuroraFPSEditor
{
    internal static class MenuItemsHelper
    {
        #region [AI]
        [MenuItem(AuroraFPSProduct.Name + "/Create/AI/Destionation Map", false, 51)]
        private static void CreateDestinationMap()
        {
            GameObject destinationMap = new GameObject("Destination Map");
            destinationMap.AddComponent<DestinationMap>();
            GUIContent content = new GUIContent("Destination map created!");
            destinationMap.isStatic = true;
            EditorGUIUtility.PingObject(destinationMap);
            Selection.activeGameObject = destinationMap;
            EditorWindow.GetWindow<SceneView>().ShowNotification(content);
        }
        #endregion

        #region [Managers]
        [MenuItem(AuroraFPSProduct.Name + "/Create/Managers/Spawn Manager", false, 51)]
        private static void CreateSpawnManager()
        {
            GameObject spawnManager = new GameObject("Spawn Manager");
            spawnManager.AddComponent<SpawnManager>();
            GUIContent content = new GUIContent("Spawn manager created!");
            spawnManager.isStatic = true;
            EditorGUIUtility.PingObject(spawnManager);
            Selection.activeGameObject = spawnManager;
            EditorWindow.GetWindow<SceneView>().ShowNotification(content);
        }

        [MenuItem(AuroraFPSProduct.Name + "/Create/Managers/Pool Manager", false, 52)]
        private static void CreatePoolManager()
        {
            GameObject poolManager = GameObject.FindObjectOfType<PoolManager>()?.gameObject;
            GUIContent content = new GUIContent("Pool manager already created.");
            if (poolManager == null)
            {
                poolManager = new GameObject("Pool Manager");
                poolManager.AddComponent<PoolManager>();
                content.text = "Pool manager created!";
            }
            poolManager.isStatic = true;
            EditorGUIUtility.PingObject(poolManager);
            Selection.activeGameObject = poolManager;
            EditorWindow.GetWindow<SceneView>().ShowNotification(content);
        }

        [MenuItem(AuroraFPSProduct.Name + "/Create/Managers/Terrain Manager", false, 53)]
        private static void CreateTerrainManager()
        {
            GameObject terrainManager = GameObject.FindObjectOfType<TerrainManager>()?.gameObject;
            GUIContent content = new GUIContent("Terrain manager already created.");
            if (terrainManager == null)
            {
                terrainManager = new GameObject("Terrain Manager");
                terrainManager.AddComponent<TerrainManager>();
                content.text = "Terrain manager created!";
            }
            terrainManager.isStatic = true;
            EditorGUIUtility.PingObject(terrainManager);
            Selection.activeGameObject = terrainManager;
            EditorWindow.GetWindow<SceneView>().ShowNotification(content);
        }
        #endregion
    }
}