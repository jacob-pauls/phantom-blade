using System.Collections;
using UnityEngine;

public class DoubleJumpOverlay : StandardMenu
{
    [SerializeField] private TPB_Player player;

    public override void Initialize()
    {
        if (!isInitialized)
        {
            player.onDoubleJumpUnlocked.AddListener(ShowDoubleJumpOverlay);
            animator = GetComponent<Animator>();
            isInitialized = true;
        }
    }

    void ShowDoubleJumpOverlay()
    {
        Show();
        StartCoroutine("CloseMenuCoroutine");
    }
    
    private IEnumerator CloseMenuCoroutine() 
    {
        yield return new WaitForSeconds(5);
        Hide(); 
    }
}
