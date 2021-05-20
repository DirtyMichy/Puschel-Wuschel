using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class Theme_Block : MonoBehaviour
{
    public enum blockType
    {
        Default,
        Corner,
        FloatingDefault,
        FloatingCorner,
        None
    };

    public bool spawnGras = true;

    public blockType chosenBlock = blockType.Default;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        //SceneManager.GetActiveScene().name.Contains("Forest");

        string spriteName = SceneManager.GetActiveScene().name + "_" + chosenBlock;

        if (spawnGras && gameObject.transform.childCount == 0)
        {
            GameObject gras = new GameObject();
            gras.transform.parent = gameObject.transform;
            gras.transform.localPosition = new Vector3(0f, 0.6f, 0f);
            gras.name = "Gras";
            gras.AddComponent<SpriteRenderer>();
            gras.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SceneManager.GetActiveScene().name + "/" + SceneManager.GetActiveScene().name + "_Gras");
            Debug.Log("Loading gras: " + SceneManager.GetActiveScene().name + "_Gras");
        }

        if (chosenBlock != blockType.None)
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SceneManager.GetActiveScene().name + "/" + SceneManager.GetActiveScene().name + "_" + chosenBlock);
        /*
        switch (SceneManager.GetActiveScene().name)
        {
            case "Forest":
                Debug.Log("AAAAAAAAAAAAAAAH: " + spriteName);
                break;
            default:
                break;
        }
        */
    }
}