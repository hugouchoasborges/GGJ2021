using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ui;

public class CliffColliderRight : MonoBehaviour
{

    [SerializeField] CliffColliderLeft leftCollider;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            Debug.Log("Player Triggered!");
            GameController.Instance.cutsceneController.SetCliffCutscene(OnCliffCallback);
        }
    }

    private void OnCliffCallback() {
        GetComponent<Collider2D>().isTrigger = false;
        leftCollider.SetAsTrigger();
    }

}
