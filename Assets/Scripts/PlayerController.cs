using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{
    public AudioClip DeathClip; // 사망시 재생할 오디오 클립
    public float JumpForce = 700f; // 점프 힘
    public int MaxJumpCount = 2;

    private int _jumpCount = 0; // 누적 점프 횟수
    private bool _isOnGround = true; // 바닥에 닿았는지 나타냄
    private bool _isDead = false; // 사망 상태

    private Rigidbody2D _rigidbody; // 사용할 리지드바디 컴포넌트
    private Animator _animator; // 사용할 애니메이터 컴포넌트
    private AudioSource _audioSource; // 사용할 오디오 소스 컴포넌트
    private Vector2 _zero;

    private static class AnimationID // 이름공간으로 묶어주는 역할
    {
        public static readonly int IS_ON_GROUND = Animator.StringToHash("IsOnGround");
        public static readonly int DIE = Animator.StringToHash("Die");
    }

    private static readonly float MIN_NORMAL_Y = Mathf.Sin(45f * Mathf.Deg2Rad);

    private void Awake()
    {
           

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _zero = Vector2.zero;
    }
    public void Start()
    {
        GameManager.Instance.Foo();
    }
    private void Update()
    {
        if(_isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 최대 점프에 도달했으면 아무것도 안함 
            if(_jumpCount >= MaxJumpCount)
            {
                return;
            }

            ++_jumpCount;
            _rigidbody.velocity = _zero;
            _rigidbody.AddForce(new Vector2(0f, JumpForce));
            _audioSource.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity *= 0.5f;
            }
        }

        _animator.SetBool(AnimationID.IS_ON_GROUND, _isOnGround);
    }

    private void die()
    {
        // 사망 처리
        _isDead = true;
        _animator.SetTrigger(AnimationID.DIE);
        _rigidbody.velocity = _zero;
        _audioSource.PlayOneShot(DeathClip);

        GameManager.Instance.End();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if(other.tag == "Dead")
        {
            if (_isDead == false)
            {
                die();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿았음을 감지하는 처리
        ContactPoint2D point = collision.GetContact(0);
        if (point.normal.y >= MIN_NORMAL_Y)
        {
            _isOnGround = true;
            _jumpCount = 0;

            GameManager.Instance.AddScore();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isOnGround = false;
        // 바닥에서 벗어났음을 감지하는 처리
    }
}