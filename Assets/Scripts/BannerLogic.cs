using UnityEngine;

public class BannerLogic : MonoBehaviour
{
    Camera camToLookAt;

    // Start is called before the first frame update
    void Start()
    {
        camToLookAt = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(camToLookAt.transform.forward);
    }
}
