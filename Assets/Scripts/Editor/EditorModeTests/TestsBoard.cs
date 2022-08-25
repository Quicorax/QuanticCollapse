using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestsBoard
{
    [Test]
    public void TestsBoardSimplePasses()
    {
    }

    [UnityTest]
    public IEnumerator TestsBoardWithEnumeratorPasses()
    {
        yield return null;
    }
}
