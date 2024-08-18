using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMoveAlongPath : MonoBehaviour
{
    public static BezierMoveAlongPath instance;

    void Awake()
    {
        // Make this a Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public void StartMoving(Transform[] paths, float speed, GameObject target) {
        StartCoroutine(MoveAlongAllPaths(paths, speed, target));
    }

    public void MoveAlongSinglePath(Transform path, float speed, GameObject target) {
        StartCoroutine(MoveAlongPath(path, speed, target));
    }

    public IEnumerator MoveAlongAllPaths(Transform[] paths, float speed, GameObject target) {
        foreach (Transform path in paths)
        {
            yield return StartCoroutine(MoveAlongPath(path, speed, target));
        }
    }

    public IEnumerator MoveAlongPath(Transform path, float speed, GameObject target) {
        Vector2 p0 = path.GetChild(0).position;
        Vector2 p1 = path.GetChild(1).position;
        Vector2 p2 = path.GetChild(2).position;
        Vector2 p3 = path.GetChild(3).position;

        float t = 0f;
        Vector2 newPos;

        while (t < 1)
        {
            t += Time.deltaTime * speed;

            newPos = Mathf.Pow(1 - t, 3) * p0 +
            3 * Mathf.Pow(1 - t, 2) * t * p1 +
            3 * (1 - t) * Mathf.Pow(t, 2) * p2 + 
            Mathf.Pow(t, 3) * p3;

            target.transform.position = newPos;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    

}
