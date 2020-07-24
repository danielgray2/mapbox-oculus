using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Microsoft.Data.Analysis;
using System;

namespace Tests
{
    public class DataObjTests
    {
        DataFrame df;
        DataFrame dfFromCsv;
        DataObj dO;
        DataObj dOCsv;

        [SetUp]
        public void Setup()
        {
            PrimitiveDataFrameColumn<float> colOne = new PrimitiveDataFrameColumn<float>("colOne", 4);
            StringDataFrameColumn colTwo = new StringDataFrameColumn("colTwo", 4);
            StringDataFrameColumn colThree = new StringDataFrameColumn("colThree", 4);
            StringDataFrameColumn colFour = new StringDataFrameColumn("colFour", 4);
            StringDataFrameColumn colFive = new StringDataFrameColumn("colFive", 4);

            colOne[0] = 1f;
            colOne[1] = 2f;
            colOne[2] = 3f;
            colOne[3] = 4f;

            colTwo[0] = "5";
            colTwo[1] = "6";
            colTwo[2] = "7";
            colTwo[3] = "8";

            colThree[0] = "a";
            colThree[1] = "b";
            colThree[2] = "c";
            colThree[3] = "d";

            colFour[0] = "05/17/2020";
            colFour[1] = "05/18/2020";
            colFour[2] = "05/19/2020";
            colFour[3] = "05/20/2020";

            colFive[0] = "08:30:22.145";
            colFive[1] = "08:31:22";
            colFive[2] = "08:33:22.145";
            colFive[3] = "08:34:22.145";

            colOne.Where(val => val < 1f);

            df = new DataFrame(colOne, colTwo, colThree, colFour, colFive);
            dO = new DataObj(df);

            dfFromCsv = DataFrame.LoadCsv("Assets\\Tests\\TestData.csv");
            dOCsv = new DataObj(dfFromCsv);
        }

        [Test]
        public void TestCalcMinInt()
        {
            float min = dO.GetMin("colOne");
            Assert.AreEqual(1f, min);
        }

        [Test]
        public void TestCalcMinString()
        {
            string min = dO.GetMin("colThree");
            Assert.AreEqual("a", min);
        }

        [Test]
        public void TestCalcMaxInt()
        {
            float max = dO.GetMax("colOne");
            Assert.AreEqual(4f, max);
        }

        [Test]
        public void TestCalcMaxString()
        {
            string max = dO.GetMax("colThree");
            Assert.AreEqual("d", max);
        }

        [Test]
        public void TestCalcAvg()
        {
            float avg = (float)dO.GetMedian("colOne");
            Assert.AreEqual(2.5f, avg, 0.001);
        }

        [Test]
        public void TestCalcLowerQrt()
        {
            float lowerQrt = dO.GetLowerQrt("colOne");
            Assert.AreEqual(1.5f, lowerQrt, 0.001);
        }

        [Test]
        public void TestCalcUpperQrt()
        {
            float upperQrt = dO.GetUpperQrt("colOne");
            Assert.AreEqual(3.5f, upperQrt, 0.001);
        }

        [Test]
        public void TestCalcMinFromCsv()
        {
            float min = dOCsv.GetMin("colOne");
            Assert.AreEqual(1f, min, 0.001);
        }

        [Test]
        public void TestCalcMaxFromCSV()
        {
            float max = dOCsv.GetMax("colOne");
            Assert.AreEqual(4f, max);
        }

        [Test]
        public void TestCalcAvgFromCSV()
        {
            float avg = (float)dOCsv.GetMedian("colOne");
            Assert.AreEqual(2.5f, avg, 0.001);
        }

        [Test]
        public void TestCalcLowerQrtFromCSV()
        {
            float lowerQrt = dOCsv.GetLowerQrt("colOne");
            Assert.AreEqual(1.5f, lowerQrt, 0.001);
        }

        [Test]
        public void TestCalcUpperQrtFromCSV()
        {
            float upperQrt = dOCsv.GetUpperQrt("colOne");
            Assert.AreEqual(3.5f, upperQrt, 0.001);
        }

        [Test]
        public void TestSliceByAttrInt()
        {
            DataObj retObj = dO.SliceByAttribute("colOne", 2, 3);
            DataFrame retDf = retObj.df;
            Assert.AreEqual(2f, retDf[0, 0]);
            Assert.AreEqual(2, retDf.Rows.Count());
        }

