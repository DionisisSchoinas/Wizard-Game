
using System.Collections.Specialized;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 inputRotation;
    private Vector3 mousePlacement;
    private Vector3 screenCentre;
    public Vector3 offset;
    public Camera camera;
    public Transform player;
    // Update is called once per frame
    void Update()
    {

        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, player.position);
        if (p.Raycast(mouseRay, out float hitDist))
        {
            Vector3 hitPoint = mouseRay.GetPoint(hitDist);
            transform.LookAt(hitPoint);
        }
        //FindMousePosition();

        //  transform.rotation = Quaternion.LookRotation(new Vector3(inputRotation.x,0f, inputRotation.z)+offset);
    }
    
    void FindMousePosition()
    {
        screenCentre = new Vector3(Screen.width * 0.5f, 0, Screen.height * 0.5f);
        mousePlacement = Input.mousePosition;
        mousePlacement.z = mousePlacement.y;
        mousePlacement.y = 0;
        inputRotation = mousePlacement - screenCentre;
    }
  
}
