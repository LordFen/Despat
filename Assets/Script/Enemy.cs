using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreaturePattern
{
    public class Enemy:Creature
    {

        Transform playerTarget;

        public Enemy(GameObject g, GameObject player,int id,Animator a, GameObject e, AudioClip clip) : base(g.transform)
        {
            
            this.creatureTrans = g.transform;
            this.playerTarget = player.transform;
            this.id = id;
            this.hp = 100;
            this.anim = a;
            anim.SetBool("Move", true);
            this.explosion = e;
            this.clip = clip;
        }

        protected override void Move(Creature target)
        {
                base.Move(target);
           
        }

        protected override void UpdateState(CreatureState cS)
        {
            base.UpdateState(cS);
        }

        protected override void Attack()
        {          
            base.Attack();
        }

        protected override void Damage(int idFriend)
        {
            
            GameObject g = Lean.LeanPool.Spawn(explosion, creatureTrans.transform.position, Quaternion.identity);
            Lean.LeanPool.Despawn(g, 2);
           
            switch (idFriend)
            {
                case 0:
                    switch (id)
                    {
                        case 0:
                            MediumDamage();
                            break;

                        case 1:
                            WeakDamage();
                            break;

                        case 2:
                            StrongDamage();
                            break;
                    }
                    break;

                case 1:
                    switch (id)
                    {
                        case 0:
                            StrongDamage();
                            break;

                        case 1:
                            MediumDamage();
                            break;

                        case 2:
                            WeakDamage();
                            break;
                    }
                    break;

                case 2:
                    switch (id)
                    {
                        case 0:
                            WeakDamage();
                            break;

                        case 1:
                            StrongDamage();
                            break;

                        case 2:
                            MediumDamage();
                            break;
                    }
                    break;
            }

            CheckDie();
        }



    }
}
