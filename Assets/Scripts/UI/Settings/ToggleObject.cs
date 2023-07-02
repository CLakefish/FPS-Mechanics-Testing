using System.Collections;
using System.Collections.Generic; using UnityEngine.UI;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [Header("Value to find")]
    [SerializeField] string key;
    Toggle t;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Toggle>();
        t.isOn = PlayerPrefs.GetInt(key) == 1;
    }
}
