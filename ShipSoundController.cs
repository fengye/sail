using UnityEngine;
using PitchDetector;

public class ShipSoundController : MonoBehaviour
{
	Detector pitchDetector; 
	public int pitchTimeInterval=100;
	private int minFreq, maxFreq;
	float[] waveData;
	Quaternion cachedTargetRotation;

	float accumTime;
	public float DETECT_INTERVAL = 0.1f;
	public float SPEED = 10.0f;

	void Awake()
	{
		pitchDetector = new Detector();
		pitchDetector.setSampleRate(AudioSettings.outputSampleRate);
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
		waveData = new float[bufferLen];

		SetuptMic();

		accumTime = 0;
	}

	void Update()
	{
		if (accumTime > DETECT_INTERVAL )
		{

			GetComponent<AudioSource>().GetOutputData(waveData, 0);
			pitchDetector.DetectPitch(waveData);
			int midiNote = pitchDetector.lastMidiNote();
			string note = pitchDetector.lastNote();
			

			if (midiNote > 0)
			{
				int normalizedNote = midiNote - 50;
				normalizedNote = (int)Mathf.Clamp(normalizedNote, -4, 4);
				cachedTargetRotation.eulerAngles = new Vector3(0, normalizedNote * 22.5f, 0);
				// transform.rotation = rot;

				Debug.Log("Detected note: " + midiNote);	
			}



			accumTime -= DETECT_INTERVAL;
		}

		transform.rotation = Quaternion.Slerp(transform.rotation, cachedTargetRotation, 0.05f);

		transform.position = transform.position + 
			transform.forward * SPEED * Time.deltaTime;

		accumTime += Time.deltaTime;
	}
}