using spine;
using System;
using System.Net;
using System.Collections;
using UnityEngine;

namespace monster
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MonsterController : MonoBehaviour
    {
        private readonly string[] _phases = { "phase1", "phase2", "phase3", "phase4" };
        private readonly Vector2[] _colliderOffsets =
        {
            new Vector2(0.035f, 1.815f),
            new Vector2(0.035f, 1.163f),
            new Vector2(0.035f, 1.30f),
            new Vector2(-0.007f, 0.24f),
        };
        private readonly Vector2[] _colliderSizes =
        {
            new Vector2(4.94f, 3.12f),
            new Vector2(4.94f, 3.12f),
            new Vector2(3.28f, 2.08f),
            new Vector2(0.71f, 0.34f),
        };

        [Header("Heart Spawn")]
        [SerializeField] Vector3 heartSpawnOffset;
        [SerializeField] GameObject heartPiecePrefab;

        private int _currentPhaseId = 0;

        [Header("Collision Detection")]
        [SerializeField] private bool playerCollisionEnabled = false;
        [SerializeField] private LayerMask _whatIsPlayer;
        private int _whatIsPlayerNumber;
        private BoxCollider2D _boxCollider2D;

        [Header("Components")]
        [SerializeField] private SpineController _spineController;

        private void Awake()
        {
            _whatIsPlayerNumber = (int)Mathf.Log(_whatIsPlayer.value, 2);
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }


        // ========================== INIT ============================


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            ConfigCurrentCollider();
            RunIdleAnimation();
        }


        // ========================== Collision Detection ============================

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!playerCollisionEnabled)
                return;

            if (collision.gameObject.layer == _whatIsPlayerNumber)
            {
                GoToNextPhase();
                ConfigCurrentCollider();
            }
        }

        private void ConfigCurrentCollider()
        {
            _boxCollider2D.offset = _colliderOffsets[_currentPhaseId];
            _boxCollider2D.size = _colliderSizes[_currentPhaseId];
        }


        // ========================== Animation transitions ============================

        private void GoToNextPhase()
        {
            if (_currentPhaseId >= _phases.Length - 1)
                return;

            RunTransitionAnimation();
            _currentPhaseId++;
            AddIdleAnimation();

            if(_currentPhaseId == 2)
            {
                StartCoroutine(SpawnPiece_Routine());
            }
        }

        private void RunTransitionAnimation()
        {
            _spineController.PlayAnimation(_phases[_currentPhaseId] + "_gets_smaller");
        }

        private void RunIdleAnimation()
        {
            _spineController.PlayAnimation(_phases[_currentPhaseId] + "_idle");
        }

        private void AddIdleAnimation()
        {
            _spineController.AddAnimation(_phases[_currentPhaseId] + "_idle");
        }

        IEnumerator SpawnPiece_Routine()
        {
            yield return new WaitForSeconds(1f);
            var pos = transform.position + heartSpawnOffset;
            var piece = Instantiate(heartPiecePrefab, pos, Quaternion.identity).GetComponent<HeartPiece>();
            yield return piece.Appear(0);
        }

    }
}