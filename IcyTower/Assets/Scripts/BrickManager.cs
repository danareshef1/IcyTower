using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> ObjectsList = new List<GameObject>();
    private IRelativePositionManager positionManager;
    private IJumper jumper;

    [SerializeField] private float nextObjectHeight;
    [SerializeField] private float offsetUnder;
    [SerializeField] private int boundries;

    private void Awake()
    {
        positionManager = new JumperRelativePositionManager(ObjectsList);
        positionManager.Boundries = boundries;
        positionManager.NextObjectDelta = nextObjectHeight;
        positionManager.MoveOffset = offsetUnder;
        positionManager.MovedObject += HandleMovedObject;
    }

    private void Start()
    {
        jumper = GameObject.FindObjectOfType<Jumper>();
        DisableAllColliders();
    }

    void Update()
    {
        positionManager.MoveObject();
        EnableCollidersBasedOnPosition();
    }

    private void HandleMovedObject(GameObject brick)
    {
        DisableCollider(brick);
    }

    private void DisableAllColliders()
    {
        foreach (GameObject brick in ObjectsList)
        {
            DisableCollider(brick);
        }
    }

    private void DisableCollider(GameObject brick)
    {
        Collider2D collider = brick.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    private void EnableCollidersBasedOnPosition()
    {
        foreach (GameObject brick in ObjectsList)
        {
            Collider2D collider = brick.GetComponent<Collider2D>();
            if (collider != null)
            {
                // Enable collider if brick is in its correct position
                if (jumper.MaxHeight >= brick.transform.position.y)
                {
                    collider.enabled = true;
                }
                else
                {
                    collider.enabled = false;
                }
            }
        }
    }
}
