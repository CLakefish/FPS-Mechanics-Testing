using System.Collections;
using System.Collections.Generic; using UnityEngine.UI;
using UnityEngine;

public class SliderObject : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] public TMPro.TMP_Text value;
    PlayerCamera player;

    [Header("Components")]
    Slider s;

    void Start()
    {
        s = GetComponent<Slider>();
        player = FindObjectOfType<PlayerCamera>();

        s.value = PlayerPrefs.GetInt("PlayerFOV", 80);
        value.text = s.value.ToString();

        ChangeFOV();
    }

    public void ChangeFOV()
    {
        player.FOV = s.value;
        value.text = s.value.ToString();
        PlayerPrefs.SetInt("PlayerFOV", (int)s.value);
    }
}
