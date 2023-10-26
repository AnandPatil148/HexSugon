using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Menu[] Menus;

    public void OpenMenu(Menu menu)
    {
        foreach(Menu M in Menus)
        {
            M.Close();
        }

        menu.Open();
    }
    public void OpenMenu(string menuName)
    {
        foreach(Menu M in Menus)
        {
            if(M.menuName == menuName)
            {
                M.Open();
            }
            else
            {
                M.Close();
            }
        }
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void CloseMenu(string menuName)
    {
        foreach(Menu M in Menus)
        {
            if(M.menuName == menuName)
            {
                M.Close();
            }
        }
    }

}
