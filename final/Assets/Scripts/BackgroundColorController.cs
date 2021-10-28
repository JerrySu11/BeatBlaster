using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorController : MonoBehaviour
{
    public Camera cam;
    [Range(0,2)]
    public int colorVariationChoice;
    public int colorVariationRange;
    public int frameBetweenColorChange = 0;
    private int count = 0;
    private int frameCount = 0;
    private bool direction = true;
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (frameCount == 0)
        {
            colorVariation();
            
        }
        if (frameBetweenColorChange > frameCount)
        {
            frameCount++;
        }
        else
        {
            frameCount = 0;
        }
            
    }
    private void colorVariation()
    {
        Color current = cam.backgroundColor;
        if (direction)
        {
            if (count >= colorVariationRange / 2)
            {
                direction = false;

            }
            else if (colorVariationChoice == 0)
            {
                cam.backgroundColor = new Color(current.r+1f/255f, current.g, current.b, current.a);
                count++;

            }
            else if (colorVariationChoice == 1)
            {
                cam.backgroundColor = new Color(current.r , current.g + 1f / 255f, current.b, current.a);
                count++;

            }
            else if (colorVariationChoice == 2)
            {
                cam.backgroundColor = new Color(current.r, current.g , current.b + 1f / 255f, current.a);
                count++;

            }
        }
        else
        {
            if (count <= 0)
            {
                direction = true;

            }
            else if (colorVariationChoice == 0)
            {
                cam.backgroundColor = new Color(current.r - 1f / 255f, current.g, current.b, current.a);
                count--;

            }
            else if (colorVariationChoice == 1)
            {
                cam.backgroundColor = new Color(current.r, current.g - 1f / 255f, current.b, current.a);
                count--;

            }
            else if (colorVariationChoice == 2)
            {
                cam.backgroundColor = new Color(current.r, current.g, current.b - 1f / 255f, current.a);
                count--;

            }
        }
        
        //cam.backgroundColor;
    }
}
