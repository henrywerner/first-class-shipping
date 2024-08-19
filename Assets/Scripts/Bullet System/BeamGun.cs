using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamGun : Gun
{
    [SerializeField] float BeamDuration;
    [SerializeField] float BeamCoolDown;
    [SerializeField] float BeamWidth;
    [SerializeField] float BeamLerpFrames = 60f;
    private List<GameObject> _activeBeams = new List<GameObject> ();

    public override void Shoot()
    {
        if (_activeBeams.Count == 0)
        {
            foreach (GameObject node in _nodes)
            {
                GameObject newBeam = Instantiate(_bullet, node.transform.position, node.transform.rotation);
                _activeBeams.Add(newBeam);
                StartCoroutine(WidthLerp(newBeam, 0, BeamWidth));
                newBeam.transform.parent = this.transform.parent;
            }
        }
    }

    public override void Fire()
    {
        ShootWithCooldown(BeamCoolDown);
    }

    public override void ShootWithCooldown(float WaitTime)
    {
        if (!_isOnCooldown)
        {
            StartCoroutine(ShootThenWait(WaitTime));
            _isOnCooldown = true;
        }
    }

    public void DeactivateAllBeams()
    {
        foreach (GameObject beam in _activeBeams)
        {
            StartCoroutine(DeactivateBeam(beam));
        }
    }

    IEnumerator DeactivateBeam(GameObject beam)
    {
        yield return StartCoroutine(WidthLerp(beam, BeamWidth, 0));
        _activeBeams.Remove(beam);
        Destroy(beam);
    }

    public IEnumerator DeavtivateAfterDuration()
    {
        yield return new WaitForSeconds(BeamDuration);

        DeactivateAllBeams();
    }

    public IEnumerator ShootThenWait(float WaitTime)
    {
        this.Shoot();

        yield return StartCoroutine(DeavtivateAfterDuration());

        yield return new WaitForSeconds(WaitTime);
        _isOnCooldown = false;
    }

    IEnumerator WidthLerp(GameObject beam, float StartWidth, float EndWidth)
    {
        Vector3 beamSize = new Vector3(StartWidth, beam.transform.localScale.y, beam.transform.localScale.z);
        beam.transform.localScale = beamSize;

        Vector3 beamGoal = new Vector3(EndWidth, beam.transform.localScale.y, beam.transform.localScale.z);

        int framesElapsed = 0;

        while (beam.transform.localScale.x != EndWidth)
        {
            beamSize = Vector3.Lerp(beam.transform.localScale, beamGoal, framesElapsed / BeamLerpFrames);
            beam.transform.localScale = beamSize;
            yield return new WaitForFixedUpdate();
            framesElapsed++;
        }
    }
}
