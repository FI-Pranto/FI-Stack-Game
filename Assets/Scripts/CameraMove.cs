using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MoveStacks ms;

    bool nowTime=false;

    float elapsedtime = 0f;
    float desiredDuration = 2f;
    float percentageComplete = 0f;

    Vector3 currPos;
    Vector3 targetPos;

    // Update is called once per frame

    void Update()
    {
        if (ms.i != 0 && ms.i % 3 == 0)
        {
          
            currPos=transform.position;
            targetPos=new Vector3(transform.position.x,transform.position.y+0.3f,transform.position.z);
            nowTime = true;
        }
        else if (nowTime)
        {
            elapsedtime += Time.deltaTime;

            percentageComplete =elapsedtime/desiredDuration;

            transform.position = Vector3.Lerp(currPos, targetPos, percentageComplete);
            if (percentageComplete >= 1f)
            {
                elapsedtime = 0f;
                percentageComplete = 0f;
                nowTime = false;

            }
        }
        
    }
}
