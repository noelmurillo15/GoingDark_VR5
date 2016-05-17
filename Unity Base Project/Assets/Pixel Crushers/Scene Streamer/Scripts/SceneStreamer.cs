﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
using UnityEngine.SceneManagement;
#endif

namespace PixelCrushers.SceneStreamer {

	/// <summary>
	/// SceneStreamer is a singleton MonoBehavior used to load and unload scenes that contain
	/// pieces of the game world. You can use it to implement continuous worlds. The piece
	/// of the world containing the player is called the "current scene." SceneStreamer 
	/// automatically loads neighboring scenes up to a distance you specify and unloads 
	/// scenes beyond that distance.
	/// 
	/// All scenes are loaded into a single containing GameObject. As the player moves through
	/// the world, scenes are added and removed from this container.
	/// 
	/// You'll usually only call the static method SetCurrentScene(), and often only through 
	/// the SetStartScene script.
	/// 
	/// You can manually load and unload scenes using LoadScene() and UnloadScene(), and
	/// check whether a scene is loaded with IsSceneLoaded().
	/// 
	/// You can also hook into the events On Loading and On Loaded to get notification when a
	/// scene begins loading and when it's done loading.
	/// 
	/// SceneStreamer works with Unity and Unity Pro. In Unity 4 free, it uses LoadLevelAdditive. 
	/// In Unity 5 or Unity 4 Pro, it uses LoadLevelAdditiveAsync.
	/// </summary>
	[AddComponentMenu("Scene Streamer/Scene Streamer")]
	public class SceneStreamer : MonoBehaviour
	{

		/// <summary>
		/// The GameObject that will contain all loaded scenes.
		/// </summary>
		[Tooltip("The GameObject that will contain all loaded scenes")]
		public GameObject sceneContainer = null;

		/// <summary>
		/// The max number of neighbors to load out from the current scene.
		/// </summary>
		[Tooltip("Max number of neighbors to load out from the current scene")]
		public int maxNeighborDistance = 1;

		/// <summary>
		/// A failsafe in case loading hangs. After this many seconds, the SceneStreamer
		/// will stop waiting for the scene to load.
		/// </summary>
		[Tooltip("(Failsafe) If scene doesn't load after this many seconds, stop waiting")]
		public float maxLoadWaitTime = 10f;

		[System.Serializable] public class StringEvent : UnityEvent<string> {}
		[System.Serializable] public class StringAsyncEvent : UnityEvent<string, AsyncOperation> {}

		public StringAsyncEvent onLoading = new StringAsyncEvent();

		public StringEvent onLoaded = new StringEvent();

		/// <summary>
		/// The name of the player's current scene.
		/// </summary>
		private string m_currentSceneName = null;

		/// <summary>
		/// The names of all loaded scenes.
		/// </summary>
		private HashSet<string> m_loaded = new HashSet<string>();

		/// <summary>
		/// The names of all scenes that are in the process of being loaded.
		/// </summary>
		private HashSet<string> m_loading = new HashSet<string>();

		/// <summary>
		/// The names of all scenes within maxNeighborDistance of the current scene.
		/// This is used when determining which neighboring scenes to load or unload.
		/// </summary>
		private HashSet<string> m_near = new HashSet<string>();

		private static bool _applicationIsQuitting = false;

		private static object _lock = new object();

		private static SceneStreamer _instance = null;
		
		private static SceneStreamer instance
		{
			get
			{ // Adapted from: http://wiki.unity3d.com/index.php/Singleton
				if (_applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance SceneStreamer " +
					                 "already destroyed on application quit." +
					                 " Won't create again - returning null.");
					return null;
				}
				lock(_lock)
				{
					if (_instance == null)
					{
						_instance = FindObjectOfType(typeof(SceneStreamer)) as SceneStreamer;
						if ( FindObjectsOfType(typeof(SceneStreamer)).Length > 1 )
						{
							Debug.LogError("[Singleton] Something went really wrong " +
							               " - there should never be more than 1 SceneStreamer!" +
							               " Reopening the scene might fix it.");
							return _instance;
						}
						if (_instance == null)
						{
							_instance = new GameObject("Scene Loader", typeof(SceneStreamer)).GetComponent<SceneStreamer>();
						}
					}
					return _instance;
				}
			}
			set
			{
				_instance = value;
			}
		}

