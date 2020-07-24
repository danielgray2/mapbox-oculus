using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAnimation
{
    public enum AnimateAttrs
    {
        SCALE,
        POS,
        COLOR
    }

    public enum EndBehav
    {
        DESTROY,
        STOP,
        REPEAT_BEGINNING,
        REVERSE,
        CALL_ANOTHER
    }
    public GameObject gO { get; set; }
    public IAnimation nextAnim { get; set; }
    public string colName { get; set; }
    public EndBehav endBehav;

    public IAnimation(EndBehav endBehav)
    {
        this.endBehav = endBehav;
    }

    public Dictionary<AnimateAttrs, string> Vector3AttributesEnum = new Dictionary<AnimateAttrs, string>
    {
        { AnimateAttrs.SCALE, "scale" },
        { AnimateAttrs.POS, "pos" },
        { AnimateAttrs.COLOR, "color" }
    };

    public Dictionary<EndBehav, string> EndBehaviorsEnum = new Dictionary<EndBehav, string>
    {
        { EndBehav.DESTROY, "destroy" },
        { EndBehav.STOP, "stop" },
        { EndBehav.REPEAT_BEGINNING, "repeatBeginning" },
        { EndBehav.REVERSE, "reverse" },
        { EndBehav.CALL_ANOTHER, "callAnother" }
    };

    public abstract void Animate();
    public void Activate(GameObject gO)
    {
        this.gO = gO;
        this.gO.GetComponent<DataPoint>().AddAnimation(this);
    }
    public void HandleEnd()
    {
        switch (this.endBehav)
        {
            case EndBehav.DESTROY:
                HandleDestroy();
                break;
            case EndBehav.STOP:
                HandleStop();
                break;
            case EndBehav.REPEAT_BEGINNING:
                HandleRepeatBeginning();
                break;
            case EndBehav.REVERSE:
                HandleReverse();
                break;
            case EndBehav.CALL_ANOTHER:
                HandleCallAnother();
                break;
            default:
                throw new ArgumentException("The way you want to handle the end is not valid");
        }
    }

    public abstract void HandleDestroy();

    public abstract void HandleStop();

    public abstract void HandleRepeatBeginning();

    public abstract void HandleReverse();
    public abstract void HandleCallAnother();
}
