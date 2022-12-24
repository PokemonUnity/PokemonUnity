using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Use these smooth methods to move one value towards another<br /><br />
* <strong>Example: </strong><br />fromY = LeanSmooth.spring(fromY, followArrow.localPosition.y, ref velocityY, 1.1f);<br />
* fromVec3 = LeanSmooth.damp(fromVec3, dude5Title.localPosition, ref velocityVec3, 1.1f);<br />
* fromColor = LeanSmooth.damp(fromColor, dude5Title.GetComponent<Renderer>().material.color, ref velocityColor, 1.1f);<br />
* Debug.Log("Smoothed y:" + fromY + " vec3:" + fromVec3 + " color:" + fromColor);<br />
*
* @class LeanSmooth
*/

public class LeanSmooth {


	/**
	* <summary>Moves one value towards another (eases in and out to destination with no overshoot)</summary>
	* 
	* @method LeanSmooth.damp (float)
	* @param {float} current:float the current value
	* @param {float} target:float the value we are trying to reach
	* @param {float} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @example
	* followVar = LeanSmooth.damp(followVar, destinationVar, ref followVelocity, 1.1f);\n
	* Debug.Log("current:"+followVar);
	*/
	public static float damp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
			deltaTime = Time.deltaTime;

		smoothTime = Mathf.Max(0.0001f, smoothTime);
		float num = 2f / smoothTime;
		float num2 = num * deltaTime;
		float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
		float num4 = current - target;
		float num5 = target;
		if (maxSpeed > 0f)
		{
			float num6 = maxSpeed * smoothTime;
			num4 = Mathf.Clamp(num4, -num6, num6);
		}
		target = current - num4;
		float num7 = (currentVelocity + num * num4) * deltaTime;
		currentVelocity = (currentVelocity - num * num7) * num3;
		float num8 = target + (num4 + num7) * num3;
		if (num5 - current > 0f == num8 > num5)
		{
			num8 = num5;
			currentVelocity = (num8 - num5) / deltaTime;
		}
		return num8;
	}

	/**
	* <summary>Moves one value towards another (eases in and out to destination with no overshoot)</summary>
	* 
	* @method LeanSmooth.damp (Vector3)
	* @param {float} current:Vector3 the current value
	* @param {float} target:Vector3 the value we are trying to reach
	* @param {float} currentVelocity:Vector3 the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @example
	* transform.position = LeanSmooth.damp(transform.position, destTrans.position, ref followVelocity, 1.1f);\n
	* Debug.Log("current:"+transform.position);
	*/
	public static Vector3 damp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		float x = damp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime);
		float y = damp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime);
		float z = damp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime);

		return new Vector3(x, y, z);
	}

	/**
	* <summary>Moves one color value towards another color (eases in and out to destination with no overshoot)</summary>
	* 
	* @method LeanSmooth.damp (Color)
	* @param {float} current:Color the current value
	* @param {float} target:Color the value we are trying to reach
	* @param {float} currentVelocity:Color the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @example
	* fromColor = LeanSmooth.damp(fromColor, transform.GetComponent<Renderer>().material.color, ref velocityColor, 1.1f);\n
	* Debug.Log("current:"+fromColor);
	*/
	public static Color damp(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		float r = damp(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime);
		float g = damp(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime);
		float b = damp(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime);
		float a = damp(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime);

		return new Color(r, g, b, a);
	}

	/**
	* <summary>Moves one value towards another (eases in and out to destination with possible overshoot bounciness)</summary>
	* 
	* @method LeanSmooth.spring (float)
	* @param {float} current:float the current value
	* @param {float} target:float the value we are trying to reach
	* @param {float} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @param {float} [friction]:float rate at which the spring is slowed down once it reaches it's destination
	* @param {float} [accelRate]:float the rate it accelerates from it's initial position
	* @example
	* followVar = LeanSmooth.spring(followVar, destinationVar, ref followVelocity, 1.1f);\n
	* Debug.Log("current:"+followVar);
	*/
	public static float spring(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		if (deltaTime < 0f)
			deltaTime = Time.deltaTime;

		float diff = target - current;

		currentVelocity += deltaTime / smoothTime * accelRate * diff;

		currentVelocity *= (1f - deltaTime * friction);

		if (maxSpeed > 0f && maxSpeed < Mathf.Abs(currentVelocity))
			currentVelocity = maxSpeed * Mathf.Sign(currentVelocity);

		float returned = current + currentVelocity;

		return returned;
	}

	/**
	* <summary>Moves one value towards another (eases in and out to destination with possible overshoot bounciness)</summary>
	* 
	* @method LeanSmooth.spring (Vector3)
	* @param {Vector3} current:float the current value
	* @param {Vector3} target:float the value we are trying to reach
	* @param {Vector3} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @param {float} [friction]:float rate at which the spring is slowed down once it reaches it's destination
	* @param {float} [accelRate]:float the rate it accelerates from it's initial position
	* @example
	* transform.position = LeanSmooth.spring(transform.position, destTrans.position, ref followVelocity, 1.1f);\n
	* Debug.Log("current:"+transform.position);
	*/
	public static Vector3 spring(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		float x = spring(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float y = spring(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float z = spring(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime, friction, accelRate);

		return new Vector3(x, y, z);
	}

	/**
	* <summary>Moves one color towards another (eases in and out to destination with possible overshoot bounciness)</summary>
	* 
	* @method LeanSmooth.spring (Color)
	* @param {Color} current:float the current value
	* @param {Color} target:float the value we are trying to reach
	* @param {Color} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @param {float} [friction]:float rate at which the spring is slowed down once it reaches it's destination
	* @param {float} [accelRate]:float the rate it accelerates from it's initial position
	* @example
	* fromColor = LeanSmooth.spring(fromColor, transform.GetComponent<Renderer>().material.color, ref velocityColor, 1.1f);\n
	* Debug.Log("current:"+fromColor);
	*/
	public static Color spring(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		float r = spring(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float g = spring(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float b = spring(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float a = spring(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime, friction, accelRate);

		return new Color(r, g, b, a);
	}

	/**
	* <summary>Moves one value towards another (at a constant speed)</summary>
	* 
	* @method LeanSmooth.linear (float)
	* @param {float} current:float the current value
	* @param {float} target:float the value we are trying to reach
	* @param {float} moveSpeed:float the speed at which to move towards the target
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @example
	* followVar = LeanSmooth.linear(followVar, destinationVar, 50f);\n
	* Debug.Log("current:"+followVar);
	*/
	public static float linear(float current, float target, float moveSpeed, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
			deltaTime = Time.deltaTime;

		bool targetGreater = (target > current);

		float currentVelocity = deltaTime * moveSpeed * (targetGreater ? 1f : -1f);

		float returned = current + currentVelocity;

		float returnPassed = returned - target;
		if ((targetGreater && returnPassed > 0) || !targetGreater && returnPassed < 0)
		{ // Has passed point, return target
			return target;
		}

		return returned;
	}

	/**
	* <summary>Moves one value towards another (at a constant speed)</summary>
	* 
	* @method LeanSmooth.linear (Vector3)
	* @param {Vector3} current:float the current value
	* @param {Vector3} target:float the value we are trying to reach
	* @param {float} moveSpeed:float the speed at which to move towards the target
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @example
	* transform.position = LeanSmooth.linear(transform.position, followTrans.position, 50f);\n
	* Debug.Log("current:"+transform.position);
	*/
	public static Vector3 linear(Vector3 current, Vector3 target, float moveSpeed, float deltaTime = -1f)
	{
		float x = linear(current.x, target.x, moveSpeed, deltaTime);
		float y = linear(current.y, target.y, moveSpeed, deltaTime);
		float z = linear(current.z, target.z, moveSpeed, deltaTime);

		return new Vector3(x, y, z);
	}

	/**
	* <summary>Moves one color towards another (at a constant speed)</summary>
	* 
	* @method LeanSmooth.linear (Color)
	* @param {Color} current:float the current value
	* @param {Color} target:float the value we are trying to reach
	* @param {float} moveSpeed:float the speed at which to move towards the target
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @example
	* fromColor = LeanSmooth.linear(fromColor, transform.GetComponent<Renderer>().material.color, 50f);\n
	* Debug.Log("current:"+fromColor);
	*/
	public static Color linear(Color current, Color target, float moveSpeed)
	{
		float r = linear(current.r, target.r, moveSpeed);
		float g = linear(current.g, target.g, moveSpeed);
		float b = linear(current.b, target.b, moveSpeed);
		float a = linear(current.a, target.a, moveSpeed);

		return new Color(r, g, b, a);
	}

	/**
	* <summary>Moves one value towards another (with an ease that bounces back some when it reaches it's destination)</summary>
	* 
	* @method LeanSmooth.bounceOut (float)
	* @param {float} current:float the current value
	* @param {float} target:float the value we are trying to reach
	* @param {float} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @param {float} [friction]:float rate at which the spring is slowed down once it reaches it's destination
	* @param {float} [accelRate]:float the rate it accelerates from it's initial position
	* @param {float} [hitDamping]:float the rate at which to dampen the bounciness of when it reaches it's destination
	* @example
	* followVar = LeanSmooth.bounceOut(followVar, destinationVar, ref followVelocity, 1.1f);\n
	* Debug.Log("current:"+followVar);
	*/
	public static float bounceOut(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		if (deltaTime < 0f)
			deltaTime = Time.deltaTime;

		float diff = target - current;

		currentVelocity += deltaTime / smoothTime * accelRate * diff;

		currentVelocity *= (1f - deltaTime * friction);

		if (maxSpeed > 0f && maxSpeed < Mathf.Abs(currentVelocity))
			currentVelocity = maxSpeed * Mathf.Sign(currentVelocity);

		float returned = current + currentVelocity;

		bool targetGreater = (target > current);
		float returnPassed = returned - target;
		if ((targetGreater && returnPassed > 0) || !targetGreater && returnPassed < 0)
		{ // Start a bounce
			currentVelocity = -currentVelocity * hitDamping;
			returned = current + currentVelocity;
		}

		return returned;
	}

	/**
	* <summary>Moves one value towards another (with an ease that bounces back some when it reaches it's destination)</summary>
	* 
	* @method LeanSmooth.bounceOut (Vector3)
	* @param {Vector3} current:float the current value
	* @param {Vector3} target:float the value we are trying to reach
	* @param {Vector3} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @param {float} [friction]:float rate at which the spring is slowed down once it reaches it's destination
	* @param {float} [accelRate]:float the rate it accelerates from it's initial position
	* @param {float} [hitDamping]:float the rate at which to dampen the bounciness of when it reaches it's destination
	* @example
	* transform.position = LeanSmooth.bounceOut(transform.position, followTrans.position, ref followVelocity, 1.1f);\n
	* Debug.Log("current:"+transform.position);
	*/
	public static Vector3 bounceOut(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		float x = bounceOut(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float y = bounceOut(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float z = bounceOut(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);

		return new Vector3(x, y, z);
	}

	/**
	* <summary>Moves one color towards another (with an ease that bounces back some when it reaches it's destination)</summary>
	* 
	* @method LeanSmooth.bounceOut (Color)
	* @param {Color} current:float the current value
	* @param {Color} target:float the value we are trying to reach
	* @param {Color} currentVelocity:float the current velocity of the value
	* @param {float} smoothTime:float roughly the time it takes to reach the destination
	* @param {float} maxSpeed:float the top speed you want the value to move at (defaults to unlimited -1f)
	* @param {float} deltaTime:float the difference in time since the method was called (defaults to Time.deltaTime)
	* @param {float} [friction]:float rate at which the spring is slowed down once it reaches it's destination
	* @param {float} [accelRate]:float the rate it accelerates from it's initial position
	* @param {float} [hitDamping]:float the rate at which to dampen the bounciness of when it reaches it's destination
	* @example
	* fromColor = LeanSmooth.bounceOut(fromColor, transform.GetComponent<Renderer>().material.color, ref followVelocity, 1.1f);\n
	* Debug.Log("current:" + fromColor);
	*/
	public static Color bounceOut(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		float r = bounceOut(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float g = bounceOut(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float b = bounceOut(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float a = bounceOut(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);

		return new Color(r, g, b, a);
	}
}
