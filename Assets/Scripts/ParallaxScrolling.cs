using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public float speed = 100f;

    private void Start()
    {
        //Reset Offset
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0f, transform.position.y*-1f));
    }
    
    void Update()
    {
        //Keep looping between 0 and 1
        //float x = Mathf.Repeat(Time.time * Camera.main.transform.position.x, 1);

        //Create the offset
        Vector2 offset = new Vector2(Camera.main.transform.position.x/ speed, 1f);

        //Apply the offset to the material
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}

