using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;

namespace PredicateExtensions.Tests
{
    /* 
     * Data is not needed for these tests, we only need to see if the expression trees are formed correctly
     * Assertions are performed on strings instead of Expression objects 
     * since two Expressions will evaluate to different instances
     */

    [TestFixture]
    public class ExtensionTests
    {
        readonly Expression<Func<string, bool>> equalsA = str => str == "A";
        readonly Expression<Func<string, bool>> equalsC = str => str == "C";
        readonly Expression<Func<string, bool>> containsB = str => str.Contains("B");

        [Test]
        public void Can_Reduce_Predicates_With_PredicateExtensions_Or_Method()
        {
            //arrange
            Expression<Func<string, bool>> expectedOrExpression = str => (str == "A" || str.Contains("B"));
            string expectedExpression = expectedOrExpression.ToString();
            
            //Act
            Expression<Func<string, bool>> orExpression = equalsA.Or(containsB);
            string resultExpression = orExpression.ToString();
            
            //Assert
            resultExpression.Should().Be(expectedExpression);
            LogResults(expectedExpression, resultExpression);
        }
  
        [Test]
        public void Can_Reduce_Predicates_With_PredicateExtensions_And_Method()
        {
            //arrange
            Expression<Func<string, bool>> expectedAndExpression = str => (str == "A" && str.Contains("B"));
            string expectedExpression = expectedAndExpression.ToString();

            //Act
            Expression<Func<string, bool>> andExpression = equalsA.And(containsB);
            string resultExpression = andExpression.ToString();

            //Assert
            resultExpression.Should().Be(expectedExpression);
            LogResults(expectedExpression, resultExpression);

        }
  
        [Test]
        public void Can_Begin_New_Expression()
        {
            //arrange
            var predicate = PredicateExtensions.Begin<string>(true);
            Expression<Func<string, bool>> expectedOrExpression = str => (str == "A" || str.Contains("B"));
            string expectedExpression = expectedOrExpression.ToString();

            //Act
            Expression<Func<string, bool>> orExpression = predicate.Or(equalsA.Or(containsB));
            string resultExpression = orExpression.ToString();

            //Assert
            resultExpression.Should().Be(expectedExpression);
            LogResults(expectedExpression, resultExpression);

        }
  
        [Test]
        public void Can_Reduce_Grouped_Predicates()
        {
            //arrange
            Expression<Func<string, bool>> expectedGroupedPredicate = 
                str => (str == "A" || str.Contains("B")) && (str == "C");

            string expectedExpression = expectedGroupedPredicate.ToString();

            //act
            Expression<Func<string, bool>> groupedPredicate =
                (equalsA.Or(containsB))
                .And(equalsC);

            string resultExpression = groupedPredicate.ToString();

            //assert
            resultExpression.Should().Be(resultExpression);

            LogResults(expectedExpression, resultExpression);

        }
  
        private void LogResults(string expectedExpression, string resultExpression)
        {
            Console.Write(expectedExpression);
            Console.WriteLine();
            Console.Write(resultExpression);
        }
    }
}
