using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float deplacementAMount = 3f;
    public int floaterCount = 1;

    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

        if(transform.position.y <waveHeight)
        {
            float deplacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) /
                depthBeforeSubmerged) * deplacementAMount;

            rigidBody.AddForceAtPosition(new Vector3(0, Mathf.Abs(Physics.gravity.y * deplacementMultiplier), 0),
                transform.position,
                ForceMode.Acceleration);

            rigidBody.AddForce(deplacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(deplacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
