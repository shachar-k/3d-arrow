using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{
    #region DataMembers
    [SerializeField]
    private float _movespeed = 0.3f;

    #endregion

    #region Properties
    private IInputManager InputManager { get; set; }

    private Camera MainCamera {get;set;}

    private Rigidbody Rigidbody {get;set;}

    #endregion

    protected override void Start()
    {
        base.Start();
        this.InputManager = this.Container.Resolve<IInputManager>();
        this.Rigidbody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var direction = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);

        if(direction != 0)
        {
            Debug.Log("here");
            Vector3 newpos = this.transform.position + Vector3.left *direction * _movespeed;
            
            this.Rigidbody.MovePosition(newpos);
        }

    }
}
