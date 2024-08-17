using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMoveAlongPath : MonoBehaviour
{
    [SerializeField] private Transform[] _paths;
    [SerializeField] private float _speedModifier = 0.5f;


    public void StartMoving() {
        StartCoroutine(MoveAlongAllPaths());
    }

    private IEnumerator MoveAlongAllPaths() {
        for (int i = 0; i < _paths.Length; i++)
        {
            yield return StartCoroutine(MoveAlongPath(i));
        }
    }

    private IEnumerator MoveAlongPath(int pathID) {
        Vector2 p0 = _paths[pathID].GetChild(0).position;
        Vector2 p1 = _paths[pathID].GetChild(1).position;
        Vector2 p2 = _paths[pathID].GetChild(2).position;
        Vector2 p3 = _paths[pathID].GetChild(3).position;

        float t = 0f;
        Vector2 newPos;

        while (t < 1)
        {
            t += Time.deltaTime * _speedModifier;

            newPos = Mathf.Pow(1 - t, 3) * p0 +
            3 * Mathf.Pow(1 - t, 2) * t * p1 +
            3 * (1 - t) * Mathf.Pow(t, 2) * p2 + 
            Mathf.Pow(t, 3) * p3;

            transform.position = newPos;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    

}
