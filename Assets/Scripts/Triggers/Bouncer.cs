using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private float _bounceForce;

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.TryGetComponent<Player>(out Player player))
        //    player.AddForce(Vector3.up * _bounceForce);
        
    }
}
