using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviseCapsuleBuff : MonoBehaviour
{
    [SerializeField] float speed = 20; // field creation and assigning
    [SerializeField] float leftBound = -6;
    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed,Space.World);
        }
        if (transform.position.x < leftBound && gameObject.CompareTag("Buffs"))
        {
            Destroy(gameObject);
        }
    }
}
