using UnityEngine;
using System.Collections;
namespace LV
{
	public class MoveObjectController : MonoBehaviour
	{
		public float reachRange = 1.8f;

		private Animator anim;
		private Camera fpsCam;
		private GameObject player;

		private const string animBoolName = "isOpen_Obj_";

		private bool playerEntered;
		private bool showInteractMsg;
		private GUIStyle guiStyle;
		private string msg;
		private InputHandler inputHandler;
		private UIHandler uiHandler;
		private int rayLayerMask;
		private int rayLayerTask;		

		void Start()
		{
			//Initialize moveDrawController if script is enabled.
			player = GameObject.FindGameObjectWithTag("Player");
			uiHandler = player.GetComponent<UIHandler>();
			inputHandler = player.GetComponent<InputHandler>();
			fpsCam = Camera.main;
			if (fpsCam == null) //a reference to Camera is required for rayasts
			{
				Debug.LogError("A camera tagged 'MainCamera' is missing.");
			}

			//create AnimatorOverrideController to re-use animationController for sliding draws.
			anim = GetComponent<Animator>();
			anim.enabled = false;  //disable animation states by default.  

			//the layer used to mask raycast for interactable objects only
			LayerMask iRayLM = LayerMask.NameToLayer("InteractRaycast");
			LayerMask iRayLayerTask = LayerMask.NameToLayer("InteractTask");
			rayLayerMask = 1 << iRayLM.value;
			rayLayerTask = 1 << iRayLayerTask.value;

			//setup GUI style settings for user prompts
			setupGui();

		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject == player)     //player has collided with trigger
			{
				playerEntered = true;
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.gameObject == player)     //player has exited trigger
			{
				playerEntered = false;
				//hide interact message as player may not have been looking at object when they left
				showInteractMsg = false;
			}
		}

		private void Update()
		{
			interactionWithObject(inputHandler.interactFlag, inputHandler.taskFlag);
		}

		private void interactionWithObject(bool interactFlag, bool taskFlag)
		{
			if (playerEntered)
			{
				//center point of viewport in World space.
				Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
				RaycastHit hit;

				//if raycast hits a collider on the rayLayerMask
				if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, reachRange, rayLayerMask))
				{
					MoveableObject moveableObject = null;
					//is the object of the collider player is looking at the same as me?
					if (!isEqualToParent(hit.collider, out moveableObject))
					{   //it's not so return;
						return;
					}

					if (moveableObject != null)     //hit object must have MoveableDraw script attached
					{
						showInteractMsg = true;
						string animBoolNameNum = animBoolName + moveableObject.objectNumber.ToString();

						bool isOpen = anim.GetBool(animBoolNameNum);    //need current state for message.
						msg = getGuiMsg(isOpen);

						if (interactFlag)
						{
							anim.enabled = true;
							anim.SetBool(animBoolNameNum, !isOpen);
							msg = getGuiMsg(!isOpen);
						}

					}
				}
				else if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, reachRange, rayLayerTask))
				{
					MoveableObject moveableObject = null;
					//is the object of the collider player is looking at the same as me?
					if (!isEqualToParent(hit.collider, out moveableObject))
					{   //it's not so return;
						return;
					}

					if (moveableObject != null)     //hit object must have MoveableDraw script attached
					{
						showInteractMsg = true;
						//string animBoolNameNum = animBoolName + moveableObject.objectNumber.ToString();

						/*bool isOpen = anim.GetBool(animBoolNameNum); */   //need current state for message.
												
						msg = getGuiMsgTask(true);

						if (taskFlag)
						{
							//anim.enabled = true;
							//anim.SetBool(animBoolNameNum, !isOpen);
							uiHandler.HandleTaskBar(moveableObject.objectNumber);
							if (uiHandler.barPercent >= 100)
                            {
								msg = getGuiMsgTask(false);
							}
                            else
                            {
								msg = getGuiMsgTask(true);
							}							
						}
					}
				}
				else
				{
					showInteractMsg = false;
				}
			}
		}

		//is current gameObject equal to the gameObject of other.  check its parents
		private bool isEqualToParent(Collider other, out MoveableObject draw)
		{
			draw = null;
			bool rtnVal = false;
			try
			{
				int maxWalk = 6;
				draw = other.GetComponent<MoveableObject>();

				GameObject currentGO = other.gameObject;
				for (int i = 0; i < maxWalk; i++)
				{
					if (currentGO.Equals(this.gameObject))
					{
						rtnVal = true;
						if (draw == null) draw = currentGO.GetComponentInParent<MoveableObject>();
						break;          //exit loop early.
					}

					//not equal to if reached this far in loop. move to parent if exists.
					if (currentGO.transform.parent != null)     //is there a parent
					{
						currentGO = currentGO.transform.parent.gameObject;
					}
				}
			}
			catch (System.Exception e)
			{
				Debug.Log(e.Message);
			}

			return rtnVal;

		}


		#region GUI Config

		//configure the style of the GUI
		private void setupGui()
		{
			guiStyle = new GUIStyle();
			guiStyle.fontSize = 16;
			guiStyle.fontStyle = FontStyle.Bold;
			guiStyle.normal.textColor = Color.white;
			msg = "Press E to Open";
		}

		private string getGuiMsg(bool isOpen)
		{
			string rtnVal;
			if (isOpen)
			{
				rtnVal = "Press E to Close";
			} else
			{
				rtnVal = "Press E to Open";
			}

			return rtnVal;
		}

		private string getGuiMsgTask(bool isOpen)
		{
			string rtnVal;
			if (isOpen && uiHandler.barPercent < 100)
			{
				rtnVal = "Hold F to Start Task";
			}
			else
			{
				rtnVal = "Task Completed Today!";
			}
           
			return rtnVal;
		}


		void OnGUI()
		{
			if (showInteractMsg)  //show on-screen prompts to user for guide.
			{
				GUI.Label(new Rect(50, Screen.height - 50, 200, 50), msg, guiStyle);
			}
		}
		//End of GUI Config --------------
		#endregion
	}
}