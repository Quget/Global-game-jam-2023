using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D  rigidbody2D = null;

    [SerializeField]
    private float movementSpeed = 25;

    [SerializeField]
    private Transform aimTransform = null;

    [SerializeField]
    private Transform gunPoint = null;

    [SerializeField]
    private Weapon weapon;

    private void Update()
    {
        Aim();
        FlipCheck();
        CheckInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void CheckInput()
    {
        float fireTrigger = Input.GetAxis("FireTrigger");
        if (Input.GetButton("Fire1") || fireTrigger > 0)
        {
            Vector3 direction = transform.position - gunPoint.position;
            //Apply bonus here
            float timeBetweenshot = 250;
            if (fireTrigger > 0)
                timeBetweenshot *= 1 + (1-fireTrigger);

            weapon.Shoot(-direction,gunPoint.position,1,0, 25, 5,0.5f, timeBetweenshot);
        }
    }

    private void FlipCheck()
    {
        Vector3 thisScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        thisScreenPoint = Input.mousePosition - thisScreenPoint;
        int flipScale = thisScreenPoint.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(flipScale, 1, 1);
    }

    private void Aim()
    {
        Vector2 input = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
        Vector3 aimVector = input;
        if (input == Vector2.zero)
        {
            aimVector = Camera.main.WorldToScreenPoint(aimTransform.position);
            aimVector = Input.mousePosition - aimVector;
            gunPoint.transform.localPosition = new Vector3(0, 1, 0);
        }
        else
        {
            gunPoint.transform.localPosition = new Vector3(1, 0, 0);
        }

        float angle = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

    }
    private void Movement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = (input * movementSpeed) * Time.fixedDeltaTime;
        //physics way
        rigidbody2D.AddForce(input * rigidbody2D.drag);
        //rigidbody2D.transform.Translate(input);
    }
}
