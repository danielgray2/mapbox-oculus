using Assets.Mapbox.Unity.MeshGeneration.Modifiers.MeshModifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using UnityScript.Steps;

/*
public class CalMenuContr : IAbstractMenu
{
    [SerializeField]
    GameObject menuManagerGo;

    public class UnityEvent : UnityEvent<List<DateTime>> { }

    [SerializeField]
    GameObject monthDDGo;

    [SerializeField]
    GameObject dayDDGo;

    [SerializeField]
    GameObject hourSliderGo;

    [SerializeField]
    GameObject currTimeTextGo;

    TMP_Dropdown monthDDObj;
    TMP_Dropdown dayDDObj;
    Slider hourSliderObj;
    TMP_Text currTimeTextObj;
    const int DATA_YEAR = 2019;

    Dictionary<string, int> monthNameToIntMap = new Dictionary<string, int>
    {
        { "jan", 1 },
        { "feb", 2 },
        { "mar", 3 },
        { "apr", 4 },
        { "may", 5 },
        { "jun", 6 },
        { "jul", 7 },
        { "aug", 8 },
        { "sep", 9 },
        { "oct", 10 },
        { "nov", 11 },
        { "dec", 12 },
    };

    Dictionary<int, string> monthIntToNameMap = new Dictionary<int, string>
    {
        { 1, "jan" },
        { 2, "feb" },
        { 3, "mar" },
        { 4, "apr" },
        { 5, "may" },
        { 6, "jun" },
        { 7, "jul" },
        { 8, "aug" },
        { 9, "sept" },
        { 10, "oct" },
        { 11, "nov" },
        { 12, "dec" },
    };

    public UnityEvent DateUpdated = new UnityEvent();

    void Start()
    {
        monthDDObj = monthDDGo.GetComponent<TMP_Dropdown>();
        dayDDObj = dayDDGo.GetComponent<TMP_Dropdown>();
        currTimeTextObj = currTimeTextGo.GetComponent<TMP_Text>();
        hourSliderObj = hourSliderGo.GetComponent<Slider>();
        UpdateSelectedTime();
    }

    public void SetMonthOptions(List<string> stringList)
    {
        List<TMP_Dropdown.OptionData> optionsList = CreateOptions(stringList);
        monthDDObj.options = optionsList;
    }

    public void SetDayOptions(List<string> stringList)
    {
        List<TMP_Dropdown.OptionData> optionsList = CreateOptions(stringList);
        dayDDObj.options = optionsList;
    }

    protected List<TMP_Dropdown.OptionData> CreateOptions(List<string> stringList)
    {
        List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();
        foreach (string item in stringList)
        {
            optionData.Add(new TMP_Dropdown.OptionData(item));
        }

        return optionData;
    }

    public void HandleMonthChanged()
    {
        string monthValue = monthDDObj.options.ElementAt(monthDDObj.value).text;
        string lowerCaseMonth = monthValue.ToLower();
        int monthValueInt = monthNameToIntMap[lowerCaseMonth];
        
        int days = DateTime.DaysInMonth(DATA_YEAR, monthValueInt);
        List<string> newDays = new List<string>();
        for (int i = 1; i <= days; i++)
        {
            newDays.Add(i.ToString());
        }
        SetDayOptions(newDays);
        UpdateSelectedTime();
        
    }

    public void HandleDayChanged()
    {
        UpdateSelectedTime();
    }

    public void HandleHourChanged()
    {
        UpdateSelectedTime();
    }

    public void UpdateSelectedTime()
    {
        double minsToAdd = 59;
        string monthValue = monthDDObj.options.ElementAt(monthDDObj.value).text;
        string dayValue = dayDDObj.options.ElementAt(dayDDObj.value).text;
        int startHour = (int)hourSliderObj.value;

        string lowerCaseMonth = monthValue.ToLower();
        int monthValueInt = monthNameToIntMap[lowerCaseMonth];
        int dayValueInt = Int32.Parse(dayValue);
        int minute = 0;
        int second = 0;

        DateTime startTime = new DateTime(DATA_YEAR, monthValueInt, dayValueInt, startHour, minute, second);
        DateTime endTime = startTime.AddMinutes(minsToAdd);

        currTimeTextObj.text = monthValue + ". " + dayValue + ", " + DATA_YEAR.ToString() + " between " + startTime.ToShortTimeString() + " and " + endTime.ToShortTimeString();

        DateUpdated?.Invoke(new List<DateTime> { startTime, endTime });
    }
}
*/