namespace UnitTests;

public class CodeTests
{
    private double _error;
    private string _imports;
    private double _smallError;

    [SetUp]
    public void Setup()
    {
        _error = 1e-6;
        _smallError = 1e-9;
        _imports =
            """
            import "../../../../Libraries"
            """;
    }

    [Test]
    public void Test1()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return 5.123
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(5.123).Within(_error))
        );
    }

    [Test]
    public void Test2()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return (2 + 3) / 4
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo((2 + 3) / 4.0).Within(_error))
        );
    }

    [Test]
    public void Test3()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  sum = 0
                  for (i = -2) (i <= 5) (i = i + 1) {
                      sum = sum + i
                  }
                  return sum
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(12.0).Within(_error))
        );
    }

    [Test]
    public void Test4()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  n = 6721278541
                  return IsPrime(n)
              }

              def IsPrime(n) {
                  top = n ** (1 / 2) + 1
                  for (i = 2) (i <= top) (i = i + 1) {
                      if (n % i == 0) {
                          return 0
                      }
                  } 
                  
                  return 1
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.0).Within(_error))
        );
    }

    [Test]
    public void Test5()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  n = 83521
                  return IsPrime(n)
              }

              def IsPrime(n) {
                  top = n ** (1 / 2) + 1
                  for (i = 2) (i <= top) (i = i + 1) {
                      if (n % i == 0) {
                          return 0
                      }
                  } 
                  
                  return 1
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(0.0).Within(_error))
        );
    }

    [Test]
    public void Test6()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return Fact(10)
              }

              def Fact(i) {
                  if i <= 1 { return i }
                  return Fact(i - 1) * i
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(3628800.0).Within(_error))
        );
    }

    [Test]
    public void Test7()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  vec = CreateVector(5,6,7,8, 4)
                  return GetValue(vec, 2)
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(7.0).Within(_error))
        );
    }

    [Test]
    public void Test8()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return ToNumber("3.21")
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(3.21).Within(_error))
        );
    }

    [Test]
    public void Test9()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return 1 / 2 / 3 / 4 / 5 / 6
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.0 / 2 / 3 / 4 / 5 / 6).Within(_smallError))
        );
    }

    [Test]
    public void TestA1()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return Divs(1, 2, 3, 4, 5, 6)
              }

              def Divs(a, b, c, d, e, f) {
                  return a / b / c / d / e / f
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.0 / 2 / 3 / 4 / 5 / 6).Within(_smallError))
        );
    }

    [Test]
    public void TestA2()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  names = CreateVector("A", "B", "F", "C", "E", "G", "H", "I", "D", 9)
                  vec = CreateVector(CreateVector(1,2, 2), CreateVector(3,4, 2), CreateVector(5, 1), CreateVector(8, 1), CreateVector(0), CreateVector(6,7, 2), CreateVector(0), CreateVector(0), CreateVector(0), 9)
                  return Dfs(names, vec, 0)
              }

              def Dfs(names, vec, ind) {
                  s = GetValue(names, ind)
                  for (i = 0) (i < GetSize(GetValue(vec, ind))) (i = i + 1) {
                      s = Concat(s, Dfs(names, vec, GetValue(GetValue(vec, ind), i)))
                  }
                  return s
              }
              """,
            any => Assert.That(any.Get<string>(), Is.EqualTo("ABCDEFGHI"))
        );
    }

    [Test]
    public void TestA3()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return not (0)
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.0).Within(_error))
        );
    }

    [Test]
    public void TestA4()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return not (5 < 3)
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.0).Within(_error))
        );
    }

    [Test]
    public void TestA5()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return not ((5 - 9) < 3)
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(0.0).Within(_error))
        );
    }

    [Test]
    public void TestA7()
    {
        TestCode(
            $$"""
              {{_imports}}

              def Main() {
                  return not not 1
              }
              """,
            any => Assert.That(any.Get<double>(), Is.EqualTo(1.0).Within(_error))
        );
    }

    private void TestCode(string code, Action<Any> result)
    {
        var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
        var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
        var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);

        var msilExecutor = new TranslatorToMsil.TranslatorToMsil();
        msilExecutor.Init(new ExecutorConfiguration(module));

        var interpreter = new QuarkVirtualMachine();
        interpreter.Init(new ExecutorConfiguration(module));

        var resultTranslator = msilExecutor.RunModule().First();
        var resultInterpreter = interpreter.RunModule().First();

        result(resultTranslator);
        result(resultInterpreter);
    }
}