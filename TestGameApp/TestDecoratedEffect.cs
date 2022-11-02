using System;
using GameLibrary.Effects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGameApp
{
    [TestClass]
    public class TestDecoratedEffect
    {
        DamageEffect damageEffect;

        [TestInitialize]
        public void InitalizeGun()
        {
            damageEffect = new DamageEffect();
        }

        [TestMethod]
        public void TestSlowdownEffectDecorator()
        {
            SlowdownEffect slowdownEffect = new SlowdownEffect();
            slowdownEffect.SetDecoratedEffect(damageEffect);

            float expectedSpeed = 10f;
            float actualSpeed = slowdownEffect.EffectTime;

            Assert.AreEqual(expectedSpeed, actualSpeed);
        }

        [TestMethod]
        public void TestFrezzeEffectDecorator()
        {
            FrezzeEffect frezzeEffect = new FrezzeEffect();
            frezzeEffect.SetDecoratedEffect(damageEffect);

            float expectedSpeed = 7f;
            float actualSpeed = frezzeEffect.EffectTime;

            Assert.AreEqual(expectedSpeed, actualSpeed);
        }
    }
}
