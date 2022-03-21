using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Parser.Utils;

namespace ParserTests
{
    public class BufferedEnumerableTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BehavesAsEnumerable()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var bufferedEnumerable = new BufferedEnumerable<int>(list);

            bufferedEnumerable.Select(i => i).ToList().Should().BeEquivalentTo(list);
        }

        [Test]
        public void CanBeEnumeratedTwice()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var bufferedEnumerable = new BufferedEnumerable<int>(list);

            bufferedEnumerable.Select(i => i).ToList().Should().BeEquivalentTo(list);
            bufferedEnumerable.Select(i => i).ToList().Should().BeEquivalentTo(list);
        }
        [Test]
        public void CanBeSubstringed()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var bufferedEnumerable = new BufferedEnumerable<int>(list);
            var subEnumerable = bufferedEnumerable[1..3];
            subEnumerable.Select(i => i).ToList().Should().BeEquivalentTo(new List<int>() { 2, 3, 4 });
        }
        [Test]
        public void CanBeSubstringedTwice()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var bufferedEnumerable = new BufferedEnumerable<int>(list);
            var subEnumerable1 = bufferedEnumerable[0..2];
            var subEnumerable2 = bufferedEnumerable[2..4];
            subEnumerable1.Select(i => i).ToList().Should().BeEquivalentTo(new List<int>() { 1, 2, 3 });
            subEnumerable2.Select(i => i).ToList().Should().BeEquivalentTo(new List<int>() { 3, 4, 5 });
        }
    }
}