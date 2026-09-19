using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{
    #region DataMembers
    private Quaternion _defualtRotation;
    private Vector3 _cameraDistance;
    private Quaternion _cameraRotationOffset;

    #endregion

    #region Properties
    private IInputManager InputManager { get; set; }

    private Camera MainCamera {get;set;}

    private Rigidbody Rigidbody {get;set;}

    private GameSettings GameSettings {get;set;}

    #endregion

    protected override void Start()
    {
        base.Start();
        this.InputManager = this.Container.Resolve<IInputManager>();
        this.GameSettings = this.Container.Resolve<GameSettings>();
        this.Rigidbody = this.GetComponent<Rigidbody>();
        this._defualtRotation = this.transform.rotation;
        this.MainCamera = Camera.main;
        this._cameraRotationOffset = Quaternion.Euler(0f, 180f, 0f);
        this.MainCamera.transform.rotation = this._cameraRotationOffset;
        this._cameraDistance = this.MainCamera.transform.position - this.transform.position;
    }

    void FixedUpdate()
    {
        var direction = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);

        if(direction != 0)
        {
            Vector3 newpos = this.transform.position + Vector3.left *direction * this.GameSettings.Movespeed;
            Quaternion targetRotation = Quaternion.Euler(this._defualtRotation.eulerAngles + new Vector3( this.GameSettings.TurnAngle * direction, 0, 0));    
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, targetRotation, this.GameSettings.TurnRate));
            this.Rigidbody.MovePosition(newpos);
        }
        else
        {
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, this._defualtRotation, this.GameSettings.TurnRate));
        }
    }

    void LateUpdate()
    {
        this.MainCamera.transform.position = this.transform.position + this._cameraDistance;

        Quaternion playerTilt = this.Rigidbody.rotation * Quaternion.Inverse(this._defualtRotation);
        Quaternion targetRotation = this._cameraRotationOffset * playerTilt;
        this.MainCamera.transform.rotation = Quaternion.Slerp(this.MainCamera.transform.rotation, targetRotation, this.GameSettings.TurnRate);
    }
}
