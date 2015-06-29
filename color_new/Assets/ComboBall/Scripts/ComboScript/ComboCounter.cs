/// <summary>
/// Combo Counter
/// Include details of combos:
/// 1. Color
/// 2. Number of balls
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboDetails
{
	public ComboBall.BallColors color;
	public int numberOfBalls = 0;

	public void Reset()
	{
		numberOfBalls = 0;
	}
}

public class ComboCounter : MonoBehaviour 
{
	public static ComboCounter Instance;
	void Awake()
	{
		Instance = this;
		comboDetails = new ComboDetails();
	}

	private int comboCount = 0;
	private ComboDetails comboDetails;
	public TextMesh comboCountText;
	public TextMesh comboColorText;
	public TextMesh comboBallCountText;

	public void AddCombo(ComboBall.BallColors color, int numOfBalls)
	{
		comboCount++;
		comboDetails.color = color;
		comboDetails.numberOfBalls = numOfBalls;
		comboCountText.text = string.Format("Combo Count: {0}", comboCount);
		comboColorText.text = string.Format("Color:\t\t{0}", comboDetails.color);
		switch(comboDetails.color)
		{
		case ComboBall.BallColors.BLUE:
			comboColorText.color = Color.blue;
			break;
		case ComboBall.BallColors.GREEN:
			comboColorText.color = Color.green;
			break;
		case ComboBall.BallColors.PINK:
			comboColorText.color = new Color(1.0f, 95.0f/255.0f, 246.0f/255.0f);
			break;
		case ComboBall.BallColors.PURPLE:
			comboColorText.color = new Color(101.0f/255.0f, 0.0f, 126.0f/255.0f);
			break;
		case ComboBall.BallColors.RED:
			comboColorText.color = Color.red;
			break;
		case ComboBall.BallColors.YELLOW:
			comboColorText.color = Color.yellow;
			break;
		default:

			break;
		}
		comboBallCountText.text = string.Format("Ball Count:\t\t{0}", comboDetails.numberOfBalls);
	}

	public void ResetComboCounter()
	{
		comboCount = 0;
		comboDetails.Reset();

	}

}
