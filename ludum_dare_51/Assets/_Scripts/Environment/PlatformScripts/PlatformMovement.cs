using System.Collections;
using System.Collections.Generic;
using Special2dPlayerController;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class PlatformMovement : MonoBehaviour
{
    
    public float _speed = 1.5f;
    public bool isRadial = false;
    public float radius = 5f;
    
    [SerializeField] private bool _looped;
    [SerializeField] private bool _ascending;
    public List<Transform> patrolPts;
    private List<Vector2> _points;
    
    [Tooltip("If velocity is above this threshold the platform will not affect the player")] 
    [SerializeField] private float _unlockThreshold = 2.5f;

    private Rigidbody2D _player;
    protected Vector2 _startPos;
    protected Vector2 _lastPos;

    protected void Awake() {
        _startPos = transform.position;
        _points = new List<Vector2>();
        foreach (var pt in patrolPts)
        {
            _points.Add(pt.position);
        }
    }
    

    protected  void OnCollisionEnter2D(Collision2D col) {
        if (col.transform.TryGetComponent(out IPlayerController _))
        {
            var normal = col.GetContact(0).normal;
            if (Vector2.Dot(normal, Vector2.down) > 0.5f) // player is on top
                _player = col.transform.GetComponent<Rigidbody2D>();
        }
    }

    protected  void OnCollisionExit2D(Collision2D col) {
        if (col.transform.TryGetComponent(out IPlayerController _))
            _player = null;
    }

    protected  void MovePlayer(Vector2 change) {
        if (!_player || _player.velocity.magnitude >= _unlockThreshold) return;
            
        _player.MovePosition(_player.position + change);
    }
    

    private Vector2 Pos => transform.position;
    private int _index = 0;

    void FixedUpdate()
    {
        HandlePatrol();
        HandleRadial();

        Vector2 newPos = transform.position;
        Vector2 change = newPos - _lastPos;
        _lastPos = newPos;

        MovePlayer(change);
    }

    void movePlatformPosition(Vector2 newPost)
    {
        transform.position = newPost;
    }

    void HandlePatrol()
    {
        
        if (patrolPts.Count == 0 || isRadial) return;
        
        
        Vector2 target =  (Vector2) _points[_index];
        Vector2 newPos = Vector2.MoveTowards(Pos, target, _speed * Time.fixedDeltaTime);
        movePlatformPosition(newPos);

        float diffMag = (Pos - target).magnitude;
        if (diffMag<= 0.5f) {
            if (_looped)
                _index = (_ascending ? _index + 1 : _index + patrolPts.Count - 1) % patrolPts.Count;
            else { // ping-pong
                if (_index >= patrolPts.Count - 1){
                    _ascending = false;
                    _index--;
                } 
                else if (_index <= 0) {
                    _ascending = true;
                    _index++;
                }
                _index = Mathf.Clamp(_index, 0, patrolPts.Count - 1);
            }
        }
        
    }
    
    void HandleRadial()
    {
        if (!isRadial) return;
        Vector2 newPos = _startPos + new Vector2(
            Mathf.Cos(Time.time * _speed), 
            Mathf.Sin(Time.time * _speed)
            ) * radius;
        movePlatformPosition(newPos);
    }

    
    private void OnDrawGizmos() {
        
        if (isRadial)
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        else
        {
            
            if (!Application.isPlaying) {
                _points = new List<Vector2>();
                foreach (var pt in patrolPts)
                {
                    _points.Add(pt.position);
                }
            }
            
            
            if (_points==null || _points.Count ==0) return;
            
            Vector2 previous = (Vector2) _points[0];
            Vector2 last = (Vector2) _points[^1];
            Gizmos.DrawWireSphere(previous, 0.2f);
            if (_looped) Gizmos.DrawLine(previous, last); 
            
            for (var i = 1; i < _points.Count; i++) {
                Vector2 p = (Vector2) _points[i];
                Gizmos.DrawWireSphere(p, 0.2f);
                Gizmos.DrawLine(previous, p);

                previous = p;
            }
        }
        
    }
   
    
    
}
