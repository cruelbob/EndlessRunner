using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InputController {
    private const string LEFT_BUTTON_NAME = "left";
    private const string RIGHT_BUTTON_NAME = "right";
    private const string SPACE_BUTTON_NAME = "space";

    public void Init() {
        MainController.Instance.OnUpdate += OnUpdate;
    }

    private void OnUpdate() {
        if (MainController.Instance.LevelController.Level != null && MainController.Instance.LevelController.Level.Active) {
            if (Input.GetKeyDown(LEFT_BUTTON_NAME)) {
                MainController.Instance.LevelController.Level.Character.MoveLeft();
            } else if (Input.GetKeyDown(RIGHT_BUTTON_NAME)) {
                MainController.Instance.LevelController.Level.Character.MoveRight();
            }

            if (Input.GetKeyDown(SPACE_BUTTON_NAME)) {
                MainController.Instance.LevelController.Level.Character.Jump();
            }
        } else {
            if (Input.GetKeyDown(SPACE_BUTTON_NAME)) {
                MainController.Instance.LevelController.CreateNewLevel();
            }
        }
    }
}
