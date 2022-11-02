using System;
using GameLibrary.Guns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp
{
    [TestClass]
    public class TestDecoratedGun
    {
        DamageGun damageGun;

        [TestInitialize]
        public void InitalizeGun()
        {
            damageGun = new DamageGun();
        }

        [TestMethod]
        public void TestSlowdownGunDecorator()
        {
            SlowdownGun slowdownGun = new SlowdownGun();
            slowdownGun.SetDecoratedGun(damageGun);

            float expectedUseTime = 20f;
            float actualUseTime = slowdownGun.Ammo;

            Assert.AreEqual(expectedUseTime, actualUseTime);

            float expectedReloadTime = 1f;
            float actualRealodTime = slowdownGun.ReloadTime;

            Assert.AreEqual(expectedReloadTime, actualRealodTime);
        }

        [TestMethod]
        public void TestFrezzeGunDecorator()
        {
            FrezzeGun frezzeGun = new FrezzeGun();
            frezzeGun.SetDecoratedGun(damageGun);

            float expectedUseTime = 15f;
            float actualUseTime = frezzeGun.Ammo;

            Assert.AreEqual(expectedUseTime, actualUseTime);

            float expectedReloadTime = 1.5f;
            float actualRealodTime = frezzeGun.ReloadTime;

            Assert.AreEqual(expectedReloadTime, actualRealodTime);
        }
    }
}
