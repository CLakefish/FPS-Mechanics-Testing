using System.Collections;
using System.Collections.Generic; using TMPro; using UnityEngine.UI;
using UnityEngine;

public class LevelSelectEffects : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject visual;
    [SerializeField] TMP_Text timeText;

    [Header("Toggles")]
    [SerializeField] List<Toggle> toggles = new List<Toggle>();
    bool isOpen = true;

    [Header("Opening Menu")]
    [SerializeField] float smoothingTime;
    [SerializeField] bool floating = false;
    Vector3 startPos = new(), endPos = new();

    // Start is called before the first frame update
    void Start()
    {
        toggleMenu();

        startPos = visual.transform.localPosition;
        endPos = new Vector3(visual.transform.localPosition.x, visual.transform.localPosition.y, visual.transform.localPosition.z - 0.32f);

        if (timeText.text == "BEST TIME : 00 : 00 : 00") timeText.color = Color.black;
    }


    private void Update()
    {
        foreach (Toggle t in toggles) toggleColor(t);

        visual.transform.localPosition = Vector3.Lerp((!isOpen) ? endPos : startPos, (!isOpen) ? endPos : startPos, 30);
    }

    public void toggleColor(Toggle t)
    {
        switch (t.isOn)
        {
            case true:

                t.GetComponentInChildren<TMP_Text>().color = Color.white;

                break;

            case false:

                t.GetComponentInChildren<TMP_Text>().color = Color.black;

                break;
        }
    }

    public void toggleMenu()
    {
        isOpen = !isOpen;

        menu.SetActive(isOpen);
    }
}
