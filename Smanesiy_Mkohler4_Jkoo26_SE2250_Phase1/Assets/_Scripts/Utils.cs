using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //Returns a list of all materials on this gameobject and its children
    static public Material[] GetAllMaterials(GameObject gameoObj)
    {
        //Returns an array of the component type
        Renderer[] rends = gameoObj.GetComponentsInChildren<Renderer>();

        //Extracts material field for each component in the rend array and adds it to the mats list
        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }
        //Return the mats array
        return (mats.ToArray());
    }
}
