using UnityEngine;

public class Mate_Nametag : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Speichere die Anfangsrotation des Child-Objekts
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Setze die Rotation des Child-Objekts zurück
        transform.rotation = initialRotation;
    }
}

