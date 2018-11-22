using UnityEngine;

public enum PickUpKind
{
	Blue = 0,
	Green = 1,
	Yellow = 2,
	Key = 3
}

[System.Serializable]
public class PickupData
{
	public PickUpKind type;     // тип пикапа
	public int targetAmount;	// сколько пикапов нужно собрать
	public int LeftAmount {		// сколько пикапов осталось собрать
		get {
			return targetAmount - collectedAmount;
		}
	}
	public int CollectedAmount {	// для вывода в UI
		get {
			if(collectedAmount <= targetAmount)	return collectedAmount;
			return targetAmount;
		}
	}
	public AudioClip collectClip;	// звук для подобранного пикапа
	public GameObject[] fences;	// оградки, зависящие от данного вида пикапа

	private int collectedAmount;	// сколько пикапов собрано

	public void CollectPickup() {
		collectedAmount++;	// собираем пикап
		if (LeftAmount <= 0)	// если осталось собрать <= 0
			CrashFences();		// рушим оградки
	}

	void CrashFences() {
		foreach (var fence in fences)
			fence.SetActive(false);
	}
}
