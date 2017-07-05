using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public int hp = 100;
    public Text pick;
    public string basePick = "<b>Recoger";
    private string buttonText = "\n Pulsa B</b>";
    private float catchDistance = 3f;
    private Animator animator;
    private GameObject rightArm;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        rightArm = GameObject.FindGameObjectWithTag("rightArm");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //ANIMACION ANDAR
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != new Vector2(0, 0))
        {
            if(!animator.GetBool("isWalking"))
                animator.SetBool("isWalking", true);
        }
        else
        {
            if (animator.GetBool("isWalking"))
                animator.SetBool("isWalking", false);
        }
        //COGER UN OBJETO
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // al medio de la pantalla
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, catchDistance))
        {
            GameObject selectedObject = hit.collider.gameObject;
            if (selectedObject.tag.Equals("Collectable"))
            {
                //SI esta mirando a un objeto coleccionable
                pick.enabled = true;
                pick.text = basePick + " " + selectedObject.name + buttonText;
                //SI pulsa B lo cogemos
                if (OVRInput.Get(OVRInput.Button.Two))
                {
                    selectedObject.SendMessage("Collect");
                }
            }
            else
                pick.enabled = false;
        }
        else
            pick.enabled = false;
        
	}

    public void Damage(int value)
    {
        hp -= value;
    }
}
