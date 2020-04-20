using UnityEngine;
using System;

public abstract class Screen : MonoBehaviour
{ 
	public abstract ScreenType ScreenType { get; }

	public virtual void Open()
	{
		gameObject.SetActive(true);
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
	}
}


public enum ScreenType
{
	GameOver,
	WinGame,
	Pause
}
