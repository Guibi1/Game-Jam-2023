using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomHitbox : MonoBehaviour
{
    public List<GameObject> aliens = new List<GameObject>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            aliens.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            aliens.Remove(other.gameObject);
        }
    }

    public void DoDamageToEnemies(float damage)
    {
        List<GameObject> delete = new List<GameObject>();
        foreach (GameObject alien in aliens)
        {
            if (alien == null)
            {
                delete.Add(alien);
                continue;
            }

            alien.GetComponent<Alien>().OnHit(damage, gameObject);
        }

        foreach (GameObject alien in delete)
        {
            aliens.Remove(alien);
        }
    }
}
