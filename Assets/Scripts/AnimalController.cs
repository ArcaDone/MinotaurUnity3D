using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    private Vector3 randomPos;

    public Transform currentTarget;

    private NavMeshAgent agent;
    public string walkingBool = "isWaling";
    public string runningBool = "isRunning";
    public string attackingBool = "isAttacking";
     
    public int distanceFromTarget = 15;
    public float searchDistance = 20f;
    public float walkSpeed = 0.75f;
    public float runSpeed = 6f;

    private Animator anim;

    private bool isIdling;
    private bool isRunning;
    private bool isAttacking;

    private Transform startTransform;
    public float multiplyBy = 5f;

    private void Start()
    {
        randomPos = transform.position;
        FindPlayer();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        WalkToRandomSpot();
    }

    private void Update()
    {

        FindPlayer();

        if (currentTarget == null)
        {
            WalkToRandomSpot();
            return;
        }

        if (Vector3.Distance(transform.position, currentTarget.position) <= distanceFromTarget)
        {
            if (currentTarget.gameObject.CompareTag("Player"))
            {
                ChasePlayer();
            }
            

        }
        else if (isRunning)
        {
            WalkToRandomSpot();
        }

        if (isIdling)
        {
            if (Vector3.Distance(transform.position, randomPos) <= 1)
            {
                WalkToRandomSpot();
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(50000, transform.position, 10);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void WalkToRandomSpot()
    {
        agent.speed = walkSpeed;
        randomPos = new Vector3(transform.position.x + Random.Range(1, 10), transform.position.y, transform.position.z + Random.Range(1, 10));

        agent.SetDestination(randomPos);

        isRunning = false;
        isIdling = true;
        isAttacking = false;
        anim.SetBool(runningBool, isRunning);
        anim.SetBool(walkingBool, isIdling);
        anim.SetBool(attackingBool, isAttacking);
        currentTarget = null;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(currentTarget.transform.position);
        if (!isRunning)
        {
            Debug.Log("Run");
            isRunning = true;
            isIdling = false;
            agent.speed = 2f;
            anim.SetBool(runningBool, isRunning);
            anim.SetBool(walkingBool, isIdling);
        }

        if (Vector3.Distance(transform.position, currentTarget.position) <= 2f)
        {
            Debug.Log("Attack");
            isRunning = false;
            isAttacking = true;
            isIdling = false;
            anim.SetBool(runningBool, isRunning);
            anim.SetBool(attackingBool, isAttacking);
            anim.SetBool(walkingBool, isIdling);
        }
        
    }

    void FindPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchDistance);
        foreach (Collider nearyby in colliders)
        {
            if (nearyby.gameObject.CompareTag("Player"))
            {
                currentTarget = nearyby.transform;
                return;
            }
        }
    }
}