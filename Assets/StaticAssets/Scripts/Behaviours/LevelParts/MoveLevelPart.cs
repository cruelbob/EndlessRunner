using UnityEngine;
using System.Collections;

public class MoveLevelPart : MonoBehaviour {
    public GameObject Left;
    public GameObject Center;
    public GameObject Right;

    private void Awake() {
        float value = Random.value;
        if(value < 0.33f) {
            Left.SetActive(true);
            Center.SetActive(false);
            Right.SetActive(false);
        } else if(value < 0.66f) {
            Left.SetActive(false);
            Center.SetActive(true);
            Right.SetActive(false);
        } else {
            Left.SetActive(false);
            Center.SetActive(false);
            Right.SetActive(true);
        }
    }
}
