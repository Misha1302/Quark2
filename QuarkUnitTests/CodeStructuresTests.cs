using QuarkExtension;
using QuarkStructures;

namespace UnitTests;

public class CodeStructuresTests : CodeTesterBase
{
    private double _error;
    private List<IQuarkExtension> _extensions;
    private string _imports;

    [SetUp]
    public void Setup()
    {
        _error = 1e-6;
        _imports =
            """
            import "../../../../Libraries"
            """;
        _extensions = [new QuarkExtStructures()];
    }

    [Test]
    public void Test1()
    {
        TestCode(
            $$"""
              {{_imports}}

              struct Vector3(x, y, z)

              def Main() {
                  v = CreateStruct("Vector3")
                  v->x = 0
                  return v->x
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(0).Within(_error)),
            _extensions
        );
    }

    [Test]
    public void Test2()
    {
        TestCode(
            $$"""
              {{_imports}}

              struct Vector3(x, y, z)

              def Main() {
                  v = CreateStruct("Vector3")
                  v->x = 3
                  v->y = 4
                  v->z = v->x / v->y
                  return v->x * v->y * v->z
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(3.0 * 4 * (3.0 / 4.0)).Within(_error)),
            _extensions
        );
    }

    [Test]
    public void Test3()
    {
        TestCode(
            $$"""
              {{_imports}}

              struct Vector3(x, y, z)

              def Main() {
                  v = CreateStruct("Vector3")
                  v2 = CreateStruct("Vector3")
                  v->x = 3
                  v->y = 0
                  v2->y = 4
                  v2 -> x = 0
                  return v->x + v->y + v2->x + v2->y
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(3 + 4).Within(_error)),
            _extensions
        );
    }

    [Test]
    public void Test4()
    {
        TestCode(
            $$"""
              {{_imports}}

              struct Vector3(x, y, z)

              def Main() {
                  v = CreateStruct("Vector3")
                  v2 = CreateStruct("Vector3")
                  v->x = v2
                  v->x->y = 5
                  v->y = 3
                  return v->x->y * v->y
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(5 * 3).Within(_error)),
            _extensions
        );
    }

    [Test]
    public void Test5()
    {
        TestCode(
            $$"""
              {{_imports}}

              struct Vector3(x, y, z)

              def Main() {
                  v = NewVector3(1.1, 2, 3)
                  return v->x * v->y * v->z
              }

              def NewVector3(x, y, z) {
                  v = CreateStruct("Vector3")
                  v->x = x
                  v->y = y
                  v->z = z
                  return v
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.1 * 2 * 3).Within(_error)),
            _extensions
        );
    }

    [Test]
    public void Test6()
    {
        TestCode(
            $$"""
              {{_imports}}

              struct Vector3(x, y, z)

              def Main() {
                  v = NewVector3(1.1, 2, 3)
                  Foo(v)->x = 5
                  return v->x * v->y * v->z
              }

              def Foo(a) {
                  return a
              }

              def NewVector3(x, y, z) {
                  v = CreateStruct("Vector3")
                  v->x = x
                  v->y = y
                  v->z = z
                  return v
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(5 * 2 * 3).Within(_error)),
            _extensions
        );
    }
}