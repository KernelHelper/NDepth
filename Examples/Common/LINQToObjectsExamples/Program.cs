namespace NDepth.Examples.Common.LINQToObjectsExamples
{
    partial class Program
    {
        static void Main()
        {
            var customerStorage = new CustomersStorage();

            // Call restriction operators examples.
            LinqWhereSimple1();
            LinqWhereSimple2(customerStorage);
            LinqWhereSimple3(customerStorage);
            LinqWhereDrilldown(customerStorage);
            LinqWhereIndexed();

            // Call projection operators examples.
            LinqSelectSimple1();
            LinqSelectSimple2(customerStorage);
            LinqSelectTransformation();
            LinqSelectAnonymousTypes1();
            LinqSelectAnonymousTypes2();
            LinqSelectAnonymousTypes3(customerStorage);
            LinqSelectIndexed();
            LinqSelectFiltered();
            LinqSelectManyCompoundFrom1();
            LinqSelectManyCompoundFrom2(customerStorage);
            LinqSelectManyCompoundFrom3(customerStorage);
            LinqSelectManyFromAssignment(customerStorage);
            LinqSelectManyMultipleFrom(customerStorage);
            LinqSelectManyIndexed(customerStorage);

            // Call partitioning operators examples.
            LinqTakeSimple();
            LinqTakeNested(customerStorage);
            LinqSkipSimple();
            LinqSkipNested(customerStorage);
            LinqTakeWhileSimple();
            LinqTakeWhileIndexed();
            LinqSkipWhileSimple();
            LinqSkipWhileIndexed();

            // Call ordering operators examples.
            LinqOrderBySimple1();
            LinqOrderBySimple2();
            LinqOrderBySimple3(customerStorage);
            LinqOrderByComparer();
            LinqOrderByDescendingSimple1();
            LinqOrderByDescendingSimple2(customerStorage);
            LinqOrderByDescendingComparer();
            LinqThenBySimple();
            LinqThenByComparer();
            LinqThenByDescendingSimple(customerStorage);
            LinqThenByDescendingComparer();
            LinqReverse();

            // Call grouping operators examples.
            LinqGroupBySimple1();
            LinqGroupBySimple2();
            LinqGroupBySimple3(customerStorage);
            LinqGroupByNested(customerStorage);
            LinqGroupByComparer();
            LinqGroupByComparerMapped();

            // Call set operators examples.
            LinqDistinct1();
            LinqDistinct2(customerStorage);
            LinqUnion1();
            LinqUnion2(customerStorage);
            LinqIntersect1();
            LinqIntersect2(customerStorage);
            LinqExcept1();
            LinqExcept2(customerStorage);

            // Call conversion operators examples.
            LinqToArray();
            LinqToList();
            LinqToDictionary1();
            LinqToDictionary2();
            LinqToLookup1();
            LinqToLookup2();
            LinqOfType();
            LinqCast();
            LinqAsEnumerable();

            // Call element operators examples.
            LinqFirstSimple(customerStorage);
            LinqFirstCondition();
            LinqFirstOrDefaultSimple();
            LinqFirstOrDefaultCondition(customerStorage);
            LinqLastSimple(customerStorage);
            LinqLastCondition();
            LinqLastOrDefaultSimple();
            LinqLastOrDefaultCondition(customerStorage);
            LinqElementAt();
            LinqElementAtOrDefault();
            LinqSingle();
            LinqSingleOrDefault();

            // Call generation operators examples.
            LinqEmpty();
            LinqRange();
            LinqRepeat();

            // Call quantifiers examples.
            LinqContains();
            LinqAnySimple();
            LinqAnyGrouped(customerStorage);
            LinqAllSimple();
            LinqAllGrouped(customerStorage);

            // Call aggregate operators examples.
            LinqCountSimple();
            LinqCountConditional();
            LinqCountNested(customerStorage);
            LinqCountGrouped(customerStorage);
            LinqLongCountSimple();
            LinqLongCountConditional();
            LinqSumSimple();
            LinqSumProjection();
            LinqSumGrouped(customerStorage);
            LinqMinSimple();
            LinqMinProjection();
            LinqMinGrouped(customerStorage);
            LinqMinElements(customerStorage);
            LinqMaxSimple();
            LinqMaxProjection();
            LinqMaxGrouped(customerStorage);
            LinqMaxElements(customerStorage);
            LinqAverageSimple();
            LinqAverageProjection();
            LinqAverageGrouped(customerStorage);
            LinqAggregateSimple();
            LinqAggregateSeed();

            // Call join operators examples.
            LinqSelectManyJoin();
            LinqCrossJoin();
            LinqInnerJoin();
            LinqLeftOuterJoin();
            LinqRightOuterJoin();
            LinqFullOuterJoin();
            LinqGroupJoin1();
            LinqGroupJoin2();

            // Call miscellaneous operators examples.
            LinqDefaultIfEmpty();
            LinqConcat1();
            LinqConcat2(customerStorage);
            LinqSequenceEqual1();
            LinqSequenceEqual2();
            LinqZip();

            // Call custom operators examples.
            LinqCustomCombine();
            LinqCustomSequenceEquivalent();

            // Call LinqEx operators examples.
            LinqGenerate();
            LinqToEnumerable();
            LinqSingleWrapper();
            LinqDo();
            LinqForEach();
            LinqToStringPretty();
            LinqCombine();
            LinqShuffle();

            // Call query execution examples.
            LinqDeferredExecution();
            LinqImmediateExecution();
            LinqQueryReuse();

            // Parallel LINQ examples.
            PLinqAsParallel(customerStorage);
            PLinqAsSequential(customerStorage);
            PLinqAsEnumerable(customerStorage);
            PLinqAsOrdered(customerStorage);
            PLinqAsUnordered(customerStorage);
            PLinqWithDegreeOfParallelism(customerStorage);
            PLinqWithExecutionMode(customerStorage);
            PLinqWithMergeOptionsNotBuffered();
            PLinqWithMergeOptionsFullBuffered();
            PLinqWithCancellation1();
            PLinqWithCancellation2();
            PLinqAggregate(); 
            PLinqRange();
            PLinqRepeat();
            PLinqEmpty();
            PLinqForAll(customerStorage);
            PLinqExceptions(customerStorage);
        }
    }
}
