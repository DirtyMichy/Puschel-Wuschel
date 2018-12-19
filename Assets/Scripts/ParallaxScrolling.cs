using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public float speed = 100f;
    public float offsetY = -3f;

    private void Start()
    {
        //Reset Offset
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0f, transform.position.y*-1f));
    }
    
    void Update()
    {
        //Create the offset
        Vector2 offset = new Vector2(Camera.main.transform.position.x/ speed, offsetY);

        //Apply the offset to the material
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}

