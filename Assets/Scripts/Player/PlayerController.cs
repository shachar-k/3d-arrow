using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{
    private const int NO_VALUE = -999;
    #region DataMembers
    private Quaternion _defualtRotation;
    private Vector3 _cameraDistance;
    private Quaternion _cameraRotationOffset = Quaternion.Euler(0f, 180f, 0f);

    #endregion

    #region Properties
    private IInputManager InputManager { get; set; }

    private Camera MainCamera { get; set; }

    private Rigidbody Rigidbody { get; set; }

    private GameSettings GameSettings { get; set; }

    private IGameManager GameManager { get; set; }

    private float TimeSinceRamp { get; set; } = NO_VALUE;

    private int RampCount { get; set; } = 1;

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
        if (other.tag == Consts.PlaneTag)
        {
            this.GameManager.SpawnPlaneByCollison(other);
        }
        else if(other.tag == Consts.ObsticalTag)
        {
            this.gameObject.SetActive(false);
            this.GameManager.GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collison entered");
    }

    void FixedUpdate()
    {
        if (!this.GameManager.IsGameRunning())
        {
            return;
        }

        this.CalcRamp();

        float speed = this.GameSettings.BaseSpeed * Time.fixedDeltaTime *this.RampCount * this.GameSettings.RampSpeed;
        Vector3 newpos = this.transform.position + Vector3.back * speed;
        var direction = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);

        if (direction != 0)
        {
            newpos += Vector3.left * direction * speed * this.GameSettings.MoveSpeedDiff;
            Quaternion targetRotation = Quaternion.Euler(this._defualtRotation.eulerAngles + new Vector3(this.GameSettings.TurnAngle * direction, 0, 0));
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, targetRotation, this.GameSettings.TurnRate));
        }
        else
        {
            this.Rigidbody.MoveRotation(Quaternion.Slerp(this.Rigidbody.rotation, this._defualtRotation, this.GameSettings.TurnRate));
        }

        this.Rigidbody.MovePosition(newpos);
    }

    private void CalcRamp()
    {
        if (this.TimeSinceRamp == NO_VALUE)
        {
            this.TimeSinceRamp = Time.time;
        }
        else if (Time.time - this.TimeSinceRamp >= this.GameSettings.RampFixedMinutes * 60 && this.RampCount <= this.GameSettings.MaxRamp)
        {
            this.RampCount++;
            this.TimeSinceRamp = Time.time;
        }
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