        [Test]
        public void TestSliceByAttrStr()
        {
            DataObj retObj = dOCsv.SliceByAttribute("colThree", 'b', 'd');
            DataFrame retDf = retObj.df;
            Assert.AreEqual(2f, retDf[0, 0]);
            Assert.AreEqual(3, retDf.Rows.Count());
        }

        [Test]
        public void TestSliceByIndex()
        {
            DataObj retObj = dOCsv.SliceByIndex(1, 3);
            DataFrame retDf = retObj.df;
            Assert.AreEqual(2f, retDf[0, 0]);
            Assert.AreEqual(3, retDf.Rows.Count());
        }

        [Test]
        public void TestDTP()
        {
            dO.ParseDateColumns();
        }

        [Test]
        public void TestCreateDateFromStringNoMillis()
        {
            DateTime ret = dO.CreateDateFromString("2020-07-17 20:00:00");
            Assert.AreEqual(new DateTime(2020, 7, 17, 20, 0, 0), ret);
        }

        [Test]
        public void TestCreateDateFromStringMillis()
        {
            DateTime ret = dO.CreateDateFromString("2020-07-17 20:01:30.123");
            Assert.AreEqual(new DateTime(2020, 7, 17, 20, 1, 30, 123), ret);
        }

        [Test]
        public void TestCreateDateCol()
        {
            DataFrameColumn col = new StringDataFrameColumn("dateCol", 5);
            string stringOne = "03-19-2020 12:35:42";
            string stringTwo = "05/27/2020 9:22:01";
            string stringThree = "10/19/20 12:35:42";
            string stringFour = "2020-07-02 05:35:42";
            string stringFive = "2020-07-02 05:35:42.123";


            col[0] = stringOne;
            col[1] = stringTwo;
            col[2] = stringThree;
            col[3] = stringFour;
            col[4] = stringFive;

            PrimitiveDataFrameColumn<DateTime> dTC = dO.AttemptParseDateCol(col);
            Assert.AreEqual(new DateTime(2020, 3, 19, 12, 35, 42), dTC[0]);
            Assert.AreEqual(new DateTime(2020, 5, 27, 9, 22, 1), dTC[1]);
            Assert.AreEqual(new DateTime(2020, 10, 19, 12, 35, 42), dTC[2]);
            Assert.AreEqual(new DateTime(2020, 7, 2, 5, 35, 42), dTC[3]);
            Assert.AreEqual(new DateTime(2020, 7, 2, 5, 35, 42, 123), dTC[4]);
        }

        [Test]
        public void TestCreateDateColHandlesTimesOnly()
        {
            DataFrameColumn col = new StringDataFrameColumn("dateCol", 5);
            string stringOne = "12:35:42";
            col[0] = stringOne;
            PrimitiveDataFrameColumn<DateTime> dTC = dO.AttemptParseDateCol(col);
            Assert.AreEqual(null, dTC);
        }

        [Test]
        public void TestCanFindMillis()
        {
            string checkThis = "12:43:55.111";
            int ret = dO.CheckForMillis(checkThis);
            Assert.AreEqual(111, ret);
        }

        [Test]
        public void TestAddsMillisCorrectly()
        {
            string checkThis = "12:43:55.1";
            int ret = dO.CheckForMillis(checkThis);
            Assert.AreEqual(100, ret);

            checkThis = "12:43:55.10";
            ret = dO.CheckForMillis(checkThis);
            Assert.AreEqual(100, ret);

            checkThis = "12:43:55.10 extra text";
            ret = dO.CheckForMillis(checkThis);
            Assert.AreEqual(100, ret);
        }

        [Test]
        public void TestNoFalseMillis()
        {
            string[] checkThese = new string[]
            {
                "12:43:55.10.",
                "12.43.5510",
                "12.abc"
            };

            foreach(string s in checkThese)
            {
                int ret = dO.CheckForMillis(s);
                Assert.AreEqual(0, ret, "This failed on " + s);
            }
        }

        [Test]
        public void TestParseDateColumns()
        {
            dO.ParseDateColumns();

            Assert.AreEqual(1, df.Columns["colOne"][0]);
            Assert.AreEqual("5", df.Columns["colTwo"][0]);
            Assert.AreEqual("a", df.Columns["colThree"][0]);
            Assert.AreEqual(new DateTime(2020, 5, 17), df.Columns["colFour"][0]);
            Assert.AreEqual("08:30:22.145", df.Columns["colFive"][0]);
        }

        [Test]
        public void TestGetIQR()
        {
            float val = dO.GetIQR("colOne");
            Assert.AreEqual(2, val);
        }
    }
}
