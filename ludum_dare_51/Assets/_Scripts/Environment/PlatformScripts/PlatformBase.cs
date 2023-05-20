using UnityEngine;
using UnityEngine.Serialization;

namespace Special2dPlayerController {
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PlatformBase : MonoBehaviour {

        [Tooltip("If velocity is above this threshold the platform will not affect the player")] 
        [SerializeField] private float unlockThreshold = 2.5f;

        private Rigidbody2D _player;
        protected Rigidbody2D Rb;
        protected Vector2 StartPos;
        protected Vector2 LastPos;

        protected virtual void Awake() {
            Rb = GetComponent<Rigidbody2D>();
            StartPos = Rb.position;
        }

        protected virtual void FixedUpdate() {
            var newPos = Rb.position;
            var change = newPos - LastPos;
            LastPos = newPos;

            MovePlayer(change);
        }

        protected virtual void OnCollisionEnter2D(Collision2D col) {
            if (col.transform.TryGetComponent(out IPlayerController _))
            {
                var normal = col.GetContact(0).normal;
                if (Vector2.Dot(normal, Vector2.down) > 0.5f) // player is on top
                    _player = col.transform.GetComponent<Rigidbody2D>();
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D col) {
            if (col.transform.TryGetComponent(out IPlayerController _))
                _player = null;
        }

        protected virtual void MovePlayer(Vector2 change) {
            if (!_player || _player.velocity.magnitude >= unlockThreshold) return;
            
            _player.MovePosition(_player.position + change);
        }
    }
}