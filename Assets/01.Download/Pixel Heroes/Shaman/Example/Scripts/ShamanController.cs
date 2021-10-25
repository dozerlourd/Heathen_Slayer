using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSO
{
    public class ShamanController : RaycastPlayer
    {
        public bool KnockbackStarted = false;
        public float KnockbackSpeed = -5.0f;
        
        public Transform EffectsParent;
        public Transform BlowDartSpawnPos;
        public GameObject BlowDartPrefab;
        public float BlowDartSpeed = 5.0f;

        public ShamanPoisonArea PoisonArea;

        private bool _isNormalDeath;

        // Use this for initialization
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            Tick();
            
            EffectsParent.localScale = Direction == FacingDir.Right ? Vector3.one : new Vector3(-1, 1, 1);

            if (OnCooldown)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1) && !Attacking && !Stunned && IsAlive)
            {
                PerformLightAttack();
                TriggerCooldown();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && !Attacking && !Stunned && IsAlive)
            {
                PerformHeavyAttack();
                TriggerCooldown();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && !Attacking && !Stunned && IsAlive)
            {
                PerformBlowDartAttack();
                TriggerCooldown();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && !Attacking && !Stunned && IsAlive)
            {
                PerformCastSpell();
                TriggerCooldown();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) && !Attacking && !Stunned && IsAlive)
            {
                SmallFlinch();
            }

            if (Input.GetKeyDown(KeyCode.Alpha6) && !Attacking && !Stunned && IsAlive)
            {
                BigFlinch();
            }

            if (Input.GetKeyDown(KeyCode.Alpha7) && !Attacking && IsAlive)
            {
                ToggleDazed();
                if (Stunned)
                    Dazed();
            }

            if (Input.GetKeyDown(KeyCode.Alpha8) && !Attacking && IsAlive)
            {
                PerformKnockback();
            }

            if (Input.GetKeyDown(KeyCode.Alpha9) && !Attacking && !Stunned)
            {
                ToggleNormalDeath();
            }

            if (Input.GetKeyDown(KeyCode.Alpha0) && !Attacking && !Stunned)
            {
                ToggleSpecialDeath();
            }
        }

        private void ToggleSpecialDeath()
        {
            if (!IsAlive && _isNormalDeath)
                return;

            _isNormalDeath = false;
            animator.SetBool("NormalDeath", false);
            ToggleDeath();
        }

        private void ToggleNormalDeath()
        {
            if (!IsAlive && !_isNormalDeath)
                return;

            _isNormalDeath = true;
            animator.SetBool("NormalDeath", true);
            ToggleDeath();
        }

        void FixedUpdate()
        {
            FixedTick();
            UpdateAnimatorBase();
        }

        public override void StopAttacks()
        {
            base.StopAttacks();
            animator.SetBool("LightAttackSlashStart", false);
            animator.SetBool("HeavyAttackStabStart", false);
            animator.SetBool("BlowDartAttackStart", false);
            animator.SetBool("CastSpellStart", false);
            animator.SetBool("KnockbackStart", false);
        }

        public void PerformLightAttack()
        {
            Attacking = true;
            animator.SetBool("LightAttackSlashStart", true);
        }

        public void PerformHeavyAttack()
        {
            Attacking = true;
            animator.SetBool("HeavyAttackStabStart", true);
        }

        public void PerformBlowDartAttack()
        {
            Attacking = true;
            animator.SetBool("BlowDartAttackStart", true);
        }

        public void PerformCastSpell()
        {
            Attacking = true;
            animator.SetBool("CastSpellStart", true);
        }

        public void SmallFlinch()
        {
            Stun();
            animator.Play("SmallFlinch");
        }

        public void BigFlinch()
        {
            Stun();
            animator.Play("BigFlinch");
        }

        private void PerformKnockback()
        {
            Attacking = true;
            animator.SetBool("KnockbackStart", true);
        }

        public void StartKnockback()
        {
            SetAttackVelocityX(KnockbackSpeed);
        }

        public void FinishedKnockback()
        {
            SetAttackVelocityX(0);
        }
        
        public override void SpawnAttackEffect(int attackIndex)
        {
            switch (attackIndex)
            {
                case 1:
                    //spawn blow dart
                    SpawnBlowDart();
                    break;
                case 2:
                    //toggle poison area
                    PoisonArea.TogglePoisonArea();
                    break;
                default:
                    break;
            }
        }

        private void SpawnBlowDart()
        {
            var blowDart = Instantiate(BlowDartPrefab, BlowDartSpawnPos.position, Quaternion.identity);
            var blowDartProjectile = blowDart.GetComponent<ShamanBlowDart>();
            blowDartProjectile.Body.velocity = Direction == FacingDir.Right ?
                new Vector2(BlowDartSpeed, 0) : new Vector2(-BlowDartSpeed, 0);
            if (Direction == FacingDir.Left)
                blowDartProjectile.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}