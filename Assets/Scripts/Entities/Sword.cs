// Created by Binh Bui on 06, 27, 2021

using UnityEngine;

namespace Entities
{
    public class Sword : MonoBehaviour
    {
        public Animator animator;
        private void OnTriggerEnter2D(Collider2D other)
        {
            Slash();
        }

        private void Slash()
        {
            animator.Play("SwordSlash");
        }
    }
}