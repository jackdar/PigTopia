using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    [SerializeField] private float fadeSpeed, fadeAmount;
    private float originalOpacity;


    //public Renderer renderer;

    private Material Mat;
    public bool DoFade = false;

    void Start()
    {
        Mat = GetComponent<Material>();
        originalOpacity = Mat.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
