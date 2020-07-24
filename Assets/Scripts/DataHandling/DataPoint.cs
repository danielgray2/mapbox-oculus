using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.Analysis;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public DataFrameRow data { get; set; }

    List<IAnimation> animationList = new List<IAnimation>();
    List<IAnimation> animsToAdd = new List<IAnimation>();
    List<IAnimation> animsToRemove = new List<IAnimation>();
    bool instantiated = false;

    public Vector3 origLocalScale { get; set; }
    public Vector3 origLocalPos { get; set; }
    public Quaternion origLocalRotation { get; set; }
    public Color origColor { get; set; }

    void Update()
    {
        HandleAnimations();
    }

    public void SetData(DataFrameRow data)
    {
        this.data = data;
    }

    protected void HandleAnimations()
    {
        CheckForRemovals();
        CheckForAdditions();
        foreach(IAnimation currAnim in animationList)
        {
            currAnim.Animate();
        }
    }

    public void RemoveAnimation(IAnimation anim)
    {
        animsToRemove.Add(anim);
    }

    public void AddAnimation(IAnimation anim)
    {
        animsToAdd.Add(anim);
    }

    protected void CheckForRemovals()
    {
        foreach(IAnimation anim in animsToRemove)
        {
            if (animationList.Contains(anim))
            {
                animationList.Remove(anim);
            }
            else
            {
                throw new ArgumentException("The animation that you wanted to remove does not currently exist");
            }
        }
        animsToRemove = new List<IAnimation>();
    }

    protected void CheckForAdditions()
    {
        foreach (IAnimation anim in animsToAdd)
        {
            if (!animationList.Contains(anim))
            {
                animationList.Add(anim);
            }
            else
            {
                throw new ArgumentException("The animation that you wanted to add currently exists");
            }
        }
        animsToAdd = new List<IAnimation>();
    }

    public void SetOriginalValues()
    {
        origLocalPos = this.transform.localPosition;
        origLocalScale = this.transform.localScale;
        origLocalRotation = this.transform.localRotation;
        origColor = this.GetComponent<Renderer>().material.color;
    }
}
