#PredicateExtensions

PredicateExtensions is a C# utility that will take two Lambda expressions and combine them using .And or .Or extension methods. Expressions be joined at runtime for dynamic LINQ queries. PredicateExtensions can be used with EntityFramework to refactor and create dynamic queries.

##Example
    Expression<Func<string, bool>> equalsA = str => str == "A";
    Expression<Func<string, bool>> equalsB = str => str == "B";
	
	IQueryable<string> myValues = {"A", "B", "C", "D" };
    myValues.Where(equalsA.Or(equalsB)); //"A", "B"

