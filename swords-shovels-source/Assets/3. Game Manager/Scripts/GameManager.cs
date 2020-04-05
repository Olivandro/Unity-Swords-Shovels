using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : Singleton<GameManager>
{


    #region Variable declarations

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;
    // REMEMBER you can only Count these, can't use Length
    private List<GameObject> _instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    private List<AsyncOperation> _loadOperations;
    private GameState _currentGameState = GameState.PREGAME;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    public Events.EventGameState OnGameStateChanged;

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

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

        // LoadLevel("Main");
    }

    private void Update() 
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            // GameManager.Instance.StartGame();
            // _mainMenu.FadeOut();
        }    
    }

    private void UpdateState(GameState state)
    {
        GameState _previousGameState = _currentGameState;
        // This might be a problem child...
        // Keep track off and see if your 
        // have to appy ToString() to each
        // case.
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, _previousGameState);
    }

    private void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
        
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

            if (_loadOperations.Count == 0)
            {
                 UpdateState(GameState.RUNNING);
            }
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


    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        // if (_currentGameState == GameState.RUNNING)
        // {
        //     UpdateState(GameState.PAUSED);
        // }
        // else
        // {
        //     UpdateState(GameState.RUNNING);
        // }

        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void QuitGame()
    {
        // Clean up 
        // Autosaving
        Application.Quit();
        
    }
}
