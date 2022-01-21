using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpaceController : MonoBehaviour
{
    public Material emptyMaterial;
    public Material destinationMaterial;
    public Material fullMaterial;
    public GameObject particles;

    GridValue gridValue;

    public void SetValue(GridValue value)
    {
        gridValue = value;
        switch (value)
        {
            case GridValue.Space:
                GetComponent<MeshRenderer>().material = emptyMaterial;
                break;
            case GridValue.Destination:
                GetComponent<MeshRenderer>().material = destinationMaterial;
                break;
            default:
                GetComponent<MeshRenderer>().material = fullMaterial;
                break;
        }
    }

    public GridValue GetValue()
    {
        return gridValue;
    }

    public void StartParticles()
    {
        particles.SetActive(true);
    }

    public void StopParticles()
    {
        particles.SetActive(false);
    }
}
