using UnityEngine;
using PitchDetector;

public class ShipSoundController : MonoBehaviour
{
	public enum TurningPolicy
	{
		DIRECT,
		DIRECT_SILENCE_TO_NEUTRAL,
		DIFFERENTIAL,
		DIFFERENTIAL_SILENT_TO_NEUTRAL
	}
	Detector pitchDetector; 
	public int pitchTimeInterval=100;
	private int minFreq, maxFreq;
	// float[] waveData;
	Quaternion cachedTargetRotation;

	public TurningPolicy turningPolicy = TurningPolicy.DIRECT;

	float accumTime;
	// public float DETECT_INTERVAL = 0.1f;
	public float SPEED = 10.0f;

	int lastNormalizedNote;
	int lastMidiNote;

	void Awake()
	{
		pitchDetector = new Detector();
		pitchDetector.setSampleRate(AudioSettings.outputSampleRate);
		lastNormalizedNote = 0;
	}

	void SetuptMic() {
		//GetComponent<AudioSource>().volume = 0f;
		GetComponent<AudioSource>().clip = null;
		GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
		GetComponent<AudioSource>().mute = false; // Mute the sound, we don't want the player to hear it
		StartMicrophone();
	}

	void StartMicrophone () {
        Debug.Log("Setting up mic");
        Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);//Gets the frequency of the device
		if ((minFreq + maxFreq) == 0)
			maxFreq = 44100;

		GetComponent<AudioSource>().clip = Microphone.Start(null, true, 10, maxFreq);//Starts recording
		while (!(Microphone.GetPosition(null) > 0)){} // Wait until the recording has started
		GetComponent<AudioSource>().Play(); // Play the audio source!
        Debug.Log("Setup done");
    }

	void Start()
	{
		int bufferLen = (int)Mathf.Round (AudioSettings.outputSampleRate * pitchTimeInterval / 1000f);
		Debug.Log ("Buffer len: " + bufferLen);
		// waveData = new float[bufferLen];

		SetuptMic();

		accumTime = 0;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		pitchDetector.DetectPitch(data);
		int midiNote = pitchDetector.lastMidiNote();
		string note = pitchDetector.lastNote();
		float freq = pitchDetector.lastFrequency();

		if (midiNote > 0)
		{
			int normalizedNote = midiNote - 50;
			normalizedNote = (int)Mathf.Clamp(normalizedNote, -4, 4);

			if (turningPolicy == TurningPolicy.DIRECT ||
				turningPolicy == TurningPolicy.DIRECT_SILENCE_TO_NEUTRAL)
			{
				cachedTargetRotation.eulerAngles = new Vector3(0, 0, -normalizedNote * 22.5f);
			}

			lastNormalizedNote = normalizedNote;
			
			// transform.rotation = rot;
			// Debug.Log("Detected note: " + midiNote);	
			Debug.Log("Detected freq: " + freq + " - Note:  " + midiNote);
		}
		else
		{
			if (turningPolicy == TurningPolicy.DIRECT_SILENCE_TO_NEUTRAL)
			{
				cachedTargetRotation.eulerAngles = Vector3.zero;
			}

			lastNormalizedNote = 0;
		}

		lastMidiNote = midiNote;
	}

	void Update()
	{
		// if (accumTime > DETECT_INTERVAL )
		// {

		// 	GetComponent<AudioSource>().GetOutputData(waveData, 0);
		// 	pitchDetector.DetectPitch(waveData);
		// 	int midiNote = pitchDetector.lastMidiNote();
		// 	string note = pitchDetector.lastNote();
		// 	float freq = pitchDetector.lastFrequency();
			

		// 	if (midiNote > 0)
		// 	{
		// 		int normalizedNote = midiNote - 50;
		// 		normalizedNote = (int)Mathf.Clamp(normalizedNote, -4, 4);
		// 		cachedTargetRotation.eulerAngles = new Vector3(0, 0, -normalizedNote * 22.5f);
		// 		// transform.rotation = rot;
		// 		// Debug.Log("Detected note: " + midiNote);	
		// 		Debug.Log("Detected freq: " + freq + " - Note:  " + midiNote);
		// 	}
		// 	accumTime -= DETECT_INTERVAL;
		// }

		if (turningPolicy == TurningPolicy.DIRECT ||
			turningPolicy == TurningPolicy.DIRECT_SILENCE_TO_NEUTRAL)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, cachedTargetRotation, 0.05f);
		}
		if (turningPolicy == TurningPolicy.DIFFERENTIAL)
		{
			Quaternion currRotation = transform.rotation;
			transform.rotation = currRotation * Quaternion.AngleAxis(Time.deltaTime * -lastNormalizedNote * 22.5f, Vector3.forward);

			Vector3 eulerAngles = transform.rotation.eulerAngles;
			if (eulerAngles.z > 180)
				eulerAngles.z -= 360;

			eulerAngles.z = Mathf.Clamp(eulerAngles.z, -90.0f, 90.0f);

			Quaternion rot = Quaternion.identity;
			rot.eulerAngles = eulerAngles;
			transform.rotation = rot;
		}
		if (turningPolicy == TurningPolicy.DIFFERENTIAL_SILENT_TO_NEUTRAL)
		{
			if (lastMidiNote == 0)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.05f);
			}
			else
			{
				Quaternion currRotation = transform.rotation;
				transform.rotation = currRotation * Quaternion.AngleAxis(Time.deltaTime * -lastNormalizedNote * 22.5f, Vector3.forward);

				Vector3 eulerAngles = transform.rotation.eulerAngles;
				if (eulerAngles.z > 180)
					eulerAngles.z -= 360;

				eulerAngles.z = Mathf.Clamp(eulerAngles.z, -90.0f, 90.0f);
				Quaternion rot = Quaternion.identity;
				rot.eulerAngles = eulerAngles;
				transform.rotation = rot;
			}
		}


		transform.position = transform.position + 
			transform.up * SPEED * Time.deltaTime;

		accumTime += Time.deltaTime;
	}
}