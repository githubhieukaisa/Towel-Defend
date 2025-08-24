using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using Assets.HeroEditor.Common.ExampleScripts;
using HeroEditor.Common.Enums;
using System;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    public Character character;
    public BowExample bowExample;
    public Firearm firearm;
    public Transform armL;
    public Transform armR;
    public KeyCode fireButton = KeyCode.Mouse1;
    public KeyCode reloadButton = KeyCode.R;
    [Header("Check to disable arm auto rotation.")]
    public bool fixedArm;

    public void Start()
    {
        //if ((character.WeaponType == WeaponType.Firearms1H || character.WeaponType == WeaponType.Firearms2H) && firearm.Params.Type == FirearmType.Unknown)
        //{
        //    throw new Exception("Firearm params not set.");
        //}
    }

    public void Update()
    {
        if (character.Animator.GetInteger("State") >= (int)CharacterState.DeathB) return;

        switch (character.WeaponType)
        {
            case WeaponType.Melee1H:
            case WeaponType.Melee2H:
            case WeaponType.MeleePaired:
                if (Input.GetKeyDown(fireButton))
                {
                    character.Slash();
                }
                break;
            case WeaponType.Bow:
                bowExample.chargeButtonDown = Input.GetKeyDown(fireButton);
                bowExample.chargeButtonUp = Input.GetKeyUp(fireButton);
                break;
            case WeaponType.Firearms1H:
            case WeaponType.Firearms2H:
                firearm.Fire.FireButtonDown = Input.GetKeyDown(fireButton);
                firearm.Fire.FireButtonPressed = Input.GetKey(fireButton);
                firearm.Fire.FireButtonUp = Input.GetKeyUp(fireButton);
                firearm.Reload.ReloadButtonDown = Input.GetKeyDown(reloadButton);
                break;
            case WeaponType.Supplies:
                if (Input.GetKeyDown(fireButton))
                {
                    character.Animator.Play(Time.frameCount % 2 == 0 ? "UseSupply" : "ThrowSupply", 0); // Play animation randomly.
                }
                break;
        }

        if (Input.GetKeyDown(fireButton))
        {
            character.GetReady();
        }
    }

    /// <summary>
    /// Called each frame update, weapon to mouse rotation example.
    /// </summary>
    public void LateUpdate()
    {
        switch (character.GetState())
        {
            case CharacterState.DeathB:
            case CharacterState.DeathF:
                return;
        }

        Transform arm;
        Transform weapon;

        switch (character.WeaponType)
        {
            case WeaponType.Bow:
                arm = armL;
                weapon = character.BowRenderers[3].transform;
                break;
            case WeaponType.Firearms1H:
            case WeaponType.Firearms2H:
                arm = armR;
                weapon = firearm.FireTransform;
                break;
            default:
                return;
        }

        if (character.IsReady())
        {
            RotateArm(arm, weapon, fixedArm ? arm.position + 1000 * Vector3.right : Camera.main.ScreenToWorldPoint(Input.mousePosition), -40, 40);
        }
    }

    public float AngleToTarget;
    public float AngleToArm;

    /// <summary>
    /// Selected arm to position (world space) rotation, with limits.
    /// </summary>
    public void RotateArm(Transform arm, Transform weapon, Vector2 target, float angleMin, float angleMax) // TODO: Very hard to understand logic.
    {
        target = arm.transform.InverseTransformPoint(target);

        var angleToTarget = Vector2.SignedAngle(Vector2.right, target);
        var angleToArm = Vector2.SignedAngle(weapon.right, arm.transform.right) * Math.Sign(weapon.lossyScale.x);
        var fix = weapon.InverseTransformPoint(arm.transform.position).y / target.magnitude;

        AngleToTarget = angleToTarget;
        AngleToArm = angleToArm;

        if (fix < -1) fix = -1;
        else if (fix > 1) fix = 1;

        var angleFix = Mathf.Asin(fix) * Mathf.Rad2Deg;
        var angle = angleToTarget + angleFix + arm.transform.localEulerAngles.z;

        angle = NormalizeAngle(angle);

        if (angle > angleMax)
        {
            angle = angleMax;
        }
        else if (angle < angleMin)
        {
            angle = angleMin;
        }

        if (float.IsNaN(angle))
        {
            Debug.LogWarning(angle);
        }

        arm.transform.localEulerAngles = new Vector3(0, 0, angle + angleToArm);
    }

    private static float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;

        return angle;
    }
}
