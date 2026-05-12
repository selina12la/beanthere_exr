using UnityEngine;

public class MilkSteamer : MonoBehaviour
{
    public MilkFrotherSnapZone snapZone;

    private void Start()
    {
        if (snapZone == null)
        {
            snapZone = GetComponent<MilkFrotherSnapZone>();
        }
    }

    public void OnMilkSteamed()
    {
        TaskManagerLocator.Current?.OnMilkSteamed();
    }
}