using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ui;

public class CliffColliderLeft : MonoBehaviour
{
    
    public void SetAsTrigger() {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //if (collision.CompareTag("Player")) {
        //    Debug.Log("Player Triggered!");
        //    GameController.Instance.cutsceneController.SetGameCamera();
        //}
    }

}
