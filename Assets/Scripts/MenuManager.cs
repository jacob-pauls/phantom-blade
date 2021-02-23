using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        [SerializeField] private StandardMenu menu;
        public StandardMenu StandardMenu { get { return menu; } }
        [SerializeField] private KeyCode keyCode;
        public KeyCode KeyCode { get { return keyCode; } }

    }

    public List<Menu> menus = new List<Menu>();

    private void Update()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].KeyCode == KeyCode.None) { break; }

            if (Input.GetKeyDown(menus[i].KeyCode))
            {
                if (menus[i].StandardMenu.isMenuOpen) { menus[i].StandardMenu.Close(); } else { menus[i].StandardMenu.Show(); }
            }
        }
    }
}
