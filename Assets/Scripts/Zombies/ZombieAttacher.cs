using UnityEngine;

//attach to zombie prefab
//handles attaching, damaging car, and returning to pool
public class ZombieAttacher : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private ZombieType zombieType;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int damagePerTick = 5;
    [SerializeField] private float damageInterval = 2f;
    [SerializeField] private float roamSpeed = 1f;
    [SerializeField] private float detectRange = 8f;

    [Header("Road Movement")]
    [SerializeField] private bool useCarSpeedForRoadMovement = true;
    [SerializeField] private float roadSpeedMultiplier = 10f;
    [SerializeField] private float extraZombieSpeed = 0f;
    [SerializeField] private Vector3 roadMoveDirection = Vector3.back;

    [Header("Runtime")]
    [SerializeField] private Transform targetCar;
    [SerializeField] private ZombieDamageManager currentCar;
    [SerializeField] private Transform currentAnchor;

    private int currentHP;
    private float damageTimer;
    private Rigidbody rb;
    private Collider zombieCollider;
    private CarController carController;
    private ZombiePool owningPool;
    private IZombieState currentState;
    private bool isDead;

    public ZombieType Type => zombieType;
    public bool IsAttached => currentCar != null && currentAnchor != null;
    public Transform TargetCar => targetCar;
    public ZombieDamageManager CurrentCar => currentCar;
    public Transform CurrentAnchor => currentAnchor;
    public int DamagePerTick => damagePerTick;
    public float DamageInterval => damageInterval;
    public float DetectRange => detectRange;
    public float RoamSpeed => roamSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        zombieCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        ZombieEvents.OnZombieDamagePowerUp += HandleZombieDamagePowerUp;
        ZombieEvents.OnGameOver += HandleGameOver;
        ZombieEvents.OnCarDestroyed += HandleCarDestroyed;
        CarHealthManager.OnCarDestroyed += HandleCarDestroyedFromHealthManager;
    }

    private void OnDisable()
    {
        ZombieEvents.OnZombieDamagePowerUp -= HandleZombieDamagePowerUp;
        ZombieEvents.OnGameOver -= HandleGameOver;
        ZombieEvents.OnCarDestroyed -= HandleCarDestroyed;
        CarHealthManager.OnCarDestroyed -= HandleCarDestroyedFromHealthManager;
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }

    private void LateUpdate()
    {
        if (IsAttached)
        {
            FollowAttachPoint();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState?.HandleCollision(this, collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        TryAttachFromCollider(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryAttachFromCollider(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryAttachFromCollider(other);
    }

    public void Configure(ZombieData data)
    {
        zombieType = data.type;
        maxHP = data.maxHP;
        damagePerTick = data.damagePerTick;
        damageInterval = data.damageInterval;
        roamSpeed = data.roamSpeed;
        detectRange = data.detectRange;
    }

    public void SetPool(ZombiePool pool)
    {
        owningPool = pool;
    }

    public void ResetZombie()
    {
        isDead = false;
        currentHP = maxHP;
        damageTimer = 0f;

        FindPlayerCar();
        DetachFromCar();
        EnablePhysics(true);

        ChangeState(new RoamingState());
    }

    public void ChangeState(IZombieState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    public void MoveTowardCar()
    {
        if (IsAttached)
        {
            return;
        }

        if (useCarSpeedForRoadMovement)
        {
            MoveWithRoad();
        }
        else
        {
            MoveDirectlyTowardCar();
        }
    }

    private void MoveWithRoad()
    {
        if (carController == null)
        {
            FindPlayerCar();
        }

        float carSpeed = carController != null ? carController.GetSpeed() : 1f;
        float moveSpeed = (carSpeed * roadSpeedMultiplier) + extraZombieSpeed;
        Vector3 movement = roadMoveDirection.normalized * moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);
        FaceCarOrMovementDirection(movement);
    }

    private void MoveDirectlyTowardCar()
    {
        if (targetCar == null)
        {
            FindPlayerCar();
        }

        Vector3 moveDirection = targetCar != null
            ? (targetCar.position - transform.position).normalized
            : Vector3.back;

        Vector3 movement = moveDirection * roamSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        FaceCarOrMovementDirection(movement);
    }

    public bool CanTryAttach()
    {
        if (IsAttached)
        {
            return false;
        }

        ZombieDamageManager carDamageManager = FindCarDamageManager();
        if (carDamageManager == null)
        {
            return false;
        }

        Transform attachPoint = carDamageManager.GetClosestAvailableAttachPoint(transform.position);
        if (attachPoint == null)
        {
            return false;
        }

        float distanceToAttachPoint = Vector3.Distance(transform.position, attachPoint.position);
        return distanceToAttachPoint <= detectRange;
    }

    public bool TryAttachToCar()
    {
        ZombieDamageManager carDamageManager = FindCarDamageManager();
        if (carDamageManager == null)
        {
            return false;
        }

        return TryAttachUsingManager(carDamageManager, true);
    }

    public void TryAttachFromCollider(Collider other)
    {
        if (IsAttached || other == null)
        {
            return;
        }

        ZombieDamageManager carDamageManager = other.GetComponentInParent<ZombieDamageManager>();
        if (carDamageManager == null)
        {
            return;
        }

        targetCar = carDamageManager.transform;
        carController = carDamageManager.GetComponent<CarController>();
        TryAttachUsingManager(carDamageManager, false);
    }

    private bool TryAttachUsingManager(ZombieDamageManager carDamageManager, bool checkRange)
    {
        if (carDamageManager == null || IsAttached)
        {
            return false;
        }

        Transform attachPoint = carDamageManager.GetClosestAvailableAttachPoint(transform.position);
        if (attachPoint == null)
        {
            return false;
        }

        if (checkRange)
        {
            float distanceToAttachPoint = Vector3.Distance(transform.position, attachPoint.position);
            if (distanceToAttachPoint > detectRange)
            {
                return false;
            }
        }

        if (!carDamageManager.RegisterZombie(this, attachPoint))
        {
            return false;
        }

        currentCar = carDamageManager;
        currentAnchor = attachPoint;

        transform.SetParent(currentAnchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        ChangeState(new AttachedState());
        return true;
    }

    private ZombieDamageManager FindCarDamageManager()
    {
        if (targetCar == null)
        {
            FindPlayerCar();
        }

        if (targetCar == null)
        {
            return null;
        }

        ZombieDamageManager carDamageManager = targetCar.GetComponent<ZombieDamageManager>();
        if (carDamageManager == null)
        {
            carDamageManager = targetCar.GetComponentInChildren<ZombieDamageManager>();
        }
        if (carDamageManager == null)
        {
            carDamageManager = targetCar.GetComponentInParent<ZombieDamageManager>();
        }

        return carDamageManager;
    }

    public void FollowAttachPoint()
    {
        if (currentAnchor == null)
        {
            return;
        }

        transform.position = currentAnchor.position;
        transform.rotation = currentAnchor.rotation;
    }

    public void ResetDamageTimer()
    {
        damageTimer = 0f;
    }

    public void UpdateAttachedDamage()
    {
        if (currentCar == null)
        {
            return;
        }

        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f;
            currentCar.DamageCar(damagePerTick);
        }
    }

    public void DetachFromCar()
    {
        if (currentCar != null)
        {
            currentCar.UnregisterZombie(this);
        }

        transform.SetParent(null);
        currentCar = null;
        currentAnchor = null;
        damageTimer = 0f;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
        {
            return;
        }

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        ZombieEvents.RaiseZombieKilled(this);
        ChangeState(new DeadState());
    }

    public void EnablePhysics(bool enabled)
    {
        if (rb != null)
        {
            rb.isKinematic = !enabled;
            rb.useGravity = enabled;

            if (!enabled)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (zombieCollider != null)
        {
            zombieCollider.enabled = enabled;
        }
    }

    public void ReturnToPoolOrDestroy()
    {
        if (owningPool != null)
        {
            owningPool.ReturnZombie(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void FindPlayerCar()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetCar = player != null ? player.transform : null;
        carController = player != null ? player.GetComponent<CarController>() : null;
    }

    private void FaceCarOrMovementDirection(Vector3 movement)
    {
        Vector3 direction = movement.normalized;

        if (targetCar != null)
        {
            Vector3 toCar = targetCar.position - transform.position;
            toCar.y = 0f;

            if (toCar.sqrMagnitude > 0.01f)
            {
                direction = toCar.normalized;
            }
        }

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    private void HandleZombieDamagePowerUp(int damage)
    {
        TakeDamage(damage);
    }

    private void HandleGameOver()
    {
        ChangeState(new DeadState());
    }

    private void HandleCarDestroyed()
    {
        ChangeState(new DeadState());
    }

    private void HandleCarDestroyedFromHealthManager(CarHealthManager.DamageSource source)
    {
        ChangeState(new DeadState());
    }
}
