// Copyright © 2022 Runningbird Studios.  All Rights Reserved.
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.RunningbirdStudios.ZoneRegions.Scripts.Utils
{
    public static class NavMeshUtilities
    {
        /// <summary>
        /// Bakes the NavMesh for Scene using the NavMeshSurface Component on the terrain.
        /// if a NavMeshSurface doesn't exist on the terrain it creates one
        /// </summary>
        /// <param name="zoneScene"></param>
        public static void BakeNavMesh(Scene zoneScene)
        {
            Terrain terrain = GameObject.FindObjectsOfType<Terrain>().Where(x => x.gameObject.scene == zoneScene).FirstOrDefault();
            if (terrain)
            {
                NavMeshSurface navMeshSurface = terrain.GetComponent<NavMeshSurface>();
                //If NavMeshSurface doesn't exist then create one for the terrain.
                if (navMeshSurface == null)
                {
                    navMeshSurface = terrain.gameObject.AddComponent<NavMeshSurface>();
                }
                navMeshSurface.BuildNavMesh();
            }
            EditorSceneManager.SaveScene(zoneScene);
        }

        public static void CreateOffMeshLinks(Scene zoneScene, int linkWidth)
        {
            Terrain terrain = GameObject.FindObjectsOfType<Terrain>().Where(x => x.gameObject.scene == zoneScene).FirstOrDefault();

            if (terrain)
            {
                var numberOfLinks = 0;

                numberOfLinks = (int)terrain.terrainData.size.x / linkWidth;

                Vector3 currentLocation = Vector3.zero;
                Vector3 nextLocation = Vector3.zero;

                GameObject navMeshLinksManager = terrain.gameObject.transform.Find("NavMeshLinksManager").gameObject;

                //Destroy Existing NavMeshLink Manager
                if (navMeshLinksManager != null)
                {
                    GameObject.DestroyImmediate(navMeshLinksManager);
                }

                //Create new NavMeshLink Manager
                navMeshLinksManager = new GameObject();
                navMeshLinksManager.name = "NavMeshLinksManager";
                navMeshLinksManager.transform.parent = terrain.transform;
                navMeshLinksManager.transform.localPosition = new Vector3(0, 0, 0);

                NavMeshLink[] navMeshLinks = GameObject.FindObjectsOfType<NavMeshLink>().Where(x => x.gameObject.scene == zoneScene).ToArray();

                //Destroy any existing NavMeshLinks
                if (navMeshLinks != null && navMeshLinks.Length > 0)
                {
                    foreach (NavMeshLink link in navMeshLinks)
                    {
                        GameObject.DestroyImmediate(link.gameObject);
                    }
                }

                //Create new NavMeshLinks for each side of the terrain
                CreateNavMeshLinksForTerrainSide(numberOfLinks, Side.Left, navMeshLinksManager, terrain, linkWidth);
                CreateNavMeshLinksForTerrainSide(numberOfLinks, Side.Bottom, navMeshLinksManager, terrain, linkWidth);
                CreateNavMeshLinksForTerrainSide(numberOfLinks, Side.Top, navMeshLinksManager, terrain, linkWidth);
                CreateNavMeshLinksForTerrainSide(numberOfLinks, Side.Right, navMeshLinksManager, terrain, linkWidth);
            }
            EditorSceneManager.SaveScene(zoneScene);
        }

        public static void CreateNavMeshLinksForTerrainSide(int numberOfLinks, Side side, GameObject navMeshLinksManager, Terrain terrain, int linkWidth)
        {
            Vector3 currentLocation = Vector3.zero;
            Vector3 nextLocation = Vector3.zero;
            Quaternion rotation = new Quaternion(0, 0, 0, 0);

            switch (side)
            {
                case Side.Top:
                    {
                        currentLocation = new Vector3((float)(linkWidth * 0.5), 0, terrain.terrainData.size.z);
                        nextLocation = new Vector3(currentLocation.x, 0, currentLocation.z);
                        rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    }
                case Side.Bottom:
                    {
                        currentLocation = new Vector3((float)(linkWidth * 0.5), 0, 0);
                        nextLocation = new Vector3(currentLocation.x, 0, currentLocation.z);
                        rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        break;
                    }
                case Side.Left:
                    {
                        currentLocation = new Vector3(0, 0, (float)(linkWidth * 0.5));
                        nextLocation = new Vector3(currentLocation.x, 0, currentLocation.z);
                        rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                        break;
                    }
                case Side.Right:
                    {
                        currentLocation = new Vector3(terrain.terrainData.size.x, 0, (float)(linkWidth * 0.5));
                        nextLocation = new Vector3(currentLocation.x, 0, currentLocation.z);
                        rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                        break;
                    }

                default:
                    break;
            }

            GameObject navMeshLinksSide = new GameObject();
            navMeshLinksSide.name = "NavMeshLinks" + side.ToString() + "Side";
            navMeshLinksSide.transform.parent = navMeshLinksManager.transform;
            navMeshLinksSide.transform.localPosition = new Vector3(0, 0, 0);

            for (int i = 0; i < numberOfLinks; i++)
            {
                GameObject gameObject = new GameObject();
                NavMeshLink navMeshLink = gameObject.AddComponent<NavMeshLink>();
                gameObject.name = "NavMeshLink-" + i;
                navMeshLink.width = linkWidth;
                navMeshLink.bidirectional = true;
                navMeshLink.startPoint = new Vector3(0, 0, -2.5f);
                navMeshLink.endPoint = new Vector3(0, 0, 2.5f);
                navMeshLink.gameObject.transform.parent = navMeshLinksSide.transform;
                navMeshLink.gameObject.transform.localPosition = nextLocation;
                navMeshLink.gameObject.transform.rotation = rotation;
                currentLocation = navMeshLink.transform.localPosition;
                nextLocation = GetNextLocation(currentLocation, side, linkWidth);
            }
        }

        private static Vector3 GetNextLocation(Vector3 currentLocation, Side side, int linkWidth)
        {
            Vector3 nextLocation = new Vector3(0, 0, 0);

            switch (side)
            {
                case Side.Top:
                    {
                        nextLocation = new Vector3(currentLocation.x + linkWidth, 0, currentLocation.z);
                        break;
                    }
                case Side.Bottom:
                    {
                        nextLocation = new Vector3(currentLocation.x + linkWidth, 0, 0);
                        break;
                    }
                case Side.Left:
                    {
                        nextLocation = new Vector3(currentLocation.x, 0, currentLocation.z + linkWidth);
                        break;
                    }
                case Side.Right:
                    {
                        nextLocation = new Vector3(currentLocation.x, 0, currentLocation.z + linkWidth);
                        break;
                    }

                default:
                    break;
            }

            return nextLocation;
        }

        private static Vector3 GetNewPosition(Terrain terrain, Vector3 vector3)
        {
            float height = 0;
            Vector3 newPosition = Vector3.zero;

            height = terrain.SampleHeight(vector3);
            newPosition = new Vector3(vector3.x, height, vector3.z);
            return newPosition;
        }
    }

    public enum Side
    {
        Top,
        Bottom,
        Left,
        Right
    }
}