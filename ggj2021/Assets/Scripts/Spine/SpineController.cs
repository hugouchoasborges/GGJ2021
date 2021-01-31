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
        private string _currentSkin;

        public Transform[] attachmentPivots;

        private void Awake()
        {
            _skeleton = GetComponent<SkeletonAnimation>();
            _currentSkin = _skeleton.initialSkinName;
        }


        // ========================== Animations ============================

        public void AddAnimation(string name, bool loop = true, float delay = 0, bool useSkinPrefix = false)
        {
            string animationName = name;
            if (useSkinPrefix && !string.IsNullOrEmpty(_currentSkin))
                animationName = $"{_currentSkin}_{animationName}";

            _skeleton.state.AddAnimation(0, animationName, loop, delay);
        }

        public void PlayAnimation(string name, bool loop = true, bool useSkinPrefix = false)
        {
            string animationName = name;
            if (useSkinPrefix && !string.IsNullOrEmpty(_currentSkin))
                animationName = $"{_currentSkin}_{animationName}";

            _skeleton.state.SetAnimation(0, animationName, loop);
        }


        // ========================== Skins ============================

        public void SetSkin(string name)
        {
            //GameDebug.Log($"Skin Selected: {name}", util.LogType.Spine);

            _skeleton.skeleton.SetSkin(name);
            //_skeleton.ClearState();
        }
    }
}