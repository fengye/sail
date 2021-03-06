using UnityEngine;
using System.Collections;
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

	public enum SamplingPolicy
	{
		FREQUENCY_LINEAR,
		OCTAVE_TURN_AROUND
	}

	Detector pitchDetector; 
	public int pitchTimeInterval=100;
	private int minFreq, maxFreq;
	int lastNormalizedNote;
	int lastMidiNote;
	float lastLoudness;
	float lastLoudnessDetectedTime;
	float currTimestamp;

	Quaternion cachedTargetRotation;
	public Quaternion CachedTargetRotation
	{
		get {
			return cachedTargetRotation;
		}
	}

	public Transform soundPlayerPrefab;
	public AudioClip hitSound;
	public AudioClip coinSound;

	public TurningPolicy turningPolicy = TurningPolicy.DIRECT;
	public SamplingPolicy samplingPolicy = SamplingPolicy.OCTAVE_TURN_AROUND;

	float accumTime;
	public float SPEED = 10.0f;
	private float speedMult = 1;

	public float LOUDNESS_THRESHOLD = 0.01f;
	public float LOUDNESS_DELTA_THRESHOLD = 0.01f;

	public Sprite[] sprites;
	int currentSprite = 0;

	private SpriteRenderer sprite;
	private Collider2D collider;

	public Cannon cannon;

	void Awake()
	{
		pitchDetector = new Detector();
		pitchDetector.setSampleRate(AudioSettings.outputSampleRate);
		lastNormalizedNote = 0;
		lastLoudness = 0;
		lastLoudnessDetectedTime = Time.realtimeSinceStartup;
	}

	void SetuptMic() {
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

		SetuptMic();

		collider = GetComponent<BoxCollider2D> ();
		sprite = GetComponentInChildren<SpriteRenderer> ();

		cannon.EnableCannon(true);
		accumTime = 0;

		GameScoreManager.instance.OnLifeUpdate += OnLifeUpdated;
		GameScoreManager.instance.TravelDistance = transform.position.y;
	}

	void OnDestroy()
	{
		GameScoreManager.instance.OnLifeUpdate -= OnLifeUpdated;	
	}

	static float CalculateLoudness(float[] data)
	{
		// loudness detection
		float result = 0;
		for(int i = 0; i < data.Length; ++i)
		{
			result += data[i];
		}

		result = result / data.Length;
		return result;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		float loudness = CalculateLoudness(data);
		float deltaLoudness = Mathf.Abs(loudness - lastLoudness);

		if (loudness > LOUDNESS_THRESHOLD && deltaLoudness > LOUDNESS_DELTA_THRESHOLD && currTimestamp > lastLoudnessDetectedTime + 1.0f)
		{
			cannon.Fire();
			lastLoudnessDetectedTime = currTimestamp;
		}

		// Debug.Log(string.Format("Loudness: {0:0.0000}, delta: {1:0.0000}", loudness, deltaLoudness));

		pitchDetector.DetectPitch(data);
		int midiNote = pitchDetector.lastMidiNote();
		// string note = pitchDetector.lastNote();
		// float freq = pitchDetector.lastFrequency();

		if (midiNote > 0)
		{
			int normalizedNote = 0;
			float dirSegment = 22.5f;
			if (samplingPolicy == SamplingPolicy.OCTAVE_TURN_AROUND)
			{
				int octave = midiNote / 12;
				int noteNum = midiNote % 12;

				
				if (octave % 2 == 0)
				{
					normalizedNote = noteNum;
				}
				else
				{
					normalizedNote = (12 - 1) - noteNum;
				}

				// normalizedNote range == [0, 11]
				// int normalizedNote = midiNote - 50;
				// normalizedNote = (int)Mathf.Clamp(normalizedNote, -4, 4);
				normalizedNote -= 6;

				dirSegment = 15.0f;
			}
			else
			{
				normalizedNote = midiNote - 50;
				normalizedNote = (int)Mathf.Clamp(normalizedNote, -4, 4);

				dirSegment = 22.5f;
			}
			

			if (turningPolicy == TurningPolicy.DIRECT ||
				turningPolicy == TurningPolicy.DIRECT_SILENCE_TO_NEUTRAL)
			{
				cachedTargetRotation.eulerAngles = new Vector3(0, 0, -normalizedNote * dirSegment);
			}

			lastNormalizedNote = normalizedNote;
			
			// transform.rotation = rot;
			// Debug.Log("Detected note: " + midiNote);	
			// Debug.Log("Detected freq: " + freq + " - Note:  " + midiNote);
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
		lastLoudness = loudness;
	}

	void Update()
	{
		currTimestamp = Time.realtimeSinceStartup;

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
			transform.up * SPEED * speedMult * Time.deltaTime;

		GameScoreManager.instance.TravelDistance = transform.position.y;

		accumTime += Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		switch(other.gameObject.tag) {
		case "Coin":
			Debug.Log ("it's a coin!");
			IncrementScore(10);

			GameScoreManager.instance.Coin += 10;

			PlayCoinSound();

			Destroy (other.gameObject);
			break;
		case "Slow":
			Debug.Log ("speed drain/slow: .5x speed");
			IncrementScore(5);
			speedMult *= 0.5f;
			Destroy (other.gameObject);
			StartCoroutine(ResetSpeedMult(10f, .5f));

			GameScoreManager.instance.Powerup = PowerupMode.SLOW;
			break;
		case "Boost":
			Debug.Log ("speed boost: 2x speed");
			IncrementScore(5);
			speedMult *= 2f;
			Destroy (other.gameObject);
			StartCoroutine(ResetSpeedMult(10f, 2f));

			GameScoreManager.instance.Powerup = PowerupMode.BOOST;


			break;
		case "Shot":
			Debug.Log("canons loaded");
			// do thing here
			Destroy (other.gameObject);

			GameScoreManager.instance.Powerup = PowerupMode.CANNON;
			break;
		case "Fever":
			Debug.Log ("fever mode?");
			IncrementScore(100);
			Destroy (other.gameObject);

			GameScoreManager.instance.Powerup = PowerupMode.FEVER;
			break;
		}
	}

	void IncrementScore(int delta)
	{
		GameScoreManager.instance.Score = GameScoreManager.instance.Score + delta;
	}

	void UpdateLife(int delta)
	{
		GameScoreManager.instance.Life = GameScoreManager.instance.Life + delta;
	}

	void OnLifeUpdated(int life)
	{
		if (life < 5)
		{
			currentSprite = 2;
			sprite.sprite = sprites [currentSprite];
		}
	}

	void PlayWreckSound()
	{
		GameObject obj = ((Transform)Instantiate(soundPlayerPrefab, transform.position, transform.rotation)).gameObject;
		AudioSource audio = obj.GetComponent<AudioSource>();
		audio.clip = hitSound;
		audio.Play();
	}

	void PlayCoinSound()
	{
		GameObject obj = ((Transform)Instantiate(soundPlayerPrefab, transform.position, transform.rotation)).gameObject;
		AudioSource audio = obj.GetComponent<AudioSource>();
		audio.clip = coinSound;
		audio.Play();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("collision");
		UpdateLife(-1);

		

		Debug.Log ("life: " + GameScoreManager.instance.Life);
		// if(GameScoreManager.instance.Life <= 5) {
		// 	currentSprite = 2;
		// 	sprite.sprite = sprites [currentSprite];
		// }
		transform.position = Vector3.MoveTowards(transform.position, coll.contacts[0].normal, 2);
		transform.Rotate (0, 0, Random.Range(-30, 30));
		if (coll.gameObject.CompareTag ("Obstacle")) {

			PlayWreckSound();

			// may destroy the rock
			Destroy(coll.gameObject);
			StartCoroutine (CollidedWithRock ());
		}
	}

	IEnumerator ResetSpeedMult(float delay, float inc) {
		yield return new WaitForSeconds (delay);
		Debug.Log ("speed mult reset");
		speedMult /= inc;

		GameScoreManager.instance.Powerup = PowerupMode.NONE;
	}

	IEnumerator CollidedWithRock() {
		GameObject child = transform.GetChild (0).gameObject;
		sprite.sprite = sprites [currentSprite+1];
		collider.enabled = false;
		SPEED = 1f;
		for(int i = 1; i < 15; i++) {
			Flash (child);
			yield return new WaitForSeconds (0.1f);
			if(SPEED < 10) { //&& (i % 2) == 0) {
				SPEED += 1;
			}
		}
		sprite.sprite = sprites [currentSprite];
		child.SetActive (true);
		collider.enabled = true;
	}

	void Flash(GameObject obj) {
		if(obj.activeSelf) {
			obj.SetActive (false);
		} else {
			obj.SetActive (true);
		}
	}
}