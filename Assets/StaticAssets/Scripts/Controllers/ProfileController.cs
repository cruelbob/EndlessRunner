using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfileController {
    private const string BEST_RESULT_KEY = "BEST_RESULT";

    public int BestResult { get; private set; }

    public void Init() {
        LoadProfile();
        MainController.Instance.LevelController.OnNewResult += OnNewResult;
    }

    private void OnNewResult(int newResult) {
        if(newResult > BestResult) {
            BestResult = newResult;
            SaveProfile();
        }
    }

    private void LoadProfile() {
        if (PlayerPrefs.HasKey(BEST_RESULT_KEY)) {
            BestResult = PlayerPrefs.GetInt(BEST_RESULT_KEY);
        }
    }

    private void SaveProfile() {
        PlayerPrefs.SetInt(BEST_RESULT_KEY, BestResult);
        PlayerPrefs.Save();
    }
}
