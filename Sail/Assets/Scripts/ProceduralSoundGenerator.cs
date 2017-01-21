using UnityEngine;
using System.Collections;
using System;  // Needed for Math

public class ProceduralSoundGenerator : MonoBehaviour {

	// un-optimized version
  public double baseFrequency = 440;
  public double gain = 0.05;
  public float turningSpeed = 1.0f;
  public int initOffsetVal = 5;

  readonly double A = 1.059463094359;

  private double increment;
  private double phase;
  private double samplingFreq = 22050; // fallback value

  private double currFreq;

  private float offsetVal;
  private float accumTime;

  private int dataPhase;

  void Start()
  {
  	samplingFreq = AudioSettings.outputSampleRate;
    currFreq = baseFrequency;

    offsetVal = initOffsetVal;
    accumTime = 0;
    dataPhase = 0;
  }

  // actual procedural generation happens here
  void OnAudioFilterRead(float[] data, int channels)
  {
#if true
    // update increment in case frequency has changed
    increment = currFreq * 2 * Math.PI / samplingFreq;
    for (var i = 0; i < data.Length; i = i + channels)
    {
      phase = phase + increment;
    // this is where we copy audio data to make them “available” to Unity
      data[i] = (float)(gain*Math.Sin(phase));
    // if we have stereo, we copy the mono data to each channel
      if (channels == 2) 
        data[i + 1] = data[i];
      if (phase > 2 * Math.PI) 
        phase -= 2 * Math.PI;
    }
#else
    // update increment in case frequency has changed
    increment = samplingFreq / currFreq;
    for (var i = 0; i < data.Length; i = i + channels)
    {
      int idx = (int)((dataPhase + i) / increment);
      if (idx % 2 == 0)
      {
        data[i] = (float)gain;
      }
      else
      {
        data[i] = -(float)gain; 
      }
      if (channels == 2) data[i + 1] = data[i];
      // if (phase > 2 * Math.PI) phase = 0;
    }
#endif
    dataPhase += data.Length;

  }

  void Update()
  {
    // accumTime += Time.deltaTime;
    // if (accumTime > 2.0f)
    // {
    //   offsetVal += 1;
    //   SetNoteOffset(offsetVal);

    //   accumTime = 0;
    // }

    if (Input.GetKey(KeyCode.LeftArrow))
    {
      offsetVal -= Time.deltaTime * turningSpeed;
    }
    if (Input.GetKey(KeyCode.RightArrow))
    {
      offsetVal += Time.deltaTime * turningSpeed;
    }

    offsetVal = Mathf.Clamp(offsetVal, 0, 12);

    // cast to int to have much chunky tone
    SetNoteOffset((int)offsetVal);
  }

  public void SetNoteOffset(float offset)
  {
    currFreq = baseFrequency * Math.Pow(A, offset);
    Debug.Log("Current freq offset: " + offset + " Freq: " + currFreq);
  }

}
