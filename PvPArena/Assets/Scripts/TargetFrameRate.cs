using UnityEngine;

public class TargetFrameRate : MonoBehaviour {
    void Start() => Application.targetFrameRate = 60;
}
