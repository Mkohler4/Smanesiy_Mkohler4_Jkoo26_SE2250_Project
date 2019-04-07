using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Insepctor")]
    //This is an unusual but handy use of Vecotr2s. x holds a min value
    // and y is a max value for a Random.Range() that will be called later
    public Vector2 rotationMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f; // the amount of time the PowerUp exists for
    public float fadeTime = 4f; // the amount of seconds the PowerUp fades

    [Header("Set Dynamically")]
    public WeaponType type; //The type of the PowerUp
    public GameObject cube; //Reference to the Cube child of the PowerUp GameObject
    public TextMesh letter; //Reference to the TextMesh
    public Vector3 rotationsPerSecond;  //Euler rotation speed
    public float birthTime; //Spawn time for PowerUp

    private Rigidbody _rigid;
    private BoundsCheck _boundsCheck;
    private Renderer _cuberRender;

    void Awake()
    {
        //Find the Cube Reference
        cube = transform.Find("Cube").gameObject;
        //Find the TextMesh and other components
        letter = GetComponent<TextMesh>();
        _rigid = GetComponent<Rigidbody>();
        _boundsCheck = GetComponent<BoundsCheck>();
        _cuberRender = cube.GetComponent<Renderer>();

        // Set a random velocity
        Vector3 velocity = Random.onUnitSphere; // Get Random XYZ velocity
        //Random.onUnitSphere gives you a vector point that s somewhere on
        //the surface of the sphere with a raidus of m around the origin
        velocity.z = 0; //Restrict the velocity to only apply on the XY plane
        velocity.Normalize();   //Normalizing a Vector3 object makes it length 1m
        velocity *= Random.Range(driftMinMax.x, driftMinMax.y);
        _rigid.velocity = velocity;

        // Set the rotation of this GameObject to r:[0,0,0]
        transform.rotation = Quaternion.identity;
        //Quaternion.identity is equal to no rotation.

        // Set yo the rotationsPerSecond for the Cube child using rotationMinMAx x and y
        rotationsPerSecond = new Vector3(Random.Range(rotationMinMax.x, rotationMinMax.y),
            Random.Range(rotationMinMax.x, rotationMinMax.y),
            Random.Range(rotationMinMax.x, rotationMinMax.y));
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotationsPerSecond * Time.time);

        // Fade out the PowerUp over time
        //Given the default values, a PowerUp will exist for 10 seconds
        //and the fade out over 4 seconds.
        float fadeOut = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //For lifeTime seconds, fadeOut will be <= 0. Then it will transition to 1 over the course of fadeTime seconds

        //If fadeOut >= 1, destroy thisPowerUp
        if(fadeOut >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //Use fadeOut to determine the alpha value of the Cube and Letter
        if(fadeOut > 0)
        {
            Color clr = _cuberRender.material.color;
            clr.a = 1f - fadeOut;
            _cuberRender.material.color = clr;
            //Fade the Letter too, just not as much
            clr = letter.color;
            clr.a = 1f - (fadeOut * 0.5f);
            letter.color = clr;
        }

        if (!_boundsCheck.isOnScreen)
        {
            //If the PowerUp has drifted entirely off screen, destroy it
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType weaponType)
    {
        //Grab the WeaponDefinition from Main
        WeaponDefinition definition = Main.GetWeaponDefinition(weaponType);
        // Set the color of the Cube child
        _cuberRender.material.color = definition.color;
        letter.text = definition.letter; //Set the letter that is shown
        type = weaponType;  // Finally actually set the type

    }
    public void AbsorbedBy(GameObject target)
    {
        //This function is called by the Hero class when a PowerUp is collected
        //We could take the target and shrink the size but for now we will destory this.gameObject
        Destroy(this.gameObject);

    }
}
