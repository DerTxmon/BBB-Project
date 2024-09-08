using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fliegen_Behavior : MonoBehaviour
{
    public float speed = 2f;  // Geschwindigkeit der Fliegen
    public float radius = 5f; // Radius, in dem die Fliegen sich bewegen können
    public Transform Fly1, Fly2, Fly3, Fly4, Fly5, Fly6;

    private List<Transform> flies;
    private List<Vector2> targetPositions;

    // Start is called before the first frame update
    void Start()
    {
        flies = new List<Transform> { Fly1, Fly2, Fly3, Fly4, Fly5, Fly6 };
        targetPositions = new List<Vector2>();

        // Setze für jede Fliege eine zufällige Startposition im Radius
        foreach (Transform fly in flies)
        {
            SetRandomTargetPosition(fly);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Aktualisiere jede Fliege
        for (int i = 0; i < flies.Count; i++)
        {
            MoveFly(flies[i], i);
        }
    }

    void MoveFly(Transform fly, int index)
    {
        // Berechne die Zielposition unter Beibehaltung der z-Koordinate
        Vector3 targetPosition = new Vector3(targetPositions[index].x, targetPositions[index].y, fly.position.z);

        // Bewege die Fliege in Richtung ihres Zielpunktes
        fly.position = Vector3.MoveTowards(fly.position, targetPosition, speed * Time.deltaTime);

        // Drehe die Fliege in die Richtung, in die sie fliegt
        Vector2 direction = targetPositions[index] - (Vector2)fly.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fly.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90f));

        // Wenn die Fliege ihr Ziel erreicht hat, setze eine neue zufällige Zielposition
        if (Vector2.Distance(fly.position, targetPositions[index]) < 0.1f)
        {
            SetRandomTargetPosition(fly, index);
        }
    }

    void SetRandomTargetPosition(Transform fly, int index = -1)
    {
        // Generiere eine zufällige Zielposition innerhalb des gegebenen Radius
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * radius;

        // Aktualisiere die Zielposition der Fliege
        if (index == -1)
        {
            targetPositions.Add(randomPosition);
        }
        else
        {
            targetPositions[index] = randomPosition;
        }
    }
}
