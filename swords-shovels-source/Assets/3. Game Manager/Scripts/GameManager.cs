using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    // 3. keep track of game state
    // 4. generate other persistant systems
    // 

    #region Variable declarations

    public GameObject[] SystemPrefabs;
    // REMEMBER you can only Count these, can't use Length
    private List<GameObject> _instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    private List<AsyncOperation> _loadOperations;

    #endregion
    

    private void Start() 
    {

        // Very good function for persistance of the game manager.
        // For memory efficiency and better performance I would 
        // like to unload boot. This would solve losing the game
        // manager part.
        DontDestroyOnLoad(gameObject);
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();
        InstantiateSystemPrefab();

        LoadLevel("Main");
    }

    #region Load methods
    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[Game Manager] Unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[Game Manager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;

    }
    #endregion
    
    #region Private callback methods

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            // dispatch messages
            // transition between scenes
        }
        Debug.Log("Load Complete!");
    }

    private void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete!");
    }
        
    #endregion
    
    #region Instantiate and Destroy methods
    
    private void InstantiateSystemPrefab()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    #endregion
    
}
