using UnityEngine;

public interface IUIManager
{
    #region Methods
    public System.Collections.IEnumerator DisplayGameOverScreen();

    public void SetPauseMenuVisibility(bool visibility);

    public void DisplayMenu();

    public void UpdateScore(int score);

    #endregion
}
