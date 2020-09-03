using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenGlyphMenuContr : IAbsCompContr
{

    public void UpdateGlyphXPosName(IAbsModel model, string xPosColName)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.xPosColName = xPosColName;
    }

    public void UpdateGlyphYPosName(IAbsModel model, string yPosColName)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.yPosColName = yPosColName;
    }

    public void UpdateGlyphZPosName(IAbsModel model, string zPosColName)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.zPosColName = zPosColName;
    }

    public void UpdateGlyphOneCompAxisOne(IAbsModel model, string oneCompAxisOne)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.oneCompAxisOneName = oneCompAxisOne;
    }

    public void UpdateGlyphOneCompAxisTwo(IAbsModel model, string oneCompAxisTwo)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.oneCompAxisTwoName = oneCompAxisTwo;
    }

    public void UpdateGlyphOneCompAxisThree(IAbsModel model, string oneCompAxisThree)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.oneCompAxisThreeName = oneCompAxisThree;
    }

    public void UpdateGlyphTwoCompAxisOne(IAbsModel model, string twoCompAxisOne)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.twoCompAxisOneName = twoCompAxisOne;
    }

    public void UpdateGlyphTwoCompAxisTwo(IAbsModel model, string twoCompAxisTwo)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.twoCompAxisTwoName = twoCompAxisTwo;
    }

    public void UpdateGlyphTwoCompAxisThree(IAbsModel model, string twoCompAxisThree)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.twoCompAxisThreeName = twoCompAxisThree;
    }

    public void UpdateGlyphThreeCompAxisOne(IAbsModel model, string threeCompAxisOne)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.threeCompAxisOneName = threeCompAxisOne;
    }

    public void UpdateGlyphThreeCompAxisTwo(IAbsModel model, string threeCompAxisTwo)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.threeCompAxisTwoName = threeCompAxisTwo;
    }

    public void UpdateGlyphThreeCompAxisThree(IAbsModel model, string threeCompAxisThree)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.threeCompAxisThreeName = threeCompAxisThree;
    }

    public void UpdateGlyphAxisOne(IAbsModel model, string glyphAxisOne)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.axisOneName = glyphAxisOne;
    }

    public void UpdateGlyphAxisTwo(IAbsModel model, string glyphAxisTwo)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.axisTwoName = glyphAxisTwo;
    }

    public void UpdateGlyphAxisThree(IAbsModel model, string glyphAxisThree)
    {
        TenGlyphModel glyphModel = CastToGlyphModel(model);
        glyphModel.axisThreeName = glyphAxisThree;
    }

    public TenGlyphModel CastToGlyphModel(IAbsModel model)
    {
        if (!(model is TenGlyphModel glyphModel))
        {
            throw new ArgumentException("Model must be of type GlyphModel");
        }
        return glyphModel;
    }
}
