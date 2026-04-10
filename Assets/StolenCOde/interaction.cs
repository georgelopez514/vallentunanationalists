using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] PlayerMovementScript playerMovementScript;
    public string[] tags;

    void Update()
    {
        if (playerMovementScript.hit.collider != null)
        {
            Debug.Log("[Interaction] found something");
            ColliderDetector();
        }
    }

    void ColliderDetector()
    {
        if (!Keyboard.current.eKey.isPressed) return;

        Debug.Log("[Interaction] interacted");

        foreach (string tag in tags)
        {
            Debug.Log("[Interaction] looking for match");

            if (playerMovementScript.hit.collider.CompareTag(tag))
            {
                Debug.Log("[Interaction] found matching tag: " + tag);
                EventCaller(tag);
                break;
            }
        }

        Debug.LogWarning("[Interaction] no matching tag found");
    }

    public string EventCaller(string tag)
    {
        if (tag == null)
        {
            Debug.LogError("[Interaction] failed to find collision");
            return "index error";
        }

        if (tag == "npc")
        {
            Debug.Log("[Interaction] Found an NPC, interacting");
            return "npc";
        }

        Debug.Log($"Interacted with tag: {tag}");
        return tag;
    }
}