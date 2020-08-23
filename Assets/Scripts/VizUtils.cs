using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;

public static class VizUtils
{
    public static IAbsCompModel CastToCompModel(IAbsModel model)
    {
        if(!(model is IAbsCompModel casted))
        {
            throw new ArgumentException("Model must be of type CompModel");
        }
        return casted;
    }

    public static bool Compose(IAbsCompModel superComp, IAbsCompModel subComp)
    {
        bool compatSubComp = superComp.compatSubComps.Contains(subComp.GetType());
        bool compatSuperComp = subComp.compatSuperComps.Contains(superComp.GetType());

        if (compatSubComp && compatSuperComp)
        {
            DataObj dataObj = subComp.dataObj;
            for(int i = 0; i < subComp.useTransfs.Count; i++)
            {
                DataObj transformedDo = subComp.useTransfs[i].ApplyTransformation(dataObj);
                DataObj validatedDo = new DataObj(ValidateColNames(transformedDo.df, subComp.dataObj.df));
                DataFrame merged = subComp.dataObj.MergeDf(validatedDo.df);
                subComp.dataObj.UpdateDf(merged);
            }
            return true;
        }
        return false;
    }

    public static DataFrame ValidateColNames(DataFrame dfToAdd, DataFrame dfMain)
    {
        List<string> origList = dfMain.Columns.Select(c => c.Name).ToList();
        List<string> newList = dfToAdd.Columns.Select(c => c.Name).ToList();
        List<string> inter = origList.Intersect(newList).ToList();
        int nameId = 1;
        while(inter.Count > 0)
        {
            for(int i = 0; i < inter.Count; i++)
            {
                string baseName;
                string prevName;
                if (nameId > 1)
                {
                    int numDigits = (int)Math.Floor(Math.Log10(nameId - 1) + 1);
                    baseName = inter[i].Substring(0, inter[i].Length - numDigits - 1);
                    int prevId = nameId - 1;
                    prevName = baseName + "_" + prevId;
                }
                else
                {
                    baseName = inter[i];
                    prevName = baseName;
                }

                inter[i] = baseName + "_" + nameId;
                DataFrameColumn col = dfToAdd.Columns[prevName];
                dfToAdd.Columns.SetColumnName(col, inter[i]);
            }
            newList = dfToAdd.Columns.Select(c => c.Name).ToList();
            inter = origList.Intersect(newList).ToList();
            nameId ++;
        }
        return dfToAdd;
    }
}
