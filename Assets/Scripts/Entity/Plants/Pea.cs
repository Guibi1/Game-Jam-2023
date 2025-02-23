using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using FMODUnity;

public class Pea : Plant
{
    public Alien aimedAlien;
    public GameObject Turret;
    public GameObject PeaBall;
    public Transform aimPoint;
    public float shootspeed;
    public Animator customAnimator;
    public StudioEventEmitter emitterAttack;
    public bool placed;
    public override IEnumerator Execute()
    {
        if (aimedAlien == null) yield break;
        if (!placed) yield break;
        
        GameObject peaBall = LeanPool.Spawn(PeaBall);
        peaBall.transform.position = aimPoint.transform.position;
        peaBall.GetComponent<Rigidbody>().velocity = aimPoint.forward * shootspeed;

        emitterAttack.Play();
        customAnimator.Play("Pea");

        DeleteAfterTime(peaBall);
    }

    private IEnumerator DeleteAfterTime(GameObject peaBall)
    {
        yield return new WaitForSeconds(plantData.executeTime - 0.1f); 
        if (peaBall.activeSelf && peaBall.activeInHierarchy)
        {
            LeanPool.Despawn(peaBall);
        }

    }

    public override IEnumerator Preparing()
    {
        customAnimator.Play("Idle");
        yield return null;
    }

    private void Update()
    {
        if (aimedAlien == null)
            return;
        
        Turret.transform.LookAt(aimedAlien.transform.position);

        if (aimedAlien.dying)
        {
            aimedAlien = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (aimedAlien != null)
            return;
        
        if (other.CompareTag("Alien"))
        {
            aimedAlien = other.GetComponent<Alien>();

            if (aimedAlien == null)
            {
                other.GetComponentInParent<Alien>();
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (aimedAlien != null)
            return;
        
        if (other.CompareTag("CapsuleAlien"))
        {
            aimedAlien = other.GetComponent<Alien>();

            if (aimedAlien == null)
            {
                other.GetComponentInParent<Alien>();
            }
        }
    }
}
