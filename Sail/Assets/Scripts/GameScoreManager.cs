using UnityEngine;
using System.Collections.Generic;

public enum PowerupMode
{
	NONE,
	BOOST,
	SLOW,
	CANNON,
	FEVER
}

public class GameScoreManager : MonoBehaviour
{
	static GameScoreManager _instance;
	public static GameScoreManager instance
	{
		get {
			return _instance;
		}
	}

	public int Life
	{
		get {
			return _life;
		}

		set {
			_life = value;
			BroadcastLifeUpdate();
		}
	}

	public int Score
	{
		get {
			return _score;
		}

		set {
			_score = value;
			BroadcastScoreUpdate();
		}
	}

	public int Coin
	{
		get {
			return _coin;
		}

		set {
			_coin = value;
			BroadcastCoinUpdate();
		}
	}

	public float Direction
	{
		get {
			return _direction;
		}

		set {
			_direction = value;
			BroadcastDirectionUpdate();
		}
	}

	public float TravelDistance
	{
		get {
			return _travelDistance;
		}

		set {
			_travelDistance = value;
			BroadcastTravelDistanceUpdate();
		}
	}

	public PowerupMode Powerup
	{
		get {
			return _powerUpMode;
		}

		set {
			_powerUpMode = value;
			BroadcastPowerModeUpdate();
		}
	}

	public delegate void TravelDistanceUpdateCallback(float travelDistance);
	public event TravelDistanceUpdateCallback OnTravelDistanceUpdate;

	public delegate void LifeUpdateCallback(int life);
	public event LifeUpdateCallback OnLifeUpdate;

	public delegate void ScoreUpdateCallback(int score);
	public event ScoreUpdateCallback OnScoreUpdate;

	public delegate void CoinUpdateCallback(int coin);
	public event CoinUpdateCallback OnCoinUpdate;	

	public delegate void DirectionUpdateCallback(float dir);
	public event DirectionUpdateCallback OnDirectionUpdate;

	public delegate void PowerupModeUpdateCallback(PowerupMode powerupMode);
	public event PowerupModeUpdateCallback OnPowerupUpdate;

	float _travelDistance;
	int _life;
	int _score;
	int _coin;
	float _direction;
	PowerupMode _powerUpMode;

	public UIGameOverScreen gameOverScreen;
	public GameObject player;

	public int INIT_LIFE = 10;

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		_travelDistance = 0;
		_life = INIT_LIFE;
		_score = 0;
		_coin = 0;
		_direction = 0;
		_powerUpMode = PowerupMode.NONE;

		gameOverScreen.gameObject.SetActive(false);

		BroadcastTravelDistanceUpdate();
		BroadcastLifeUpdate();
		BroadcastScoreUpdate();
		BroadcastCoinUpdate();
		BroadcastDirectionUpdate();
		BroadcastPowerModeUpdate();

	}

	void BroadcastLifeUpdate()
	{
		if (OnLifeUpdate != null)
			OnLifeUpdate(_life);

		if (_life <= 0)
		{
			player.SetActive(false);
			gameOverScreen.gameObject.SetActive(true);
			gameOverScreen.Refresh();
		}
	}

	void BroadcastScoreUpdate()
	{
		if (OnScoreUpdate != null)
			OnScoreUpdate(_score);
	}

	void BroadcastCoinUpdate()
	{
		if (OnCoinUpdate != null)
			OnCoinUpdate(_coin);
	}

	void BroadcastDirectionUpdate()
	{
		if (OnDirectionUpdate != null)
			OnDirectionUpdate(_direction);
	}

	void BroadcastPowerModeUpdate()
	{
		if (OnPowerupUpdate != null)
			OnPowerupUpdate(_powerUpMode);
	}

	void BroadcastTravelDistanceUpdate()
	{
		if (OnTravelDistanceUpdate != null)
			OnTravelDistanceUpdate(_travelDistance);
	}



}