using CodeGator.EntityFrameworkCore;

namespace CodeGator.EntityFrameworkCore.UnitTests;

[TestClass]
public sealed class ConstantsTests
{
    [TestMethod]
    public void ActorMaxLength_has_expected_value()
    {
        // Regression guard: actor strings store canonical GUID length (36 chars).
#pragma warning disable MSTEST0032
        Assert.AreEqual(36, Constants.ColumnLengths.ActorMaxLength);
#pragma warning restore MSTEST0032
    }
}
