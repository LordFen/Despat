using System;
using UnityEngine;

// Just in case so no "duplicate definition" stuff shows up
namespace UnityStandardAssets.Copy._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField]
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        private Rigidbody2D m_Rigidbody2D;
        

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Move(float moveX, float moveY)
        {
            m_Rigidbody2D.velocity = new Vector2(moveX * m_MaxSpeed, moveY * m_MaxSpeed);
        }

    }
}
