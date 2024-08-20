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

    [Header("Beam Warning")]
    [SerializeField] protected GameObject _warningBeam;
    [SerializeField] float WarningBeamWidth;
    [SerializeField] float WarningBeamLerpFrames = 30f;
    [SerializeField] float WarningBeamDuration = 1f;

    public override void Shoot()
    {
        if (_activeBeams.Count == 0)
        {
            foreach (GameObject node in _nodes)
            {
                GameObject newBeam = Instantiate(_bullet, node.transform.position, node.transform.rotation);
                _activeBeams.Add(newBeam);
                StartCoroutine(WidthLerp(newBeam, 0, BeamWidth, BeamLerpFrames));
                newBeam.transform.parent = this.transform;
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

    public IEnumerator WarningBeams()
    {
        // Warning beam VFX / SFX
        AudioController.controller.PlayLoopSFX(shootSFXObj, transform.position, WarningBeamDuration + BeamDuration);

        // Create warning beams
        List<GameObject> _activeWarningBeams = new List<GameObject>();
        foreach (GameObject node in _nodes)
        {
            GameObject newWarningBeam = Instantiate(_warningBeam, node.transform.position, node.transform.rotation);
            _activeWarningBeams.Add(newWarningBeam);
            StartCoroutine(WidthLerp(newWarningBeam, 0, WarningBeamWidth, WarningBeamLerpFrames));
            newWarningBeam.transform.parent = this.transform.parent;
        }

        // wait until beams have lerped
        for (int i = 0; i < WarningBeamLerpFrames; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        // wait desired amount of time
        yield return new WaitForSeconds(WarningBeamDuration);

        // delete warning beams
        foreach (GameObject beam in _activeWarningBeams)
        {
            Destroy(beam);
        }
        _activeWarningBeams.Clear();
    }

    IEnumerator DeactivateBeam(GameObject beam)
    {
        yield return StartCoroutine(WidthLerp(beam, BeamWidth, 0, BeamLerpFrames));
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
        yield return StartCoroutine(WarningBeams());

        this.Shoot();

        yield return StartCoroutine(DeavtivateAfterDuration());

        yield return new WaitForSeconds(WaitTime);
        _isOnCooldown = false;
    }

    IEnumerator WidthLerp(GameObject beam, float StartWidth, float EndWidth, float Duration)
    {
        Vector3 beamSize = new Vector3(StartWidth, beam.transform.localScale.y, beam.transform.localScale.z);
        beam.transform.localScale = beamSize;

        Vector3 beamGoal = new Vector3(EndWidth, beam.transform.localScale.y, beam.transform.localScale.z);

        int framesElapsed = 0;

        while (beam.transform.localScale.x != EndWidth)
        {
            beamSize = Vector3.Lerp(beam.transform.localScale, beamGoal, framesElapsed / Duration);
            beam.transform.localScale = beamSize;
            yield return new WaitForFixedUpdate();
            framesElapsed++;
        }
    }

    protected override void PlaySFX()
    {
        //TODO Make a better Beam sound to occur while out
        AudioController.controller.PlaySFXWithLock(shootSFXObj, transform.position);
    }
}
