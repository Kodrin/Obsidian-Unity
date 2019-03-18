using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class DataMoshController : MonoBehaviour
{

    public bool _isDebugging = false;
    public Datamosh _dataMosh;

    void Start()
    {

    }

    void Update()
    {  
        //put production code here!

        //debugging
        if(_isDebugging)
            Debugging();
    }

    //debugging
    public void Debugging(){
        //set values 
        if(Input.GetKeyDown("s"))
            MildEntropy();
        if(Input.GetKeyDown("a"))
            StartCoroutine(Artefacting(0.3f));
    }

    //function to trigger mild entropy
    public void MildEntropy(){
        _dataMosh.entropy = 0.85f;
        _dataMosh.noiseContrast = 1.99f;
    }

    //function to reset the data mosh to original settings
    public void BaseLineValues(){
        _dataMosh.entropy = 0.18f;
        _dataMosh.noiseContrast = 1.99f;
        _dataMosh.velocityScale = 0.8f;
        _dataMosh.diffusion = 1.87f;
        _dataMosh.blockSize = 1;
    }

    //temporarily artifact the screen
    public IEnumerator Artefacting(float duration){
        _dataMosh.entropy = 0.8f;
        _dataMosh.diffusion = 0.5f;
        _dataMosh.blockSize = 32;

        yield return new WaitForSeconds(duration);

        //reset to base line values
        BaseLineValues();
    }
}
