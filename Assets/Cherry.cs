using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;
using FMODUnity;

public class Cherry : Plant
{
    public GameObject Turret;
    public Alien aimedAlien;
    public AnimationCurve curveCrush;
    public GameObject axis;
    public GameObject CherryPrefab;
    public GameObject spawnpoint;
    public bool placed;
    [SerializeField] float bullet_speed;
    public StudioEventEmitter emitterAttack;
    public StudioEventEmitter emitterImpact;
    public override IEnumerator Execute()
    {
        spawnpoint.SetActive(true);

        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            if (aimedAlien == null) return;
            if (!placed) return;

            axis.transform.localRotation = Quaternion.Euler(new Vector3(axis.transform.localEulerAngles.x, axis.transform.localEulerAngles.y, -f));
        }, 0, 50, plantData.executeTime, curveCrush));
        
        StartCoroutine(SimpleRoutines.WaitTime(plantData.executeTime, () =>
        {
            if (aimedAlien == null) return;
            if (!placed) return;
            spawnpoint.SetActive(false);
            GameObject cherry = LeanPool.Spawn(CherryPrefab);
            cherry.transform.position = spawnpoint.transform.position;
            cherry.transform.rotation = spawnpoint.transform.rotation;
            Vector3 vec = new Vector3(cherry.transform.right.x * bullet_speed, 0, cherry.transform.right.z * bullet_speed);
            // Physics moment

            // DeltaT = DeltaX / v
            float deltaT = Vector3.Distance(cherry.transform.position, aimedAlien.transform.position) / vec.magnitude;

            // Thought process behind formula
            // DeltaY = vi * DeltaT + 1/2a * DeltaT2 <== physics formula
            // DeltaY - vi = DeltaT + 1/2a * DeltaT2
            // -vi = DeltaT + 1/2a * DeltaT2 - DeltaY
            // vi = -1 * (DeltaT + 1/2a * DeltaT2 - DeltaY)
            // DeltaY = 0
            // vi = -1* (DeltaT - 4.9 * DeltaT * DeltaT)

            float vi = -1 * (deltaT + (-4.9f) * (deltaT * deltaT));
            vec = new Vector3(vec.x, vi, vec.z);
            cherry.GetComponent<Rigidbody>().velocity = vec;


        }));

        emitterAttack.Play();
        yield return null;
    }

    public override IEnumerator Preparing()
    {
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            if (aimedAlien == null) return;
            if (!placed) return;

            axis.transform.localRotation = Quaternion.Euler(new Vector3(axis.transform.localEulerAngles.x, axis.transform.localEulerAngles.y, -f));
        }, 50, 0, plantData.executeTime, curveCrush));
        yield return null;

    }

    private void Update()
    {
        if (aimedAlien == null)
            return;

        Turret.transform.LookAt(aimedAlien.transform.position);
        Turret.transform.eulerAngles = new Vector3(Turret.transform.eulerAngles.x, Turret.transform.eulerAngles.y - 90, Turret.transform.eulerAngles.x);
    }
    private void OnTriggerStay(Collider other)
    {
        if (aimedAlien != null)
            return;

        if (other.CompareTag("Alien"))
        {
            aimedAlien = other.GetComponent<Alien>();

        }
    }


}
