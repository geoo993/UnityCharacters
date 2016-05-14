/* 2D Top Down
 * The following code restricts the character's movement to only 8 directions (even when using a controller)
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PlayerMovementExamples : MonoBehaviour {

//	private void updateMoveCharacter() {
//		Vector2 stickDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//		
//		//stickDirection = stickDirection.normalized;
//		
//		if(stickDirection.sqrMagnitude > 0.031) {
//			
//			// Rotate the character
//			float moveAngle = Mathf.Atan2(stickDirection.y, stickDirection.x)*Mathf.Rad2Deg;
//			if(moveAngle < 0) {
//				moveAngle += 360;
//			}
//			
//			//Debug.Log (moveAngle);
//			moveAngle = getDirection(moveAngle);
//
//			transform.rotation = Quaternion.Euler(0, 0, moveAngle);
//			rigidbody2D.velocity = transform.right * speed;
//		} else {
//			rigidbody2D.velocity = Vector2.zero;
//		}
//	}
//
//	private float getDirection(float moveAngle) {
//		if(moveAngle < 22.5 || moveAngle >= 337.5) {
//			// Face right
//			return 0;
//		} else if(moveAngle < 67.5 && moveAngle >= 22.5) {
//			// Face up right
//			return 45;
//		} else if(moveAngle < 112.5 && moveAngle >= 67.5) {
//			// Face up
//			return 90;
//		} else if(moveAngle < 157.5 && moveAngle >= 112.5) {
//			// Face up left
//			return 135;
//		} else if(moveAngle < 202.5 && moveAngle >= 157.5) {
//			// Face left
//			return 180;
//		} else if(moveAngle < 247.5 && moveAngle >= 202.5) {
//			// Face down left
//			return 225;
//		} else if(moveAngle < 292.5 && moveAngle >= 247.5) {
//			// Face down
//			return 270;
//		} else if(moveAngle < 337.5 || moveAngle >= 292.5) {
//			// Face down right
//			return 315;
//		}
//		
//		Debug.Log ("SHOULD NOT BE REACHING THIS CODE");
//		return 315;
//	}

}

/* 2D Top Down
 * The following code will move the player in all directions with the analog stick or just 8 directions with the arrow keys
 */