using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tests
{
    public class RandoTesting
    {
        [Test]
        public void TestInheritance()
        {
            ScatterModel currModel = new ScatterModel();
            List<Type> compatSubComps = new List<Type>() { typeof(IAbsGraphModel), typeof(IAbsMeshModel) };
            List<Type> absModelList = compatSubComps.Where(m => m.IsAssignableFrom(currModel.GetType())).ToList();
            Assert.AreEqual(1, absModelList.Count());
        }
    }
}

