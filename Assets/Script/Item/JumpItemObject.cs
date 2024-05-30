using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItemObject : MonoBehaviour, IInteractable
{
    public JumpItemData data;

    private GameObject playerObject;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerObject = CharacterManager.Instance.Player.gameObject;
        playerRigidbody = playerObject.GetComponent<Rigidbody>();
    }

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (data.type)
            {
                case JumpItemType.JumpPad:
                    Vector3 Jumpdir = transform.up;
                    float JumpForce = data.JumpForce;
                    playerRigidbody.AddForce(Jumpdir * JumpForce, ForceMode.Impulse);
                    break;
                    
            }
        }
    }
}
