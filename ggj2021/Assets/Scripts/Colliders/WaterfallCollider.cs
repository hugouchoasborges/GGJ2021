using UnityEngine;
using ui;
using util;

public class WaterfallCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameDebug.Log("Player Triggered!", util.LogType.Collider);
            GameController.Instance.cutsceneController.SetWaterfallCutscene(OnCutsceneStarted);
        }
    }

    private void OnCutsceneStarted()
    {
        GetComponent<Collider2D>().enabled = false;
    }

}
