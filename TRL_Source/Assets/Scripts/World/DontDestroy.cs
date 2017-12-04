using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);

        int count = 0;
        foreach (var obj in gameObject.scene.GetRootGameObjects())
            if (obj.name == name)
                count++;
        if (count > 1) Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
