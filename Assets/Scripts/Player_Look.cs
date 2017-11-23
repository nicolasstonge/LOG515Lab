using UnityEngine;
using System.Collections;

[AddComponentMenu("Hard Shell Studios/Examples/Player Look")]
public class Player_Look : MonoBehaviour
{

    public bool inverted;
    public float speedX;
    public float speedY;
    float xrot;
    float yrot;
	
	// Update is called once per frame
	void Update () 
	{
        float lookAxisY = hardInput.GetAxis("MouseY") + hardInput.GetAxis("ControllerRightUp", "ControllerRightDown");
        float lookAxisX = hardInput.GetAxis("MouseX") + hardInput.GetAxis("ControllerRightLeft", "ControllerRightRight");

        transform.parent.eulerAngles += (new Vector3(0, lookAxisX, 0) * speedX);
        xrot = transform.eulerAngles.y;

        if (inverted)
            yrot = Mathf.Clamp(yrot + lookAxisY * speedY, -80, 60);
        else
            yrot = Mathf.Clamp(yrot + -lookAxisY * speedY, -80, 60);


        transform.rotation = Quaternion.Euler(yrot, xrot, 0);
    }
}
