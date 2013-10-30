using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PredicateExtensions.Tests
{
    [TestFixture]
    //Data is not needed for these tests, we only need to see if the expression trees are formed correctly
    public class ExtensionTests
    {

        Expression<Func<string, bool>> EqualsA = str => str == "A";
        Expression<Func<string, bool>> EqualsC = str => str == "C";
        Expression<Func<string, bool>> ContainsB = str => str.Contains("B");

        [Test]
        public void Can_Reduce_Predicates_With_PredicateExtensions_Or_Method()
        {
            //arrange
            Expression<Func<string, bool>> expectedEither = str => (str == "A" || str.Contains("B"));

            //Act
            Expression<Func<string, bool>> withEither = EqualsA.Or(ContainsB);

            //Assert
            Assert.AreEqual(expectedEither.ToString(), withEither.ToString());
            Console.Write(expectedEither.ToString());
            Console.WriteLine();
            Console.Write(withEither.ToString());
        }
  

        [Test]
        public void Can_Reduce_Predicates_With_PredicateExtensions_And_Method()
        {
            //arrange
            Expression<Func<string, bool>> expectedBoth = str => (str == "A" && str.Contains("B"));

            //Act
            Expression<Func<string, bool>> withEither = EqualsA.And(ContainsB);

            //Assert
            Assert.AreEqual(expectedBoth.ToString(), withEither.ToString());
            Console.Write(expectedBoth.ToString());
            Console.WriteLine();
            Console.Write(withEither.ToString());
        }

        [Test]
        public void Can_Begin_New_Expression()
        {
            //arrange
            var predicate = PredicateExtensions.Begin<string>(true);
            Expression<Func<string, bool>> expectedEither = str => (str == "A" || str.Contains("B"));

            //Act
            Expression<Func<string, bool>> withEither = predicate.Or(EqualsA.Or(ContainsB));

            //Assert
            Assert.AreEqual(expectedEither.ToString(), withEither.ToString());
            Console.Write(expectedEither.ToString());
            Console.WriteLine();
            Console.Write(withEither.ToString());
        }

        [Test]
        public void Can_Reduce_Grouped_Predicates()
        {
            Expression<Func<string, bool>> expectedGroupedPredicate = 
                str => (str == "A" || str.Contains("B")) && (str == "C");

            Expression<Func<string, bool>> groupedPredicate =
                (EqualsA.Or(ContainsB))
                .And(EqualsC);

            Assert.AreEqual(
                   expectedGroupedPredicate.ToString(),
                   groupedPredicate.ToString()
                );

            Console.Write(expectedGroupedPredicate.ToString());
            Console.WriteLine();
            Console.Write(groupedPredicate.ToString());
        }
    }
}
