using Scripts.Game;
using Scripts.Player.Gun;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Shooting")] 
		public Rifle rifle;
		public Shotgun shotgun;
		public Launcher grenadeLauncher;
		public Knife knife;

		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnShoot() {

			switch (GameManager.Instance.BulletQueueManager.Top)
			{
				case BulletColor.Blue:
					if (rifle.TryShoot()) GameManager.Instance.BulletQueueManager.LoseAmmo();
					break;
				case BulletColor.Green:
					if (shotgun.TryShoot()) GameManager.Instance.BulletQueueManager.LoseAmmo();
					break;
				case BulletColor.Red:
					if (grenadeLauncher.TryShoot()) GameManager.Instance.BulletQueueManager.LoseAmmo();
					break;
				case BulletColor.Empty:
					knife.ThrowKnife();
					break;
				default:
					GameManager.Instance.BulletQueueManager.LoseAmmo();
					break;
			}
		}


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}
