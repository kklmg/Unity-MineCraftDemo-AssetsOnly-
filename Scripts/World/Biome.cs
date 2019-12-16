using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Biome")]
public class Biome : ScriptableObject
{
    public int MaxHeight;
    public int b;
    public List<Vector3> anc; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public sob Clone()
    {
        return null;
    }
}
