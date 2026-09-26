using UnityEngine;

public interface IUIManager
{
    #region Methods
    public System.Collections.IEnumerator DisplayGameOverScreen();

    public void DisplayMenu();

    public void UpdateScore(int score);

    #endregion
}
