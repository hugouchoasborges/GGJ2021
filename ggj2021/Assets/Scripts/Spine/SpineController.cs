using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace spine
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class SpineController : MonoBehaviour
    {
        private SkeletonAnimation _skeleton;

        private void Awake()
        {
            _skeleton = GetComponent<SkeletonAnimation>();
        }


        // ========================== Animations ============================


        public void PlayAnimation(string name, bool loop = false)
        {
            GameDebug.Log($"Started Animation: {name}", util.LogType.Spine);

            _skeleton.state.SetAnimation(0, name, loop);
        }


        // ========================== Skins ============================

        public void SetSkin(string name)
        {
            GameDebug.Log($"Skin Selected: {name}", util.LogType.Spine);

            _skeleton.skeleton.SetSkin(name);
        }
    }
}