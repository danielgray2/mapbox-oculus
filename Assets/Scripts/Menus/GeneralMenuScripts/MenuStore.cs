using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MenuStore
{
    private static MenuStore instance = null;
    private static readonly object padlock = new object();

    MenuStore() { }

    public static MenuStore Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MenuStore();
                }
                return instance;
            }
        }
    }
}
