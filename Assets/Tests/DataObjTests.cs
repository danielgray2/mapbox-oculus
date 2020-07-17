using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Microsoft.Data.Analysis;

namespace Tests
{
    public class DataObjTests
    {
        DataFrame df;
        DataFrame dfFromCsv;
        DataObj dO;
        DataObj dOCsv;
        [OneTimeSetUp]
        public void Setup()
        {
            PrimitiveDataFrameColumn<float> colOne = new PrimitiveDataFrameColumn<float>("colOne", 4);
            StringDataFrameColumn colTwo = new StringDataFrameColumn("colTwo", 4);
            StringDataFrameColumn colThree = new StringDataFrameColumn("colThree", 4);

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

            colOne.Where(val => val < 1f);

            df = new DataFrame(colOne, colTwo, colThree);
            dO = new DataObj(df);

            dfFromCsv = DataFrame.LoadCsv("Assets\\Tests\\TestData.csv");
            dOCsv = new DataObj(dfFromCsv);
        }

        [Test]
        public void TestCalcMin()
        {
            float min = dO.GetMin("colOne");
            Assert.AreEqual(1f, min);
        }

        [Test]
        public void TestCalcMax()
        {
            float max = dO.GetMax("colOne");
            Assert.AreEqual(4f, max);
        }

        [Test]
        public void TestCalcAvg()
        {
            float avg = dO.GetAvg("colOne");
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
            float avg = dOCsv.GetAvg("colOne");
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
    }
}
