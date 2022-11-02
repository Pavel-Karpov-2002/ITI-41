using System;
using GameLibrary.Bullets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp
{
    [TestClass]
    public class TestDecoratedBullet
    {
        DamageBullet damageBullet;

        [TestInitialize]
        public void InitalizeBullet()
        {
            damageBullet = new DamageBullet();
        }

        [TestMethod]
        public void TestSlowdownBulletDecorator()
        {
            SlowdownBullet slowdownBullet = new SlowdownBullet();
            slowdownBullet.SetDecoratedBullet(damageBullet);

            float expectedSpeed = 20f;
            float actualSpeed = slowdownBullet.Speed;

            Assert.AreEqual(expectedSpeed, actualSpeed);
        }

        [TestMethod]
        public void TestFrezzeBulletDecorator()
        {
            FrezzeBullet frezzeBullet = new FrezzeBullet();
            frezzeBullet.SetDecoratedBullet(damageBullet);

            float expectedSpeed = 15f;
            float actualSpeed = frezzeBullet.Speed;

            Assert.AreEqual(expectedSpeed, actualSpeed);
        }
    }
}
