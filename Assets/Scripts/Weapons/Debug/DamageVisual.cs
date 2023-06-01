using System.Collections;
using System.Collections.Generic; using TMPro;
using UnityEngine;

public class DamageVisual : MonoBehaviour
{
    TMP_Text textMesh;
    [SerializeField] Color textColor;
    [SerializeField] Color textFadeColor;

    [SerializeField] float fadeOutSpeed;
    [SerializeField] float moveYSpeed,
                           dissapearSpeed;

    public void visual(string damage, Color orange)
    {

        textColor = orange;
        textMesh = GetComponentInChildren<TMP_Text>();
        textMesh.SetText(damage.ToString());
    }

    private void LateUpdate()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        transform.position += new Vector3(0f, moveYSpeed * Time.deltaTime, 0f);
        dissapearSpeed -= Time.deltaTime;

        textMesh.color = textColor;

        if (dissapearSpeed <= 0f)
        {
            textFadeColor.a -= fadeOutSpeed * Time.deltaTime;

            textMesh.color = textFadeColor;

            if (textFadeColor.a <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
