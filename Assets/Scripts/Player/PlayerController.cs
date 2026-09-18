using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{
    #region DataMembers
    [SerializeField]
    private float _movespeed = 0.3f;
    [SerializeField]
    private int _turnAngle = 30;
    [SerializeField]
    private float _turnRate = 0.25f;
    private Quaternion _defualtRotation;

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
        this._defualtRotation = this.transform.rotation;
        this.MainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        var direction = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);

        if(direction != 0)
        {
            Vector3 newpos = this.transform.position + Vector3.left *direction * _movespeed;
            Quaternion targetRotation = Quaternion.Euler(this._defualtRotation.eulerAngles + new Vector3( _turnAngle * direction, 0, 0));    
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, targetRotation, _turnRate));
            this.Rigidbody.MovePosition(newpos);
        }
        else
        {
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, this._defualtRotation, _turnRate));
        }

    }
}
