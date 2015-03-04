using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

	public GameManager gameManager;
	private GameManager _gameManger;
	private bool start = true;
	
	void OnGUI() {
		if(start) {
			if(GUI.Button(new Rect (10, 10, 100, 50), "start")) {
				start = false;
				_gameManger = Instantiate(gameManager) as GameManager;
			}
		}
		if(!start) {
			if(GUI.Button (new Rect (10, 70, 100, 50), "restart")) {
				if(_gameManger != null) {
					Destroy(_gameManger.gameObject);

					print("Destroy(_gameManger.gameObject);");
				}
				_gameManger = Instantiate(gameManager) as GameManager;
			}
		
		}
	}
}
