using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHUDBehaviour : MonoBehaviour {
    public Text BestResult;
    public Text CurrentResult;
    public Text NewResult;
    public GameObject ResultsPanel;
    public GameObject NewResultPanel;
    public GameObject StartPanel;

    private void Awake() {
        MainController.Instance.LevelController.OnNewLevel += OnNewLevel;
        MainController.Instance.LevelController.OnNewResult += OnNewResult;
        BestResult.text = MainController.Instance.ProfileController.BestResult.ToString();
    }

    private void OnNewResult(int newResult) {
        ResultsPanel.gameObject.SetActive(false);
        NewResultPanel.gameObject.SetActive(true);
        StartPanel.gameObject.SetActive(true);
        NewResult.text = newResult.ToString();
    }

    private void OnNewLevel(LevelBehaviour newLevel) {
        BestResult.text = MainController.Instance.ProfileController.BestResult.ToString();
        CurrentResult.text = 0.ToString();
        ResultsPanel.gameObject.SetActive(true);
        StartPanel.gameObject.SetActive(false);
        NewResultPanel.SetActive(false);
        newLevel.OnPointsChanged += OnPointsChanged;
    }

    private void OnPointsChanged(int points) {
        CurrentResult.text = points.ToString();
    }
}
