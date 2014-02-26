namespace NDepth.Examples.Database.SQLExamples
{
    partial class Program
    {
        static void Main()
        {
            using (var db = new Database())
            {
                // Call select operators examples.
                SqlSelectExample(db);
                SqlSelectDistinctExample(db);
                SqlSelectWhereExample(db);
                SqlSelectWhereAndOrExample(db);
                SqlSelectWhereIsNullExample(db);
                SqlSelectWhereLikeExample(db);
                SqlSelectWhereInExample(db);
                SqlSelectWhereBetweenExample(db);
                SqlSelectGroupByExample(db);
                SqlSelectOrderByExample(db);
                SqlSelectAliasExample(db);
                SqlSelectTopExample(db);
                SqlSelectCaseExample(db);

                // Call aggregate functions examples.
                SqlAvgExample(db);
                SqlCountExample(db);
                SqlFirstExample(db);
                SqlLastExample(db);
                SqlMaxExample(db);
                SqlMinExample(db);
                SqlSumExample(db);
                SqlIfNullExample(db);

                // Call utility functions examples.
                SqlUpperExample(db);
                SqlLowerExample(db);
                SqlSubstrExample(db);
                SqlLengthExample(db);
                SqlRoundExample(db);
                SqlRandomExample(db);

                // Call date & time functions examples.
                SqlDateTimeNowExample(db);
                SqlDateTimeFormatExample(db);

                // Call join operators examples.
                SqlInnerJoinExample(db);
                SqlLeftJoinExample(db);
                // SqlRightJoinExample(db);
                // SqlFullOuterJoinExample(db);

                // Call select into & union operators examples.
                SqlSelectIntoExample(db);
                SqlUnionExample(db);

                // Call data manipulation operators examples.
                SqlInsertExample(db);
                SqlUpdateExample(db);
                SqlDeleteExample(db);

                // Call data definition operators examples.
                SqlCreateTableExample(db);
                SqlDropTableExample(db);
                SqlAlterTableExample(db);

                // Call data definition extended operators examples.
                SqlConstraintsExample(db);
                SqlForeignKeyExample(db);
                SqlIndexExample(db);
                SqlViewExample(db);
                SqlTriggerExample(db);

                // Call DB operators examples.
                // SqlCreateDbExample(db);
                // SqlDropDbExample(db);
            }
        }
    }
}
