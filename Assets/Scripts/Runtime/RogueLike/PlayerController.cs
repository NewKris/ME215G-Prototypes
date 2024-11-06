using System;
using UnityEngine;

namespace Demos.Runtime.RogueLike {
    public class PlayerController : MonoBehaviour {
        public PlayerCharacter playerCharacter;
        
        private void Update() {
            if (Input.GetButtonDown("Jump")) {
                playerCharacter.Jump();
            }
            
            playerCharacter.MoveDirection = Input.GetAxisRaw("Horizontal");
        }
    }
}
