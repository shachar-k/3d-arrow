using UnityEngine;

public class PlayerController : Injectable<PlayerController, IPlayerContoller>, IPlayerContoller
{

    #region Properties
    private IInputManager InputManager {get;set;}

    #endregion

    protected override void Start()
    {
        base.Start();
        this.InputManager = this.Container.Resolve<IInputManager>();
    }

    void FixedUpdate()
    {
        var moveValue = this.InputManager.GetInputValue<float>(eInput.playerMoveInput);
        Debug.Log($"Player Move Input: {moveValue}");
    }
}
