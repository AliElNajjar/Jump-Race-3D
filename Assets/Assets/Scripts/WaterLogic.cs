using UnityEngine;



public class WaterLogic : MonoBehaviour
{
    public GameObject splash;
    public AudioSource waterSplash;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Splash
            splash.gameObject.transform.position = other.gameObject.transform.position;

            splash.gameObject.SetActive(true);
            waterSplash.Play();
            other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.SetActive(false);

            GameController.SharedInstance.LevelLost();

        } 
    }
}
