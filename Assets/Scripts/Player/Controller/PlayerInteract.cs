using System.Collections;
using System.Collections.Generic; using CUtilities.Object;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction Distance")]
    [SerializeField] float interactableDistance;

    [Header("Display")]
    [SerializeField] internal GameObject interactionPrompt;
    [SerializeField] internal TMPro.TMP_Text descriptionText,
                                             nameText;
    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(player.rb.transform.position, player.GetViewDirection());

        bool hasHit = false;

        if (Physics.Raycast(ray, out RaycastHit obj, interactableDistance))
        {
            if (obj.collider.TryGetComponent<Interactable>(out Interactable i))
            {
                InteractionHandle(i);
                descriptionText.text = i.GetDescription() == "" ? "Missing!" : i.GetDescription();
                nameText.text = i.GetName() == "" ? "Missing!" : i.GetName();

                interactionPrompt.SetActive(true);
                interactionPrompt.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
                hasHit = true;
            }
        }

        if (!hasHit)
        {
            interactionPrompt.SetActive(false);
            descriptionText.text = nameText.text = "";
        }
    }

    void InteractionHandle(Interactable interactible)
    {
        KeyCode key = KeyCode.E;

        switch (interactible.interactionType)
        {
            case (InteractionType.Click):
                if (Input.GetKeyDown(key)) interactible.Interact();
                break;

            case (InteractionType.Hold):

                break;
        }
    }
}
