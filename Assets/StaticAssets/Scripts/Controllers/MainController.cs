using UnityEngine;
using System.Collections;

public class MainController : SingletonBehaviour<MainController> {
    public delegate void OnUpdateHandler();

    public event OnUpdateHandler OnUpdate;

    public ProfileController ProfileController { get; private set; }
    public InputController InputController { get; private set; }
    public LevelController LevelController { get; private set; }
    public UIController UIController { get; private set; }

    private void Start() {
        ProfileController = new ProfileController();
        InputController = new InputController();
        LevelController = new LevelController();
        UIController = new UIController();

        ProfileController.Init();
        InputController.Init();
        LevelController.Init();
        UIController.Init();
    }

    private void Update() {
        if(OnUpdate != null) {
            OnUpdate();
        }
    }
}
