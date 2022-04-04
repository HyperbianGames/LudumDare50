using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameObject ActionOneButton;
    public GameObject ActionTwoButton;
    public GameObject ActionThreeButton;
    public GameObject ActionFourButton;
    public GameObject ActionFiveButton;
    public GameObject ActionSixButton;
    public GameObject ActionSevenButton;
    public GameObject TabCollider;
    public GameObject Weapon;

    [SerializeField]
    private InputActionReference EscAction;

    [SerializeField]
    private InputActionReference LeftClick;

    [SerializeField]
    private InputActionReference MousePos;

    [SerializeField]
    private InputActionReference WierdOne;

    public Creature Creature;
    public GameObject UIObject;
    public GameObject TeleportObj;

    private int CurrentTargetIndex = 0;

    private Dictionary<InputAction, bool> previousState = new Dictionary<InputAction, bool>();

    public static PlayerController Instance;

    // Start is called before the first frame update
    void Start()
    {
        Creature.IsPlayer = true;
        Instance = this;
        actionOne = new InputAction("ActionOne", binding: CommonUsages.PrimaryAction);
        actionOne.AddBinding("<Keyboard>/1");
        actionOne.Enable();

        actionTwo = new InputAction("ActionTwo", binding: "<Gamepad>/x");
        actionTwo.AddBinding("<Keyboard>/2");
        actionTwo.Enable();

        actionThree = new InputAction("ActionThree", binding: "<Gamepad>/y");
        actionThree.AddBinding("<Keyboard>/3");
        actionThree.Enable();

        actionFour = new InputAction("ActionFour", binding: CommonUsages.PrimaryTrigger);
        actionFour.AddBinding("<Keyboard>/4");
        actionFour.Enable();

        actionFive = new InputAction("ActionFive", binding: CommonUsages.SecondaryTrigger);
        actionFive.AddBinding("<Keyboard>/5");
        actionFive.Enable();

        actionSix = new InputAction("ActionSix", binding: CommonUsages.LeftHand);
        actionSix.AddBinding("<Keyboard>/q");
        actionSix.Enable();

        actionSeven = new InputAction("ActionSeven", binding: CommonUsages.RightHand);
        actionSeven.AddBinding("<Keyboard>/e");
        actionSeven.Enable();

        tabAction = new InputAction("TabAction", binding: CommonUsages.RightHand);
        tabAction.AddBinding("<Keyboard>/tab");
        tabAction.Enable();

        EscAction.action.Enable();
        LeftClick.action.Enable();
        MousePos.action.Enable();
        WierdOne.action.Enable();

    }

    InputAction actionOne;
    InputAction actionTwo;
    InputAction actionThree;
    InputAction actionFour;
    InputAction actionFive;
    InputAction actionSix;
    InputAction actionSeven;

    InputAction tabAction;

    // Update is called once per frame
    void Update()
    {
        if (ActionWasClicked(actionOne))
        {
            ExecuteEvents.Execute(ActionOneButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(actionTwo))
        {
            ExecuteEvents.Execute(ActionTwoButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(actionThree))
        {
            ExecuteEvents.Execute(ActionThreeButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(actionFour))
        {
            ExecuteEvents.Execute(ActionFourButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(actionFive))
        {
            ExecuteEvents.Execute(ActionFiveButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(actionSix))
        {
            ExecuteEvents.Execute(ActionSixButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(actionSeven))
        {
            ExecuteEvents.Execute(ActionSevenButton.gameObject, null, ExecuteEvents.submitHandler);
        }

        if (ActionWasClicked(tabAction))
        {
            ActionTab();
        }

        if (ActionWasClicked(EscAction.action))
        {
            EscPress();
        }

        if (LeftClick.action.triggered)
            LeftClickPress();
    }

    private void LeftClickPress()
    {
        Ray ray = Camera.main.ScreenPointToRay(MousePos.action.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider != null && hit.collider.transform != this.transform)
            {
                MainObjectMarshmellow marshmellow = hit.transform.gameObject.GetComponent<MainObjectMarshmellow>();

                Creature creatureFound;
                if (marshmellow != null)
                {
                    creatureFound = marshmellow.Parent.GetComponent<Creature>();
                }
                else
                {
                    creatureFound = hit.transform.gameObject.GetComponent<Creature>();
                }
                 
                if (creatureFound != null && !creatureFound.IsPlayer)
                {
                    foreach (Creature creature in CombatManager.Instance.Creatures)
                    {
                        creature.SetAsPlayerTarget(false);
                    }

                    creatureFound.SetAsPlayerTarget(true);
                    Creature.CurrentTarget = creatureFound;
                }
            }
        }
    }

    private void EscPress()
    {
        if (Creature.CurrentTarget != null)
        {
            CurrentTargetIndex = 0;
            Creature.CurrentTarget = null;

            foreach (Creature creature in CombatManager.Instance.Creatures)
            {
                creature.SetAsPlayerTarget(false);
            }
        }
        else
        {
            UIObject.GetComponent<GameMenuController>().ToggleMenu();
        }
    }

    public bool ActionWasClicked(InputAction action)
    {
        if (!previousState.ContainsKey(action))
            previousState.Add(action, false);

        if (previousState[action] != Mathf.Approximately(action.ReadValue<float>(), 1))
        {
            previousState[action] = !previousState[action];

            if (previousState[action])
                return true;
        }

        return false;
    }

    public void ActionOne()
    {
        Spell spell = new ActionOne();
        spell.CastEndCallback = () => { Weapon.GetComponent<JankAnimationController>().ExecuteAbilityAnimation("ActionOne"); };
        Creature.CastSpell(spell);
    }

    public void ActionTwo()
    {
        Spell spell = new ActionTwo();
        spell.CastEndCallback = () => { Weapon.GetComponent<JankAnimationController>().ExecuteAbilityAnimation("ActionTwo"); };
        Creature.CastSpell(spell);
    }

    public void ActionThree()
    {
        Spell spell = new ActionThree();
        spell.CastEndCallback = () => { Weapon.GetComponent<JankAnimationController>().ExecuteAbilityAnimation("ActionOne"); };
        Creature.CastSpell(spell);
    }

    public void ActionFour()
    {
        print("ActionFour");
    }

    public void ActionFive()
    {
        print("ActionFive");
    }

    public void ActionSix()
    {
        print("ActionSix");
    }

    public void ActionSeven()
    {
        ReadyPlayerForElavator();
        //gameObject.transform.position = TeleportObj.transform.position;
    }

    public void ReadyPlayerForElavator()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.transform.position = TeleportObj.transform.position;
        GameMenuController.Instance.ShowElevator(3);
    }

    public void ReleasePlayer()
    {
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }

    public void ActionTab()
    {
        foreach (Creature creature in CombatManager.Instance.Creatures)
        {
            creature.SetAsPlayerTarget(false);
        }

        if (CurrentTargetIndex > CombatManager.Instance.VisibleCreatures.Count - 1)
            CurrentTargetIndex = 0;

        if (CombatManager.Instance.VisibleCreatures.Count > 0)
        {
            Creature.CurrentTarget = CombatManager.Instance.VisibleCreatures[CurrentTargetIndex];
            Creature.CurrentTarget.SetAsPlayerTarget(true);
            CurrentTargetIndex++;
        }
    }
}
