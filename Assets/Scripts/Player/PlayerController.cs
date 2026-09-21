using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{
    #region DataMembers
    private Quaternion _defualtRotation;
    private Vector3 _cameraDistance;
    private Quaternion _cameraRotationOffset = Quaternion.Euler(0f, 180f, 0f);

    #endregion

    #region Properties
    private IInputManager InputManager { get; set; }

    private Camera MainCamera {get;set;}

    private Rigidbody Rigidbody {get;set;}

    private GameSettings GameSettings {get;set;}

    private IGameManager GameManager {get;set;}

    #endregion

    protected override void Start()
    {
        base.Start();
        InitializeProperties();
        this._defualtRotation = this.transform.rotation;
        this.MainCamera.transform.rotation = this._cameraRotationOffset;
        this._cameraDistance = this.MainCamera.transform.position - this.transform.position;
    }

    private void InitializeProperties()
    {
        this.InputManager = this.Container.Resolve<IInputManager>();
        this.GameSettings = this.Container.Resolve<GameSettings>();
        this.GameManager = this.Container.Resolve<IGameManager>();
        this.Rigidbody = this.GetComponent<Rigidbody>();
        this.MainCamera = Camera.main;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Consts.PlaneTag)
        this.GameManager.SpawnPlaneByCollison(other);
    }

    void FixedUpdate()
    {
        if (!this.GameManager.IsGameRunning())
        {
            return;
        }

        float speed =  this.GameSettings.BaseSpeed * Time.fixedDeltaTime;
        Vector3 newpos  = this.transform.position + Vector3.back *speed;
        var direction = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);

        if(direction != 0)
        {
            newpos += Vector3.left *direction * speed *this.GameSettings.MoveSpeedDiff;
            Quaternion targetRotation = Quaternion.Euler(this._defualtRotation.eulerAngles + new Vector3( this.GameSettings.TurnAngle * direction, 0, 0));    
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, targetRotation, this.GameSettings.TurnRate));
        }
        else
        {
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, this._defualtRotation, this.GameSettings.TurnRate));
        } 
        
        this.Rigidbody.MovePosition(newpos);
    }

    void LateUpdate()
    {
        if (!this.GameManager.IsGameRunning())
        {
            return;
        }

        this.MainCamera.transform.position = this.transform.position + this._cameraDistance;

        Quaternion playerTilt = this.Rigidbody.rotation * Quaternion.Inverse(this._defualtRotation);// get the rotation done by removing the defualt rotation from rotation
        Quaternion targetRotation = this._cameraRotationOffset * playerTilt;
        this.MainCamera.transform.rotation = Quaternion.Slerp(this.MainCamera.transform.rotation, targetRotation, this.GameSettings.TurnRate);
    }
}
