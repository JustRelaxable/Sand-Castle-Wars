using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shakeDuration > 0)
        {
            Shake();

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            ResetLocation();
        }
    }

    private void ResetLocation()
    {
        camTransform.localPosition = originalPos;
    }

    private void Shake()
    {
        camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    }

    public IEnumerator ShakeForSeconds(float seconds)
    {
		float shakeSeconds = seconds;
		float currentTime = 0f;
        while (currentTime<=shakeSeconds)
        {
			Shake();
			currentTime += Time.deltaTime;
			yield return null;
        }
		ResetLocation();
    }

	public void StartShake()
    {
		shakeDuration = int.MaxValue;
    }

	public void StopShake()
    {
		shakeDuration = 0;
    }
}