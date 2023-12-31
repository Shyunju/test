using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPovot;
    [SerializeField] private SpriteRenderer characterRendeerer;
    private TopDownCharacterController _controller;
    // Start is called before the first frame update

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
    }
    void Start()
    {
        _controller.OnLookEvent += OnAim;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAim(Vector2 newAimDirection)
    {
        RotateArm(newAimDirection);
    }
    private void RotateArm(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        characterRendeerer.flipX = armRenderer.flipY;
        armPovot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
