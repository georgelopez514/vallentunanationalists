using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public PlayerMovementScript playerMovementScript;
    public string[] tags;
    public string currentTag; 

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
        Debug.Log("[Interaction] interacted");

        bool matched = false;
        foreach (string taga in tags)
        {
            Debug.Log("[Interaction] looking for match");

            if (playerMovementScript.hit.collider.CompareTag(taga))
            {
                Debug.Log("[Interaction] found matching tag: " + taga);
                currentTag = taga; 
                EventCaller();
                matched = true;
                break;
            }
        }

        if (!matched)
            Debug.LogWarning("[Interaction] no matching tag found");
    }

    public string EventCaller()
    {
        if (currentTag == null) 
        {
            Debug.LogError("[Interaction] failed to find collision");
            return "index error";
        }

        if (currentTag == "npc" && Keyboard.current.eKey.isPressed) 
        {
            Debug.Log("[Interaction] Found an NPC, interacting");
            return "npc";
        }

        Debug.Log($"Interacted with tag: {currentTag}"); 
        return null; 
    }
}