		public void Awake()
		{
			if (_instance)
			{
				Destroy(this);
			} else
			{
				_instance = this;
				Object.DontDestroyOnLoad(this);
				if (!sceneContainer) sceneContainer = gameObject;
			}
		}

		public void OnDestroy()
		{
			_applicationIsQuitting = true;
		}

		/// <summary>
		/// Sets the current scene, loads it, and manages neighbors. The scene must be in your
		/// project's build settings.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public void SetCurrent(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, m_currentSceneName)) return;
			StartCoroutine(LoadCurrentScene(sceneName));
		}

		/// <summary>
		/// Loads a scene as the current scene and manages neighbors, loading scenes
		/// within maxNeighborDistance and unloading scenes beyond it.
		/// </summary>
		/// <returns>The current scene.</returns>
		/// <param name="sceneName">Scene name.</param>
		private IEnumerator LoadCurrentScene(string sceneName)
		{
			// First load the current scene:
			m_currentSceneName = sceneName;
			if (!IsLoaded(m_currentSceneName)) Load(sceneName);
			float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
			while ((m_loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime))
			{
				yield return null;
			}

			// Next load neighbors up to maxNeighborDistance, keeping track
			// of them in the near list:
			m_near.Clear();
			LoadNeighbors(sceneName, 0);
			failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
			while ((m_loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime))
			{
				yield return null;
			}

			// Finally unload any scenes not in the near list:
			UnloadFarScenes();
		}

		/// <summary>
		/// Loads neighbor scenes within maxNeighborDistance, adding them to the near list.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="distance">Distance.</param>
		private void LoadNeighbors(string sceneName, int distance)
		{
			if (m_near.Contains(sceneName)) return;
			m_near.Add(sceneName);
			if (distance >= maxNeighborDistance) return;
			GameObject scene = GameObject.Find(sceneName);
			NeighboringScenes neighboringScenes = (scene) ? scene.GetComponent<NeighboringScenes>() : null;
			if (!neighboringScenes) neighboringScenes = CreateNeighboringScenesList(scene);
			if (!neighboringScenes) return;
            for (int i = 0; i < neighboringScenes.sceneNames.Length; i++)
            {
				Load(neighboringScenes.sceneNames[i], LoadNeighbors, distance + 1);
			}
		}

		/// <summary>
		/// Creates the neighboring scenes list. It's faster to manually add a
		/// NeighboringScenes script to your scene's root object; this method
		/// builds it manually if it's missing, but requires the scene to have
		/// SceneEdge components.
		/// </summary>
		/// <returns>The neighboring scenes list.</returns>
		/// <param name="scene">Scene.</param>
		private NeighboringScenes CreateNeighboringScenesList(GameObject scene)
		{
			if (!scene) return null;
			NeighboringScenes neighboringScenes = scene.AddComponent<NeighboringScenes>();
			HashSet<string> neighbors = new HashSet<string>();
            var sceneEdges = scene.GetComponentsInChildren<SceneEdge>();
            for (int i = 0; i < sceneEdges.Length; i++)
            { 
				neighbors.Add(sceneEdges[i].nextSceneName);
			}
			neighboringScenes.sceneNames = new string[neighbors.Count];
			neighbors.CopyTo(neighboringScenes.sceneNames);
			return neighboringScenes;
		}

		/// <summary>
		/// Determines whether a scene is loaded.
		/// </summary>
		/// <returns><c>true</c> if loaded; otherwise, <c>false</c>.</returns>
		/// <param name="sceneName">Scene name.</param>
		public bool IsLoaded(string sceneName) {
			return m_loaded.Contains(sceneName);
		}

		/// <summary>
		/// Loads a scene.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public void Load(string sceneName)
		{
			Load(m_currentSceneName, null, 0);
		}

		private delegate void InternalLoadedHandler(string sceneName, int distance);
		
		/// <summary>
		/// Loads a scene and calls an internal delegate when done. The delegate is
		/// used by the LoadNeighbors() method.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="loadedHandler">Loaded handler.</param>
		/// <param name="distance">Distance from the current scene.</param>
		private void Load(string sceneName, InternalLoadedHandler loadedHandler, int distance)
		{
			if (IsLoaded(sceneName))
			{
				if (loadedHandler != null) loadedHandler(sceneName, distance);
				return;
			}
			m_loading.Add(sceneName);
			#if UNITY_4_6
			if (Application.HasProLicense())
			{
				StartCoroutine(LoadAdditiveAsync(sceneName, loadedHandler, distance));
			} else
			{
				StartCoroutine(LoadAdditive(sceneName, loadedHandler, distance));
			}
			#else
				StartCoroutine(LoadAdditiveAsync(sceneName, loadedHandler, distance));
            #endif
        }

		/// <summary>
		/// (Unity Pro) Runs Application.LoadLevelAdditiveAsync() and calls FinishLoad() when done.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="loadedHandler">Loaded handler.</param>
		/// <param name="distance">Distance.</param>
		private IEnumerator LoadAdditiveAsync(string sceneName, InternalLoadedHandler loadedHandler, int distance)
		{
            #if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            AsyncOperation asyncOperation = Application.LoadLevelAdditiveAsync(sceneName);
            #else
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            #endif
            onLoading.Invoke(sceneName, asyncOperation);
			yield return asyncOperation;
			FinishLoad(sceneName, loadedHandler, distance);
		}

		/// <summary>
		/// (Unity) Runs Application.LoadLevelAdditive() and calls FinishLoad() when done.
		/// This coroutine waits two frames to wait for the load to complete.
		/// </summary>
		/// <returns>The additive.</returns>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="loadedHandler">Loaded handler.</param>
		/// <param name="distance">Distance.</param>
		private IEnumerator LoadAdditive(string sceneName, InternalLoadedHandler loadedHandler, int distance)
		{
            #if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            Application.LoadLevelAdditive(sceneName);
            #else
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            #endif
            onLoading.Invoke(sceneName, null);
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			FinishLoad(sceneName, loadedHandler, distance);
		}

		/// <summary>
		/// Called when a level is done loading. Updates the loaded and loading lists, and 
		/// calls the loaded handler.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="loadedHandler">Loaded handler.</param>
		/// <param name="distance">Distance.</param>
		private void FinishLoad(string sceneName,InternalLoadedHandler loadedHandler, int distance)
		{
			GameObject scene = GameObject.Find(sceneName);
			if (scene == null) Debug.LogWarning("Scene Streamer: Can't find loaded scene named '" + sceneName + "'");
			if (scene) scene.transform.parent = sceneContainer.transform;
			m_loading.Remove(sceneName);
			m_loaded.Add(sceneName);
			onLoaded.Invoke(sceneName);
			if (loadedHandler != null) loadedHandler(sceneName, distance);
		}

		/// <summary>
		/// Unloads scenes beyond maxNeighborDistance. Assumes the near list has already been populated.
		/// </summary>
		private void UnloadFarScenes() 
		{
			HashSet<string> far = new HashSet<string>(m_loaded);
			far.ExceptWith(m_near);
			foreach (var sceneName in far) 
			{
				Unload(sceneName);
			}
		}

		/// <summary>
		/// Unloads a scene.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public void Unload(string sceneName) 
		{           
			Destroy(GameObject.Find(sceneName).gameObject);
			m_loaded.Remove(sceneName);
		}

		/// <summary>
		/// Sets the current scene.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public static void SetCurrentScene(string sceneName)
		{
			instance.SetCurrent(sceneName);
		}

		/// <summary>
		/// Determines if a scene is loaded.
		/// </summary>
		/// <returns><c>true</c> if loaded; otherwise, <c>false</c>.</returns>
		/// <param name="sceneName">Scene name.</param>
		public static bool IsSceneLoaded(string sceneName)
		{
			return instance.IsLoaded(sceneName);
		}

		/// <summary>
		/// Loads a scene.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public static void LoadScene(string sceneName)
		{
			instance.Load(sceneName);
		}

		/// <summary>
		/// Unloads a scene.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public static void UnloadScene(string sceneName)
		{
			instance.Unload(sceneName);
		}
		
	}

}