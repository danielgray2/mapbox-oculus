using NUnit.Framework;
using Microsoft.Data.Analysis;
using System.Linq;
using System.Collections.Generic;

namespace Tests
{
    public class VizUtilsTests
    {
        DataFrame df;
        DataObj dO;

        [SetUp]
        public void Setup()
        {
            PrimitiveDataFrameColumn<float> colOne = new PrimitiveDataFrameColumn<float>("colOne", 4);
            StringDataFrameColumn colTwo = new StringDataFrameColumn("colTwo", 4);
            StringDataFrameColumn colThree = new StringDataFrameColumn("colThree", 4);
            StringDataFrameColumn colFour = new StringDataFrameColumn("colFour", 4);
            StringDataFrameColumn colFive = new StringDataFrameColumn("colFive", 4);

            df = new DataFrame(colOne, colTwo, colThree, colFour, colFive);
            dO = new DataObj(df);
        }

        [Test]
        public void TestValidateColNamesHappyPath()
        {
            string fiveName = "col5";
            string sixName = "col6";
            string sevenName = "col7";

            StringDataFrameColumn colFive = new StringDataFrameColumn(fiveName, 4);
            StringDataFrameColumn colSix = new StringDataFrameColumn(sixName, 4);
            StringDataFrameColumn colSeven = new StringDataFrameColumn(sevenName, 4);

            DataFrame newDf = new DataFrame(colFive, colSix, colSeven);
            DataFrame retDf = VizUtils.ValidateColNames(newDf, df);

            List<string> retNames = retDf.Columns.Select(c => c.Name).ToList();

            Assert.AreEqual(fiveName, retNames[0]);
            Assert.AreEqual(sixName, retNames[1]);
            Assert.AreEqual(sevenName, retNames[2]);
        }

        [Test]
        public void TestValidateColNamesCollisions()
        {
            string oneName = "colOne";
            string twoName = "colTwo";
            string threeName = "colThree";

            StringDataFrameColumn colFive = new StringDataFrameColumn(oneName, 4);
            StringDataFrameColumn colSix = new StringDataFrameColumn(twoName, 4);
            StringDataFrameColumn colSeven = new StringDataFrameColumn(threeName, 4);

            DataFrame newDf = new DataFrame(colFive, colSix, colSeven);
            DataFrame retDf = VizUtils.ValidateColNames(newDf, df);

            List<string> retNames = retDf.Columns.Select(c => c.Name).ToList();

            Assert.AreEqual(oneName + "_1", retNames[0]);
            Assert.AreEqual(twoName + "_1", retNames[1]);
            Assert.AreEqual(threeName + "_1", retNames[2]);
        }

        [Test]
        public void TestValidateColNamesSeveralCollisions()
        {
            string oneName = "colOne";
            StringDataFrameColumn colFive = new StringDataFrameColumn(oneName, 4);
            DataFrame firstDf = new DataFrame(colFive);

            string oneNameTwo = "colOne_1";
            string oneNameThree = "colOne_2";
            StringDataFrameColumn colSix = new StringDataFrameColumn(oneName, 4);
            StringDataFrameColumn colSeven = new StringDataFrameColumn(oneNameTwo, 4);
            StringDataFrameColumn colEight = new StringDataFrameColumn(oneNameThree, 4);
            DataFrame secondDf = new DataFrame(colSix, colSeven, colEight);

            DataFrame retDf = VizUtils.ValidateColNames(firstDf, secondDf);

            List<string> retNames = retDf.Columns.Select(c => c.Name).ToList();

            Assert.AreEqual(oneName + "_3", retNames[0]);
        }
    }
}

