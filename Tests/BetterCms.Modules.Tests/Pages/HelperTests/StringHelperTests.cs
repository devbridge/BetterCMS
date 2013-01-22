using BetterCms.Module.Root.Mvc.Helpers;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.HelperTests
{
    [TestFixture]
    public class StringHelperTests
    {
        private const string specialSymbolsBefore = "0`1~2!3@4#5$6%7^8&9*10(11)12_13+14-15=16;17'18\\19[20]21{22}23:24\"25|26<27>28?29,30.31/32;33";
        private const string specialSymbolsAfter = "012-3-456789101112-13-14-1516-1718-1920212223-2425262728-29-30-31-32-33";

        private const string russianStringBefore = "Start. необходимо преобразовать специальные символы. End.";
        private const string russianStringAfterConverted = "start-neobxodymo-preobrazovatj-specyaljne-symvol-end";
        private const string russianStringAfterNoConvertion = "start-необходимо-преобразовать-специальные-символы-end";

        private const string japaneseStringBefore = "Start. 特殊記号を変換する必要があります. End.";
        private const string japaneseStringAfter = "start-end";

        [Test]
        public void TestTransliterate_ShouldRemoveSpecialSymbols()
        {
            var result = specialSymbolsBefore.Transliterate(true);
            Assert.AreEqual(result, specialSymbolsAfter);
        }

        [Test]
        public void TestTransliterate_ShouldKeepRussianSymbols()
        {
            var result = russianStringBefore.Transliterate(true);
            Assert.AreEqual(result, russianStringAfterNoConvertion);
        }

        [Test]
        public void TestTransliterate_ShouldConvertRussianSymbols()
        {
            var result = russianStringBefore.Transliterate();
            Assert.AreEqual(result, russianStringAfterConverted);
        }

        [Test]
        public void TestTransliterate_ShouldRemoveJapaneseSymbols()
        {
            var result = japaneseStringBefore.Transliterate();
            Assert.AreEqual(result, japaneseStringAfter);
        }
    }
}
