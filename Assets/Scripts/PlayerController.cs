using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool _isDebugging = false;
    public float _handDistance = 15.0f;
    private float _inputHorizontal;

    void Start()
    {

    }

    void Update()
    {
        // grabs input in update loop for best accuracy
        PlayerInput();
    }

    void PlayerInput()
    {
        if (BodySourceView.bodyTracked)
        {   

            // fetch hand positions
            Vector3 handLeft = BodySourceView.jointObjs[7].position;
            Vector3 handRight = BodySourceView.jointObjs[11].position;

            //calculate hand distance
            _handDistance = Vector3.Distance(handLeft,handRight);

            // calc angle of hands
            float angle = Mathf.Atan2(handRight.y - handLeft.y, handRight.x - handLeft.x) * Mathf.Rad2Deg;

            // convert angle rotation to movement values
            _inputHorizontal = Mathf.Lerp(1.0f, -1.0f, Mathf.InverseLerp(-45.0f, 45.0f, angle));

            //debug
            if(_isDebugging){
                Debug.Log("hand distance" + _handDistance);
                Debug.Log("Angle" + angle);
                Debug.Log("Input Horizontal" + _inputHorizontal);
            }
        }
    }

}