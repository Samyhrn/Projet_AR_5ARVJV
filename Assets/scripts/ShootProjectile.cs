using System.Collections;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;



public class ShootProjectile : MonoBehaviourPunCallbacks
{
    public GameObject projectilePrefab;
    public Camera ARCamera;
    public float shootForce = 10f;
    public float raycastLength = 1000000f;
    private PlayerScore playerExisting;
    public GameObject TargetPrefab;
    public GameObject ImageTarget;

    // Start is called before the first frame update
    void Start()
    {
        
        playerExisting = GameManager.instance.playerScores.FirstOrDefault(p => p.playerName == PhotonNetwork.LocalPlayer.NickName);
        if (playerExisting == null)
        {

            playerExisting = new PlayerScore()
            {
                playerName = PhotonNetwork.LocalPlayer.NickName,
                score = 0
            };
            GameManager.instance.playerScores.Add(playerExisting);
        }
        SpawnTarget();

    }

    void Update()
    {
        // Détection de l'appui sur le bouton espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    
    public void SpawnTarget()
    {
        
        GameObject target = PhotonNetwork.Instantiate(TargetPrefab.name,new Vector3(ImageTarget.transform.position.x+Random.Range(-250f,250f),ImageTarget.transform.position.y,ImageTarget.transform.position.z+Random.Range(-250f,250f)) ,
            ImageTarget.transform.rotation);
    }

    // Fonction de tir du projectile depuis la caméra AR
    public void Shoot()
    {
        GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, ARCamera.transform.position,
            ARCamera.transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(ARCamera.transform.forward * shootForce, ForceMode.Impulse);

        // Raycast pour détecter les collisions avec les objets qui ont un collider
        RaycastHit hit;
        if (Physics.Raycast(ARCamera.transform.position, ARCamera.transform.forward, out hit, raycastLength))
        {
            Debug.Log("Hit " + hit.transform.name);
            //tell witch player hit the target
            Debug.Log(PhotonNetwork.LocalPlayer.NickName+" hit the target");
        }
        
        //if we hit object tagged target then destroy the target after 4 seconds
        if (hit.transform.tag == "Target")
        {
            

            GameManager.instance.UpdateScore(PhotonNetwork.LocalPlayer.NickName, playerExisting.score + 1);
            
            Debug.Log("the score of the player "+PhotonNetwork.LocalPlayer.NickName+" is "+playerExisting.score);
            StartCoroutine(DestroyTarget(hit.transform.gameObject));
            
            
        }

        // Destruction du projectile après 2 secondes
        Destroy(projectile, 10f);

    }
    
    //destroy the target after 4 seconds
    IEnumerator DestroyTarget(GameObject target)
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.Destroy(target);
        SpawnTarget();
    }
}
