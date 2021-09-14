using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterSpwaner : MonoBehaviour
{

    public GameObject speedBooster;
    public float spwanTime = 10.0f;
    public Vector2 bounds;

    // Start is called before the first frame update
    void Start()
    {
        bounds = GameObject.Find("Main Character").transform.position;
        Debug.Log("Game dimulai: " + Time.unscaledTime);
    }

    private void spawnBooster()
    {
        GameObject ins = Instantiate(speedBooster) as GameObject;
        ins.transform.position = new  Vector2(bounds.x + 7, -3);
    }

    IEnumerator spawnn()
    {
        while(true){
            Debug.Log("booster spawn");
            yield return new WaitForSeconds(spwanTime /*+ (spwanTime * Time.unscaledTime)*/);
            spawnBooster();
        }
    }
}
