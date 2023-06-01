using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject damagePopupPrefab;

    public void DisplayDamage(string amount, Vector3 parent, Color c)
    {

        GameObject obj = Instantiate(damagePopupPrefab, parent + new Vector3(0f, 1f, 0f), Quaternion.identity);
        obj.GetComponent<DamageVisual>().visual(amount, c);
    }

}
