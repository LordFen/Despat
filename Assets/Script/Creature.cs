using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CreaturePattern
    {

    public class Creature {

 
        public Transform creatureTrans;
        public int id;
        public bool die = false;
        public Animator anim;

        protected GameObject explosion;
        protected Creature closestObject;
        protected Rigidbody2D rigidbody;
        protected int hp;
        protected float damage;
        protected Vector2 direction;
        protected Vector2 LastMove;
        protected AudioClip clip;

        public enum CreatureState{
            Move,
            Attack,
            Idle
        }

        public CreatureState state = CreatureState.Move;

        public Creature()
        {
            
        }

        public Creature(Transform t)
        {
            rigidbody = t.GetComponent<Rigidbody2D>();
        }

        public void AddClosestObject(Creature c)
        {
            closestObject = c;
        }      

        protected virtual void UpdateState(CreatureState cS)
        {
            float distance = 0.0f;
            if(closestObject!=null)
            distance = (closestObject.creatureTrans.position - creatureTrans.position).sqrMagnitude;


            switch (cS)
            {
                case CreatureState.Move:

                    if (closestObject == null)                 
                    {
                        state = CreatureState.Idle;
                        anim.SetBool("Move", false);
                        anim.SetFloat("LastX", LastMove.x);
                        anim.SetFloat("LastY", LastMove.y);

                    }
                    else if (distance <= 0.8f)
                    {
                        state = CreatureState.Attack;
                        anim.SetBool("Move", false);
                        anim.SetFloat("LastX", LastMove.x);
                        anim.SetFloat("LastY", LastMove.y);
                    }

                    break;

                case CreatureState.Attack:
                    if (closestObject == null)
                    {
                        state = CreatureState.Idle;
                        anim.SetBool("Move", false);
                        anim.SetFloat("LastX", LastMove.x);
                        anim.SetFloat("LastY", LastMove.y);
                    }
                    else if (distance > 0.8f)
                    {
                        state = CreatureState.Move;
                        anim.SetBool("Move", true);
                    }
                    break;

                case CreatureState.Idle:
                    if (closestObject != null)
                    {
                        if (distance <= 0.8f)
                        {
                            state = CreatureState.Attack;
                            anim.SetBool("Move", false);
                            anim.SetFloat("LastX", LastMove.x);
                            anim.SetFloat("LastY", LastMove.y);
                        }
                        else
                        state = CreatureState.Move;
                        anim.SetBool("Move", true);
                    }

                    break;
            }




        }

        public virtual void Action() {

            UpdateState(state);

            switch (state)
            {
                case CreatureState.Move:
                    Move(closestObject);
                    break;
                case CreatureState.Attack:
                    Attack();
                    break;
                default:
                    break;
            }
                        
        }
        protected virtual void Move(Creature c) {
            creatureTrans.position = Vector3.MoveTowards(creatureTrans.position, c.creatureTrans.position, Time.fixedDeltaTime);
            Vector3 heading = c.creatureTrans.position - creatureTrans.position;
            heading = heading / heading.magnitude;
            direction = new Vector2(heading.x*10, heading.y*10);
            anim.SetFloat("InputX", heading.x);
            anim.SetFloat("InputY", heading.y);
            LastMove.x = heading.x;
            LastMove.y = heading.y;
            
        }

       
        protected virtual void Attack() {

            GameController.audioSource.PlayOneShot(clip);

            rigidbody.AddForce((closestObject.creatureTrans.position - creatureTrans.position).normalized * -5, ForceMode2D.Impulse);
            if (closestObject.rigidbody != null)
            {
                closestObject.rigidbody.AddForce((creatureTrans.position - closestObject.creatureTrans.position).normalized * -5, ForceMode2D.Impulse);
               
                closestObject.Damage(this.id);
                Damage(closestObject.id);
            }
            else
            {
                closestObject.creatureTrans.gameObject.GetComponent<UnityStandardAssets.Copy._2D.Platformer2DUserControl>().GetDamage();

            }
           
        }

        protected virtual void Damage(int iD)
        {
          
        }

        protected virtual void WeakDamage() {
            hp -= 10;
        }
        protected virtual void MediumDamage() {
            hp -= 20;
        }
        protected virtual void StrongDamage() {
            hp -= 50;
        }

        protected virtual void CheckDie()
        {
            if(hp<=0)
            {
                //Die
                die = true;
            }
        }

    }
}
