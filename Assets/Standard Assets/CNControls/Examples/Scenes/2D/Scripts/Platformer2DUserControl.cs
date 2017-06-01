using UnityEngine;
using CnControls;

// Just in case so no "duplicate definition" stuff shows up
namespace UnityStandardAssets.Copy._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {

        public int hp = 100;
        private PlatformerCharacter2D m_Character;
        private bool move = false;
        private bool die = false;
        private Animator anim;
        private Vector2 lastMove;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            anim = GetComponent<Animator>();
        }
        
        private void FixedUpdate()
        {


            float h = CnInputManager.GetAxis("Horizontal");
            float k = CnInputManager.GetAxis("Vertical");

            if(h==0 && k==0)
            {
                move = false;
            }
         

            else if (h<=0.5f && k<=0.5f && h>=-0.5f && k>= -0.5f)
            {
                move = false;
                h = 0;
                k = 0;
            }
            else
            {
                move = true;
                lastMove.x= CnInputManager.GetAxis("Horizontal");
                lastMove.y= CnInputManager.GetAxis("Vertical");
            }

            if (!die)
            {
                anim.SetFloat("InputX", h);
                anim.SetFloat("InputY", k);
                anim.SetBool("Move", move);
                anim.SetFloat("LastX", lastMove.x);
                anim.SetFloat("LastY", lastMove.y);


                m_Character.Move(h, k);
            }
           

        }

        public void GetDamage()
        {
            hp -= 10;
            if(hp<=0)
            {
                hp = 0;
                die = true;
                anim.SetBool("Die", die);
            }
        }
      

        public bool GetDeath()
        {
            return die;
        }
    }
}
