using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject linternaObj, pistolaObj;

	private enum Objetos {Mano, Linterna, Pistola};
	private Objetos activeObject;
	private bool hasLinterna;
	private bool hasPistola;

	public int hp = 100;
	public Text pick;
	public Image crosshair;
	public string basePick = "<b>Recoger";
	private string buttonText = "\n Pulsa B</b>";
	private float catchDistance = 3f;
	private Animator animator;
	private GameObject gameController;


	//Sonido
	private AudioSource source;
	public AudioClip linternaOn;
	public AudioClip linternaOff;
	public AudioClip pistolaOn;
	public AudioClip pistolaOff;



	// Use this for initialization
	void Start () {

		source = GetComponent<AudioSource> ();

		//animator = GetComponentInChildren<Animator>();
		//rightArm = GameObject.FindGameObjectWithTag("rightArm");
		gameController = GameObject.FindGameObjectWithTag("GameController");
		activeObject = Objetos.Mano;
		crosshair.enabled = false;
	}

	// Update is called once per frame
	void FixedUpdate () {

		//COGER UN OBJETO
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // al medio de la pantalla
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, catchDistance))
		{
			GameObject selectedObject = hit.collider.gameObject;

			//OBJETO COLLECTABLE 
			if (selectedObject.tag.Equals ("Collectable")) {
				//SI esta mirando a un objeto coleccionable muestra feedback
				pick.enabled = true;
				pick.text = basePick + " " + selectedObject.name + buttonText;
				//SI pulsa B lo cogemos
				if (OVRInput.GetDown (OVRInput.Button.Two)/*DEBUG CONTROLL*/ || Input.GetButtonDown ("Fire1")) {
					switch (selectedObject.name) {
					case "Linterna":
						hasLinterna = true;
						activeObject = Objetos.Linterna;
						crosshair.enabled = false;
						pistolaObj.SetActive (false);
						linternaObj.SetActive (true);
						linternaObj.GetComponent<AudioSource>().PlayOneShot(linternaOn, Random.Range(0.6f, 1f)); //Se reproduce una vez el sonido linternaOff con un volumen entre 0.6 y 1
						break;
					case "Pistola":
						hasPistola = true;
						crosshair.enabled = true;
						activeObject = Objetos.Pistola;
						linternaObj.SetActive (false);
						pistolaObj.SetActive (true);
						pistolaObj.GetComponent<AudioSource>().PlayOneShot(pistolaOn, Random.Range(0.6f, 1f)); //Se reproduce una vez el sonido pistolaOff con un volumen entre 0.6 y 1
						break;
					}
					Destroy (selectedObject);
				}
			} else {
				pick.enabled = false;
			}

			//PUERTA
			if (selectedObject.tag.Equals ("Door")) {
				if (OVRInput.GetDown (OVRInput.Button.Two)/*DEBUG CONTROLL*/ || Input.GetButtonDown ("Fire1"))
					selectedObject.SendMessage ("Toogle");
			}
		}
		else
			pick.enabled = false;

		//TOOGLE LINTERNA
		if (hasLinterna && (Input.GetKeyDown(KeyCode.E) || OVRInput.GetDown(OVRInput.Button.Three)) /* X */) {

			if (this.activeObject == Objetos.Linterna) {
				activeObject = Objetos.Mano;
				linternaObj.SetActive (false);
				linternaObj.GetComponent<AudioSource>().PlayOneShot(linternaOff, Random.Range(0.6f, 1f)); //Se reproduce una vez el sonido linternaOn con un volumen entre 0.6 y 1
			} 
			else {
				activeObject = Objetos.Linterna;
				crosshair.enabled = false;
				pistolaObj.SetActive (false);
				linternaObj.SetActive (true);
				linternaObj.GetComponent<AudioSource>().PlayOneShot(linternaOn, Random.Range(0.6f, 1f)); //Se reproduce una vez el sonido linternaOff con un volumen entre 0.6 y 1

			}
		}

		//TOOGLE PISTOLA
		if (hasPistola && (Input.GetKeyDown(KeyCode.Q) || OVRInput.GetDown(OVRInput.Button.Four)) /* Y */) {

			if (this.activeObject == Objetos.Pistola) {
				crosshair.enabled = false;
				activeObject = Objetos.Mano;
				pistolaObj.SetActive (false);
				pistolaObj.GetComponent<AudioSource>().PlayOneShot(pistolaOff, Random.Range(0.6f, 1f)); //Se reproduce una vez el sonido pistolaOn con un volumen entre 0.6 y 1

			} 
			else {
				crosshair.enabled = true;
				activeObject = Objetos.Pistola;
				linternaObj.SetActive (false);
				pistolaObj.SetActive (true);
				pistolaObj.GetComponent<AudioSource>().PlayOneShot(pistolaOn, Random.Range(0.6f, 1f)); //Se reproduce una vez el sonido pistolaOff con un volumen entre 0.6 y 1

			}
		}

	}

	public void Damage(int value)
	{
		hp -= value;
		if (hp == 0) {
			gameController.SendMessage ("GameOver");
		}
	}


}
