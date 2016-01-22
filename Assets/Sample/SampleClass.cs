using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PrefsManager;

public class SampleClass : MonoBehaviour {

	public InputField keyInput;
	public InputField valInput;
	public Text output;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OnSave () {
		UserInfo.Save<string> (keyInput.text, valInput.text);
		Debug.Log ("Save");
	}

	public void OnLoad () {
		string val = UserInfo.Load<string> (keyInput.text);
		output.text = val;
		Debug.Log ("Load ; "+val);
	}

	public void OnDeleteKey () {
		bool flg = UserInfo.DeleteKey (keyInput.text);
		Debug.Log ("DeleteKey : "+flg);
	}

	public void OnDeleteAll () {
		bool flg = UserInfo.DeleteAll ();
		Debug.Log ("DeleteKey : "+flg);
	}
}