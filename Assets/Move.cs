using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vuforia;

public class Move : MonoBehaviour
{
    public GameObject model;
    public ObserverBehaviour[] ImageTargets;
    public int currentTarget = 0;
    public float speed = 1.0f;
    private bool isMoving = false;

    public void moveToNextMarker()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveModel());
        }
    }

    private IEnumerator MoveModel()
    {
        isMoving = true;
        ObserverBehaviour target = GetNextDetectedTarget();

        if (target == null)
        {
            Debug.LogWarning("No se encontró el siguiente marcador detectado.");
            isMoving = false;
            yield break;
        }

        Vector3 startPosition = model.transform.position;
        Vector3 endPosition = target.transform.position;

        float journey = 0;

        while (journey < 1f)
        {
            journey += Time.deltaTime * speed;
            model.transform.position = Vector3.Lerp(startPosition, endPosition, journey);
            yield return null;
        }

        // Mover al siguiente índice en la lista
        currentTarget = (currentTarget + 1) % ImageTargets.Length;
        isMoving = false;
    }

    private ObserverBehaviour GetNextDetectedTarget()
    {
        // Buscar el siguiente marcador en orden
        for (int i = 0; i < ImageTargets.Length; i++)
        {
            int index = (currentTarget + i) % ImageTargets.Length;
            ObserverBehaviour target = ImageTargets[index];

            if (target != null && (target.TargetStatus.Status == Status.TRACKED || target.TargetStatus.Status == Status.EXTENDED_TRACKED))
            {
                currentTarget = index;  // Actualiza al índice correcto
                return target;
            }
        }

        return null;
    }
}
