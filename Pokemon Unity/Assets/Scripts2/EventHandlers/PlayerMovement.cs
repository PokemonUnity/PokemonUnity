using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PokemonUnity.Unity.ThreeDimensional {
	public class PlayerMovement : MonoBehaviour
	{
		private Animator _anim;
		private Rigidbody _rigid;

		[SerializeField]
		private float _walkSpeed = 10f;
		[SerializeField]
		private float _runSpeed = 10f;
		[SerializeField]
		private float _sneakSpeed = 10f;
		[SerializeField]
		private float _sneakWait = 1f;
		[SerializeField]
		private float _rotationSpeed = 10f;
		[SerializeField]
		private float _warmUpAfter = 10f;

		private Transform _playerModel;
		private Tile _curTile;
		private List<Tile> _neighbours;
		private Tile _frontTile;
		private Vector3 _movementDirection;
		private bool _sneakingActived = false;
		private bool _warmingUpCanActivate = true;
		private Coroutine _warmUpCoroutine;
		private Coroutine _warmUpTimeoutCoroutine;


		#region Animation Properties
		private bool IsWalking
		{
			get { return _anim.GetBool("IsWalking"); }
			//Set GameVariable Boool to True as well
			set { _anim.SetBool("IsWalking", value); }
		}
		private bool IsRunning
		{
			get { return _anim.GetBool("IsRunning"); }
			set { _anim.SetBool("IsRunning", value); }
		}

		private bool IsSneaking
		{
			get { return _anim.GetBool("IsSneaking"); }
			//ToDo: Set GameVariable Boool to True as well
			//Sneaking modifies increases encounter rates and goes together with pokegear
			set { _anim.SetBool("IsSneaking", value); }
		}
		private bool IsWarmingUp
		{
			get { return _anim.GetBool("IsWarmingUp"); }
			set { _anim.SetBool("IsWarmingUp", value); }
		}
		#endregion

		#region Unity Properties
		/// <summary>
		/// Initialize playermovement script
		/// </summary>
		void Start()
		{
			_playerModel = transform.GetChild(0);
			_anim = _playerModel.GetComponent<Animator>();
			_rigid = GetComponent<Rigidbody>();
			//SetTileAndNeighbours();
		}

		void Update()
		{
			if (IsWalking || IsRunning || IsSneaking)
			{
				return;
			}

			if (_warmingUpCanActivate == false && _warmUpTimeoutCoroutine == null)
			{
				_warmUpTimeoutCoroutine = StartCoroutine(WarmUpTimeout());
			}
			else if (_warmingUpCanActivate && _warmUpCoroutine == null)
			{
				_warmUpCoroutine = StartCoroutine(WarmUp());
			}
		}

		void FixedUpdate()
		{
			if (Math.Abs(Input.GetAxis("Horizontal")) > 0.01f || Math.Abs(Input.GetAxis("Vertical")) > 0.01f)
			{
				DisableWarmUp();

				IsRunning = Input.GetButton("Fire3");
				if (IsRunning == false)
				{
					IsSneaking = Input.GetButton("Fire1");
					IsWalking = !IsSneaking;
				}
				else
				{
					IsWalking = true;
				}

				float movementSpeed = IsRunning ? _runSpeed : _walkSpeed;

				// * -1 to fix reverse movement (camera and map world positioning)
				float moveHorizontal = Input.GetAxis("Horizontal") * -1;
				float moveVertical = Input.GetAxis("Vertical") * -1;
				_movementDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);

				//SetTileAndNeighbours();
				//if (_frontTile != null)
				//{
				//	_frontTile.Fill = false;
				//}
				//_frontTile = Grid.GetFrontNeighbours(_curTile, _movementDirection);

				_playerModel.rotation = Quaternion.RotateTowards(_playerModel.rotation,
					Quaternion.LookRotation(_movementDirection), _rotationSpeed);

				HandleLayerMovement(movementSpeed);

				if (IsSneaking)
				{
					if (_sneakingActived == false)
					{
						StartCoroutine(SneakMovement());
					}
					return;
				}

				if (_sneakingActived)
				{
					_sneakingActived = false;
					StopCoroutine(SneakMovement());
				}
				_rigid.velocity = _movementDirection * movementSpeed;
			}
			else
			{
				_rigid.velocity = Vector3.zero;
				IsWalking = false;
				IsRunning = false;
				IsSneaking = false;
			}
		}
		#endregion

		#region Class Properties
		private void HandleLayerMovement(float movementSpeed)
		{
			//if (_frontTile != null && _frontTile.Position.y != transform.position.y && _frontTile.CanWalk)
			//{
			//	_movementDirection.y = _frontTile.Position.y > transform.position.y ? .25f : -.25f;
			//	_movementDirection.y *= movementSpeed;
			//
			//	float moveVertical = _movementDirection.z < 0 ? _movementDirection.z * -1 : _movementDirection.z;
			//	float moveHorizontal = _movementDirection.x < 0 ? _movementDirection.x * -1 : _movementDirection.x;
			//	float subSpeed = Mathf.Max(moveHorizontal, moveVertical);
			//
			//	if (subSpeed % 1 != 0)
			//	{
			//		_movementDirection.y *= subSpeed % 1 * 2;
			//	}
			//}
			//if (Mathf.Abs(_curTile.Position.y - transform.position.y) < 0.02f)
			//{
			//	transform.position = new Vector3(transform.position.x, _curTile.Position.y, transform.position.z);
			//	_movementDirection.y = 0;
			//}
		}

		private void DisableWarmUp()
		{
			_warmingUpCanActivate = false;
			IsWarmingUp = false;

			if (_warmUpCoroutine != null)
			{
				StopCoroutine(_warmUpCoroutine);
				_warmUpCoroutine = null;
			}
			if (_warmUpTimeoutCoroutine != null)
			{
				StopCoroutine(_warmUpTimeoutCoroutine);
				_warmUpTimeoutCoroutine = null;
			}
		}

		/// <summary>
		/// Sneak move with timeout before movement
		/// </summary>
		/// <returns></returns>
		private IEnumerator SneakMovement()
		{
			_sneakingActived = true;
			yield return new WaitForSeconds(_sneakWait);
			_rigid.velocity = _movementDirection * _sneakSpeed;
			_sneakingActived = false;
		}

		IEnumerator WarmUp()
		{
			IsWarmingUp = true;
			yield return new WaitForSeconds(3f);
			IsWarmingUp = false;
			_warmUpCoroutine = null;
			_warmingUpCanActivate = false;
		}

		IEnumerator WarmUpTimeout()
		{
			yield return new WaitForSeconds(_warmUpAfter);
			_warmingUpCanActivate = true;
			_warmUpTimeoutCoroutine = null;
		}
		#endregion
	}
}
	public class Tile
	{

	}