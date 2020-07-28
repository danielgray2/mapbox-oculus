using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.Data.Analysis;
using System;

namespace Tests
{
    public class FilterParserTests
    {
        FilterObj equal;
        FilterObj greaterThanEqualString;
        FilterObj lessThanEqualString;
        FilterObj lessThanEqualDate;
        DataObj dO;

        [SetUp]
        public void Setup()
        {
            DateTime date = DateTime.Parse("6/20/2001");
            equal = new FilterObj(FilterObj.FilterType.E, 2f, "colOne");
            greaterThanEqualString = new FilterObj(FilterObj.FilterType.GTE, "c", "colTwo");
            lessThanEqualString = new FilterObj(FilterObj.FilterType.LTE, "c", "colTwo");
            lessThanEqualDate = new FilterObj(FilterObj.FilterType.LTE, date, "colThree");
            
            PrimitiveDataFrameColumn<float> colOne = new PrimitiveDataFrameColumn<float>("colOne", 5);
            StringDataFrameColumn colTwo = new StringDataFrameColumn("colTwo", 5);
            PrimitiveDataFrameColumn<DateTime> colThree = new PrimitiveDataFrameColumn<DateTime>("colThree", 5);

            colOne[0] = 1;
            colOne[1] = 1;
            colOne[2] = 2;
            colOne[3] = 2;
            colOne[4] = 3;

            colTwo[0] = "a";
            colTwo[1] = "b";
            colTwo[2] = "c";
            colTwo[3] = "d";
            colTwo[4] = "e";

            colThree[0] = DateTime.Parse("7/25/2001");
            colThree[1] = DateTime.Parse("7/22/2001");
            colThree[2] = DateTime.Parse("5/25/2001");
            colThree[3] = DateTime.Parse("6/25/2001");
            colThree[4] = DateTime.Parse("8/25/2001");

            dO = new DataObj(new DataFrame(colOne, colTwo, colThree));
        }

        [Test]
        public void TestEqual()
        {
            DataObj ret = FilterParser.ParseAnd(dO, new List<FilterObj>() { equal });
            DataFrame retDf = ret.df;

            Assert.AreEqual(2, retDf.Rows.Count);
            Assert.AreEqual(dO.df.Rows[2], ret.df.Rows[0]);
            Assert.AreEqual(dO.df.Rows[3], ret.df.Rows[1]);
        }

        [Test]
        public void TestGreaterThanEqual()
        {
            DataObj ret = FilterParser.ParseAnd(dO, new List<FilterObj>() { greaterThanEqualString });
            DataFrame retDf = ret.df;

            Assert.AreEqual(3, retDf.Rows.Count);
            Assert.AreEqual(dO.df.Rows[2], ret.df.Rows[0]);
            Assert.AreEqual(dO.df.Rows[3], ret.df.Rows[1]);
            Assert.AreEqual(dO.df.Rows[4], ret.df.Rows[2]);
        }

        [Test]
        public void TestLessThanEqual()
        {
            DataObj ret = FilterParser.ParseAnd(dO, new List<FilterObj>() { lessThanEqualDate });
            DataFrame retDf = ret.df;

            Assert.AreEqual(1, retDf.Rows.Count);
            Assert.AreEqual(dO.df.Rows[2], ret.df.Rows[0]);
        }

        [Test]
        public void MultipleFilters()
        {
            DataObj ret = FilterParser.ParseAnd(dO, new List<FilterObj>() { greaterThanEqualString, lessThanEqualString });
            DataFrame retDf = ret.df;

            Assert.AreEqual(1, retDf.Rows.Count);
            Assert.AreEqual(dO.df.Rows[2], ret.df.Rows[0]);
        }
    }
}
