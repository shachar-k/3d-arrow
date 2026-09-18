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
            Quaternion targetRotation = Quaternion.Euler(new Vector3( -90 + _turnAngle * direction,-90, 0));
            
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, targetRotation, _turnRate));

            this.Rigidbody.MovePosition(newpos);
        }

    }
}
