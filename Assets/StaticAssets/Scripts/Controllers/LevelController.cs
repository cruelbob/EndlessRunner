using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Direction {
    Forth,
    Right,
    Back,
    Left
}

public enum CardinalDirection {
    North,
    West,
    South,
    East 
}

public class LevelController {
    public Action<int> OnNewResult;
    public Action<LevelBehaviour> OnNewLevel;

    private const string LEVEL_PREFAB_PATH = "Prefabs/Level";

    public LevelBehaviour Level { get; private set; }

    private LevelBehaviour levelPrefab;

    public void Init() {
        levelPrefab = Resources.Load<LevelBehaviour>(LEVEL_PREFAB_PATH);
    }

    public void CreateNewLevel() {
        if(Level != null) {
            GameObject.Destroy(Level.gameObject);
        }
        Level = GameObject.Instantiate(levelPrefab);
        Level.transform.SetParent(MainController.Instance.transform, false);
        Level.OnNewResult += OnNewResult;
        Level.Init();
        if(OnNewLevel != null) {
            OnNewLevel(Level);
        }
    }

    public void Remove() {
        GameObject.Destroy(Level.gameObject);
        Level = null;
    }
}