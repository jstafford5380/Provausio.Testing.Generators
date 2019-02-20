﻿using System;
using System.Linq;
using Provausio.Testing.Generators.Generators.Strings;
using Xunit;

namespace Provausio.Testing.Generators.Tests.Strings
{
    public class ParagraphTests
    {
        [Fact]
        public void GenerateParagraph_GeneratesUniqueParagraph()
        {
            // arrange
            var paraGen = new ParagraphGenerator();

            // act
            var paragraph1 = paraGen.GenerateParagraph(5, 10);
            var paragraph2 = paraGen.GenerateParagraph(5, 10);

            // assert
            Assert.True(paragraph1.Length > 0);
            Assert.True(paragraph2.Length > 0);
            Assert.NotEqual(paragraph1, paragraph2);
        }

        [Fact]
        public void GenerateParagraphs_ContinuesToGenerate_UniqueParagraphs()
        {
            // arrange
            var generator = new ParagraphGenerator();

            // act
            var paragraphs = generator.GenerateParagraphs(5, 10).Take(5);

            // assert
            Assert.Equal(
                paragraphs.Count(), 
                paragraphs.Distinct().Count());

            var x = string.Join("\r\n\r\n", paragraphs);
        }
    }
}
