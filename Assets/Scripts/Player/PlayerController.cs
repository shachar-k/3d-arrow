using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{
    #region Consts
    private const int NO_VALUE = -999;

    #endregion 

    #region DataMembers
    [SerializeField]
    private float _particleAnimationTime = 0.8f;
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

    private ParticleSystem ParticleSystem { get; set; }

    private float TimeSinceRamp { get; set; } = NO_VALUE;

    private int RampCount { get; set; } = 1;

    #endregion

    #region Methods

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    protected override void Start()
    {
        base.Start();
        InitializeProperties();
        this._defualtRotation = this.transform.rotation;
        this.MainCamera.transform.rotation = this._cameraRotationOffset;
        this._cameraDistance = this.MainCamera.transform.position - this.transform.position;
        this.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!this.GameManager.IsGameRunning())
        {
            return;
        }

        if (!this.isActiveAndEnabled)
        {
            this.gameObject.SetActive(true);
        }

        this.CalcRamp();

        float speed = this.GameSettings.BaseSpeed * Time.fixedDeltaTime * this.RampCount * this.GameSettings.RampSpeed;
        Vector3 newpos = this.transform.position + Vector3.back * speed;
        float direction = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);

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

    void OnTriggerEnter(Collider other)
    {
        if(TimeSinceRamp < 0.1)
        {
            return;
        }

        if (other.tag == Consts.PlaneTag)
        {
            this.GameManager.SpawnPlaneByCollison(other);
        }
        else if (other.tag == Consts.ObsticalTag)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GameManager.SetStatus(eGameStatus.GameOver);
            StartCoroutine(this.PlayParticalsAsync());
        }
    }

    private System.Collections.IEnumerator PlayParticalsAsync()
    {
        this.ParticleSystem.gameObject.SetActive(true);
        this.ParticleSystem.Clear();
        this.ParticleSystem.Play();
        yield return new WaitForSeconds(this._particleAnimationTime);
        this.ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        this.ParticleSystem.gameObject.SetActive(false);
        this.GameManager.GameOverScreen();
        yield return null;
        this.gameObject.SetActive(false);
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

    private void InitializeProperties()
    {
        this.InputManager = this.Container.Resolve<IInputManager>();
        this.GameSettings = this.Container.Resolve<GameSettings>();
        this.GameManager = this.Container.Resolve<IGameManager>();
        this.Rigidbody = this.GetComponent<Rigidbody>();
        this.ParticleSystem = this.GetComponentInChildren<ParticleSystem>();
        this.MainCamera = Camera.main;
        this.ParticleSystem.gameObject.SetActive(false);
    }

    #endregion
}
