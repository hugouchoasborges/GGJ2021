﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ui;
using camera;
using player;
using System;

public class CutsceneController : MonoBehaviour
{
    [System.Serializable]
    public struct CameraConfig {
        public Transform follow;
        public Vector3 offset;
        public float zoom;
        public float zoomVelocity;
        public bool parallax;
        public bool fullLock;
        public bool lockY;
    }

    public CameraConfig cliffConfig;
    public CameraConfig gameConfig;
    public CameraConfig waterfallConfig;

    CameraController cam;
    PlayerController player;

    private void Awake() {
        cam = GameController.Instance.camController;
        player = GameController.Instance.playerController;
    }

    private void SetCameraConfig(CameraConfig config) {
        cam.fullLock = false;
        cam.lockY = config.lockY;

        cam.follow = config.follow;
        cam.initialOffset = config.offset;
        cam.parallaxOn = config.parallax;
        cam.targetZoom = config.zoom;
        cam.zoomVelocity = config.zoomVelocity;
    }


    // ========================== Default game camera config ============================

    public void SetGameCamera(Action callback = null)
    {
        SetCameraConfig(gameConfig);
    }


    // ----------------------------------------------------------------------------------
    // ========================== Cliff Cutscene ============================
    // ----------------------------------------------------------------------------------


    public void SetCliffCutscene(Action callback = null) {
        player.SetMovementLock(true);
        SetCameraConfig(cliffConfig);
        StartCoroutine(CliffRoutine(callback));
    }

    IEnumerator CliffRoutine(Action callback = null) {
        yield return new WaitForSeconds(6f);
        SetGameCamera();
        yield return new WaitForSeconds(3f);
        player.SetMovementLock(false);
        callback?.Invoke();
    }


    // ----------------------------------------------------------------------------------
    // ========================== Waterfall cutscene ============================
    // ----------------------------------------------------------------------------------

    public void SetWaterfallCutscene(Action callback = null)
    {
        player.SetMovementLock(true);
        SetCameraConfig(waterfallConfig);
        StartCoroutine(WaterfallRoutine(callback));
    }

    IEnumerator WaterfallRoutine(Action callback = null)
    {
        yield return new WaitForSeconds(6f);
        SetGameCamera();
        yield return new WaitForSeconds(3f);
        player.SetMovementLock(false);
        callback?.Invoke();
    }

    // ----------------------------------------------------------------------------------
    // ========================== End game cutscene ============================
    // ----------------------------------------------------------------------------------


    public void SetEndGameCutscene(Action callback = null) {
        player.SetMovementLock(true);
        SetCameraConfig(cliffConfig);
        StartCoroutine(EndGameRoutine(callback));
    }

    IEnumerator EndGameRoutine(Action callback = null) {
        yield return new WaitForSeconds(10f);
        callback?.Invoke();
    }
}
