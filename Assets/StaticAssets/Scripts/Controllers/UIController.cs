using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIController {
    private const string GAME_HUD_PREFAB_PATH = "Prefabs/GameHUD";

    private GameObject HUD;

    public Canvas Canvas { get; private set; }

    public void Init() {
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        HUD = GameObject.Instantiate(Resources.Load<GameObject>(GAME_HUD_PREFAB_PATH));
        HUD.transform.SetParent(Canvas.transform, false);
    }
}
