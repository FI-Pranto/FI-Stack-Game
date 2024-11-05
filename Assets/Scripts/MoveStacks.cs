using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStacks : MonoBehaviour
{
    // Start is called before the first frame update
    public bool doIt = false;

    float elapsedtime = 0f;
    float desiredDuration = 2.5f;
    float percentageComplete = 0f;
    [SerializeField] GameObject stack;
    [SerializeField] GameObject dumyStack;
    GameObject newStack;
    [SerializeField] GameObject prevStack;
    Vector3 point1, point3;
    Vector3 point2, point4;

    
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject button;

    int score = 0;

   
    public int i;
   
    Vector3 newScaleOfStack;

    void Start()
    {
        point1 = new Vector3(0, 0.55f, -1.7f);
        point2 = new Vector3(0, 0.55f, 1.7f);
        point3 = new Vector3(-1.7f, 0.55f, 0);
        point4 = new Vector3(1.7f, 0.55f, 0);
        newScaleOfStack=new Vector3(1,0.1f,1);
        i = 0;
       
       
    }
    public void Play()
    {
        button.SetActive(false);
        NewStack();
        
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !button.activeSelf)
        {
            doIt = false;
            elapsedtime = 0f;
            percentageComplete = 0f;
         
          
            //newStack.tag = "Prev";


            if (i % 2 == 0)
            {
                CheckGameOver(newStack.transform.position.z, prevStack.transform.position.z, prevStack.transform.localScale.z);
            }
            else
            {
                CheckGameOver(newStack.transform.position.x, prevStack.transform.position.x, prevStack.transform.localScale.x);
            }
     

        }

        else if (i % 2 == 0 && doIt)
        {

            DoLerp(point1, point2);


        }
        else if (i % 2 != 0 && doIt)
        {
            DoLerp(point3, point4);
        }

    }
    void DoLerp(Vector3 point1, Vector3 point2)
    {
        elapsedtime += Time.deltaTime;
        percentageComplete = Mathf.PingPong(elapsedtime / desiredDuration, 1f);

        newStack.transform.position = Vector3.Lerp(point1, point2, percentageComplete);
    }
    void NewStack()
    {
        elapsedtime= 0f;
        percentageComplete = 0f;
        if (i != 0)
        {
            prevStack = newStack;
        }
        newStack =Instantiate(stack,i%2==0? point1:point3,Quaternion.identity);
        newStack.transform.localScale = newScaleOfStack;
       /* newStack.tag = "Curr";*/
        doIt=true;
    }

    void CheckGameOver(float curr,float prev,float scalePrev)
    {
        float hangOver =prev-curr;
        Debug.Log(hangOver);
        if (Mathf.Abs(hangOver) >= scalePrev)
        {
            GameOver();

        }
        else
        {
            score++;
            scoreText.text = score.ToString();
            SliceObject(hangOver,scalePrev);

        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Game");
    }

 

    private void SliceObject(float hangOver,float scalePrev)
    {
        float direction = hangOver > 0 ? 1 : -1;

        float insidePart = scalePrev - (direction*hangOver);//you can Use Abs() also
        Debug.Log(insidePart);
        float prevCubeEdge;
        float outSide = -hangOver;//you can Use Abs() also
        Vector3 dumyPos;
        GameObject newDumyStack;
        if (i % 2 == 0)
        {
            newStack.transform.localScale = new Vector3(newStack.transform.localScale.x, newStack.transform.localScale.y, insidePart);
            prevCubeEdge = prevStack.transform.position.z -(direction* prevStack.transform.localScale.z / 2);
            
            Debug.Log(prevStack.gameObject.name);//perfectly fine
            Debug.Log("hi"+prevCubeEdge);


            newStack.transform.position =new Vector3(point1.x,newStack.transform.position.y, prevCubeEdge + (direction*newStack.transform.localScale.z / 2));

            point3.z = newStack.transform.position.z;
            point4.z= newStack.transform.position.z;

            dumyPos = new Vector3(newStack.transform.position.x, newStack.transform.position.y, prevCubeEdge + (outSide/2));

            newDumyStack=Instantiate(dumyStack,dumyPos,Quaternion.identity);
            newDumyStack.transform.localScale= new Vector3(newStack.transform.localScale.x, newStack.transform.localScale.y, Mathf.Abs(outSide));
            newDumyStack.AddComponent<Rigidbody>();

            
        }
        else {

            newStack.transform.localScale = new Vector3(insidePart, newStack.transform.localScale.y, newStack.transform.localScale.z);
            prevCubeEdge = prevStack.transform.position.x -(direction* prevStack.transform.localScale.x / 2);
        

            newStack.transform.position = new Vector3(prevCubeEdge + (direction * newStack.transform.localScale.x / 2), newStack.transform.position.y, point3.z);

            point1.x= newStack.transform.position.x;
            point2.x= newStack.transform.position.x;

            dumyPos = new Vector3(prevCubeEdge + (outSide/2), newStack.transform.position.y,newStack.transform.position.z);

            newDumyStack = Instantiate(dumyStack, dumyPos, Quaternion.identity);
            newDumyStack.transform.localScale = new Vector3(Math.Abs(outSide), newStack.transform.localScale.y, newStack.transform.localScale.z);
            newDumyStack.AddComponent<Rigidbody>();

        }
        newScaleOfStack=newStack.transform.localScale;




        Again();


        
    }

    private void Again()
    {
        i++;
        point1.y += 0.1f;
        point2.y += 0.1f;
        point3.y += 0.1f;
        point4.y += 0.1f;

        Invoke("NewStack", 0.3f);
    }
}
