using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This menu will be in charge of stopping gameplay and displaying the player's inventory.
 * The inventory will have dedicated slots, meaning only specific items go into certain slots.
 * It is only for visual progression.
 * Keys needs to show an amount
 * Scrolls are capped at 2 and show an amount
 * 
 */
public class PauseMenu : StandardMenu
{
    public override void Show()
    {
        base.Show();

        // Load player data
        // Use the data to update the information

        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }


}