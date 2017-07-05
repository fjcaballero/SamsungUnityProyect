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
    private GameObject rightArm;

	// Use this for initialization
	void Start () {
        //animator = GetComponentInChildren<Animator>();
        //rightArm = GameObject.FindGameObjectWithTag("rightArm");
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
            if (selectedObject.tag.Equals("Collectable"))
            {
                //SI esta mirando a un objeto coleccionable muestra feedback
                pick.enabled = true;
                pick.text = basePick + " " + selectedObject.name + buttonText;
                //SI pulsa B lo cogemos
				if (OVRInput.GetDown(OVRInput.Button.Two)/*DEBUG CONTROLL*/ ||Input.GetButtonDown("Fire1"))
                {
					switch (selectedObject.name) {
						case "Linterna":
							hasLinterna = true;
							activeObject = Objetos.Linterna;
							crosshair.enabled = false;
							pistolaObj.SetActive (false);
							linternaObj.SetActive (true);
							break;
						case "Pistola":
							hasPistola = true;
							crosshair.enabled = true;
							activeObject = Objetos.Pistola;
							linternaObj.SetActive (false);
							pistolaObj.SetActive (true);
							break;
					}
					Destroy (selectedObject);
                }
            }
            else
                pick.enabled = false;
        }
        else
            pick.enabled = false;

		//TOOGLE LINTERNA
		if (hasLinterna && (Input.GetKeyDown(KeyCode.E) || OVRInput.GetDown(OVRInput.Button.Three)) /* X */) {
			
			if (this.activeObject == Objetos.Linterna) {
				activeObject = Objetos.Mano;
				linternaObj.SetActive (false);
			} 
			else {
				activeObject = Objetos.Linterna;
				crosshair.enabled = false;
				pistolaObj.SetActive (false);
				linternaObj.SetActive (true);
			}
		}

		//TOOGLE PISTOLA
		if (hasPistola && (Input.GetKeyDown(KeyCode.Q) || OVRInput.GetDown(OVRInput.Button.Four)) /* Y */) {
			
			if (this.activeObject == Objetos.Pistola) {
				crosshair.enabled = false;
				activeObject = Objetos.Mano;
				pistolaObj.SetActive (false);
			} 
			else {
				crosshair.enabled = true;
				activeObject = Objetos.Pistola;
				linternaObj.SetActive (false);
				pistolaObj.SetActive (true);
			}
		}
			
	}

    public void Damage(int value)
    {
        hp -= value;
    }


}
