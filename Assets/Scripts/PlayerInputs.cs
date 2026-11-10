using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ChameleonGame
{
	public class PlayerInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public bool interact;
		public bool menu;
		public bool abilty1;

		[Header("Movement Settings")]
		public bool analogMovement;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}
		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}
		public void OnMenu(InputValue value)
		{
			menu = value.isPressed;
		}
		public void OnAbilty1(InputValue value)
		{
			abilty1 = value.isPressed;
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}
		public void MenuInput(bool newMenuState)
		{
			menu = newMenuState;
		}
		public void Abilty1Input(bool newstate)
		{
			abilty1 = newstate;
		}
	}
	
}