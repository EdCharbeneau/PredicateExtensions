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
    public class EfExtensionTests
    {
        private BloggingContext dbContext;

        [TestFixtureSetUp]
        public void SetUp()
        {
            dbContext = new BloggingContext();
            //InitDatabase();
        }

        [Test]
        public void Can_Query_Using_Predicates_Built_With_Or_Extension()
        {
            //Act
            var results = dbContext.Posts.Where
                    (
                    HasTitle("First Post")
                    .Or(ContentContains("keyword"))
                    );

            Console.WriteLine(results);

            //Assert
            Assert.AreEqual(2, results.Count());
            foreach (var item in results)
            {
                Console.WriteLine("Title: {0}", item.Title);
                Console.WriteLine("Content: {0}", item.Content);
                Console.WriteLine("-------");
            }
        }

        [Test]
        public void Can_Query_Using_Predicates_Built_With_And_Extension()
        {
            //Act
            var results = dbContext.Posts.Where
                    (
                    HasTitle("First Post")
                    .And(ContentContains("Lorem"))
                    );

            Console.WriteLine(results);

            //Assert
            Assert.AreEqual(1, results.Count());
            foreach (var item in results)
            {
                Console.WriteLine("Title: {0}", item.Title);
                Console.WriteLine("Content: {0}", item.Content);
                Console.WriteLine("-------");
            }
        }

        [Test]
        public void Can_Query_Using_Dynamic_Predicates()
        {
            //arrange
            IEnumerable<string> keywords = new List<string>{ "First", "Third", "Fourth" };

            //act
            //Dynimaically build 
            //post => post.Title == "First" || post.Title == "Third" || post.Title == "Fourth"
            var results = dbContext.Posts.Where(
                        TitleContains(keywords));

            //assert
            Console.WriteLine(results);
            Assert.AreEqual(3, results.Count());
            
        }

        [Test]
        public void Given_Empty_Parameters_Dynamic_Predicate_Returns_Empty_Results()
        {
            //arrange
            IEnumerable<string> keywords = new List<string>();

            //act
            //Dynimaically build 
            //post => post.Title == "First" || post.Title == "Third" || post.Title == "Fourth"
            var results = dbContext.Posts.Where(
                        TitleContains(keywords));

            //assert
            Console.WriteLine(results);
            Assert.AreEqual(0, results.Count());

        }

        private Expression<Func<Post, bool>> TitleContains(IEnumerable<string> keywords)
        {
            var predicate = PredicateExtensions.Begin<Post>();

            foreach (var keyword in keywords)
            {
                predicate = predicate.Or(TitleContains(keyword));
            }
            return predicate;
        }

        private Expression<Func<Post, bool>> TitleContains(string keyword)
        {
            return post => post.Title.Contains(keyword);
        }

        private Expression<Func<Post, bool>> HasTitle(string title)
        {
            return post => post.Title == title;
        }

        private Expression<Func<Post, bool>> ContentContains(string keyword)
        {
            return post => post.Content.Contains(keyword);
        }

        private void InitDatabase()
        {
            //Initialize Data
            //Add Blog
            dbContext.Blogs.Add(new Blog { Name = "First Blog" });
            dbContext.SaveChanges();

            var blog = dbContext.Blogs.First();

            //Add posts
            blog.Posts.Add(new Post { Title = "First Post", Content = "Lorem Ipsum" });
            blog.Posts.Add(new Post { Title = "Second Post", Content = "Lorem Ipsum" });
            blog.Posts.Add(new Post { Title = "Third Post", Content = "keyword" });
            blog.Posts.Add(new Post { Title = "Fourth Post", Content = "Lorem Ipsum" });

            dbContext.SaveChanges();
        }

        //[Test]
        //public void Db_Has_Data()
        //{
        //    Assert.Greater(dbContext.Posts.Count(), 0);
        //}

    }
}
