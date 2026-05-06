using DefaultNamespace;
using UnityEngine;
 
public class CoffeeGrinderController : MonoBehaviour

{

    [Header("Audio")]

    public AudioSource beansPourSound;

    public AudioSource filterSnapSound;

    public AudioSource grinderSound;

    [Header("Visual")]

    public ParticleSystem beansParticles;

    public ParticleSystem grindParticles;

    [Header("Portafilter Reference")]

    public PortafilterXR currentPortafilter; 

    private ITaskListManager taskManager;  // ← Interface
 
    private void Start()

    {

        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;

    }
 
    private bool hasBeans = false;

    private bool hasFilter = false;

    public void AddBeans()

    {

        if (!hasBeans)

        {

            hasBeans = true;

            beansPourSound.Play();

            beansParticles.Play();

            CheckGrinderReady();

            if (taskManager != null) 

                taskManager.OnBeansPoured();

        }

    }

    public void RemoveBeans()

    {

        hasBeans = false;

        beansParticles.Stop();

        StopGrinder();

    }

    public void AddFilter(PortafilterXR portafilter)

    {

        if (!hasFilter)

        {

            hasFilter = true;

            currentPortafilter = portafilter;  

            filterSnapSound.Play();

            CheckGrinderReady();

            if (taskManager != null)

                taskManager.OnFilterGround();

        }

    }

    public void RemoveFilter()

    {

        hasFilter = false;

        currentPortafilter = null;

        StopGrinder();

    }

    private void CheckGrinderReady()

    {

        Debug.Log($"CheckGrinderReady: hasBeans={hasBeans}, hasFilter={hasFilter}");

        if (hasBeans && hasFilter)

        {

            Debug.Log("START GRINDING!");

            grinderSound.Play();

            grindParticles.Play();

            StartGrinding();

        }

    }

    private void StartGrinding()

    {

        Invoke(nameof(FinishGrinding), 1.5f);

    }

    private void FinishGrinding()

    {

        if (currentPortafilter != null)

        {

            currentPortafilter.AddGrounds();

        }

        StopGrinder();

    }

    private void StopGrinder()

    {

        grinderSound.Stop();

        grindParticles.Stop();

    }

}
 