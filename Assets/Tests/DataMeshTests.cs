using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using Microsoft.Data.Analysis;

namespace Tests
{
    public class DataMeshTests
    {
        DataFrame df;
        DataObj dO;
        [SetUp]
        public void Setup()
        {
            PrimitiveDataFrameColumn<float> colOne = new PrimitiveDataFrameColumn<float>("colOne", 5);
            PrimitiveDataFrameColumn<float> colTwo = new PrimitiveDataFrameColumn<float>("colTwo", 5);
            PrimitiveDataFrameColumn<float> colThree = new PrimitiveDataFrameColumn<float>("colThree", 5);
            StringDataFrameColumn colFour = new StringDataFrameColumn("colFour", 5);

            colOne[0] = 1;
            colOne[1] = 1;
            colOne[2] = 2;
            colOne[3] = 2;
            colOne[4] = 3;

            colTwo[0] = 1;
            colTwo[1] = 1;
            colTwo[2] = 1;
            colTwo[3] = 1;
            colTwo[4] = 2;

            colThree[0] = 1;
            colThree[1] = 1;
            colThree[2] = 1;
            colThree[3] = 2;
            colThree[4] = 2;

            colFour[0] = "b";
            colFour[1] = "b";
            colFour[2] = "b";
            colFour[3] = "b";
            colFour[4] = "b";

            df = new DataFrame(colOne, colTwo, colThree, colFour);
            dO = new DataObj(df);
        }

        // TODO: Make another test like this one, but with
        // unordered data
        [Test]
        public void TestFindColOrdering()
        {
            GameObject gO = new GameObject();
            DataMesh dM = gO.AddComponent<DataMesh>();
            List<string> colOrdering = dM.FindColOrdering(dO, "colOne", "colTwo", "colThree");

            List<string> correctOrdering = new List<string> { "colTwo", "colThree", "colOne" };
            Assert.AreEqual(correctOrdering, colOrdering);
        }
    }
}
