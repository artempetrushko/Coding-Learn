using EmeraldAI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private EmeraldAIAbility _ability;
    [SerializeField] private float maxRandLimit = 5;
    [SerializeField] private float minRandLimit = -5;
    [SerializeField] private AnimationCurve PenetrationReductionGraph;
    [SerializeField] private bool RandomDamage = true;
    [SerializeField] private float TimeToDestruct = 10;
    
    public EmeraldAISystem AiSystem;
    
    private float CurrentDamage;
    private Vector3 PreviousStep;
    private float StartTime;
    private Rigidbody rb;


    private void Awake()
    {
        Destroy(this,TimeToDestruct);

        PreviousStep = gameObject.transform.position;

        StartTime = Time.time;
        rb = GetComponent<Rigidbody>();
        CurrentDamage = _ability.AbilityDamage;
        if (RandomDamage)
            CurrentDamage += Random.Range(minRandLimit, maxRandLimit);
        rb.isKinematic = false;
        /*Keyframe[] ks;
        ks = new Keyframe [3];
       
        ks [0] = new Keyframe (0, 1);
        ks [1] = new Keyframe (StartPoinOfDamageReduction / 100, 1);
        ks [2] = new Keyframe (1, FinalDamageInPercent / 100);
       
        DamageReductionGraph = new AnimationCurve (ks);*/
    }

    public void StartMovement(Transform TargetPoint,Transform _shootPoint)
    {
        rb.velocity=(TargetPoint.position-_shootPoint.position).normalized*_ability.ProjectileSpeed;
    }

    private void FixedUpdate()
    {
        var CurrentStep = gameObject.transform.rotation;

        transform.LookAt(PreviousStep, transform.up);
        var hit = new RaycastHit();
        var Distance = Vector3.Distance(PreviousStep, transform.position);
        if (Distance == 0.0f)
            Distance = 1e-05f;
        //  Debug.Log(Distance);
        if (Physics.Raycast(PreviousStep, transform.TransformDirection(Vector3.back), out hit, Distance * 0.9999f) &&
            hit.transform.gameObject != gameObject)
        {
            Debug.Log("ok");
            RegisterHit(hit);
        }

        gameObject.transform.rotation = CurrentStep;

        PreviousStep = gameObject.transform.position;
    }

    private void RegisterHit(RaycastHit hit)
    {
        if (hit.transform.tag == "Player")
        {
            var playerHealth = hit.transform.gameObject.GetComponent<EmeraldAIPlayerDamage>();
            if (!playerHealth)
                playerHealth=hit.transform.gameObject.AddComponent<EmeraldAIPlayerDamage>();
            playerHealth.SendPlayerDamage((int) CurrentDamage, hit.transform, AiSystem);
            Destroy(gameObject);
            AiSystem.OnDoDamageEvent.Invoke();
        }
    }
    
    private float GetDamageCoefficient()
    {
        var Value = 1.0f;
        var CurrentTime = Time.time - StartTime;
        Value = PenetrationReductionGraph.Evaluate(CurrentTime / TimeToDestruct);

        return Value;
    }
}