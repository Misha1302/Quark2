namespace QuarkBenchmarks;

public class DfsTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        names = CreateVector("A", "B", "F", "C", "E", "G", "H", "I", "D", 9)
        vec = CreateVector(CreateVector(1,2, 2), CreateVector(3,4, 2), CreateVector(5, 1), CreateVector(8, 1), CreateVector(0), CreateVector(6,7, 2), CreateVector(0), CreateVector(0), CreateVector(0), 9)
        _ = Dfs(names, vec, 0)
        return 0
    }

    def Dfs(names, vec, ind) {
        // _ = Print(GetValue(names, ind))
        // _ = Print(" ")
        for (i = 0) (i < GetSize(GetValue(vec, ind))) (i = i + 1) {
            _ = Dfs(names, vec, GetValue(GetValue(vec, ind), i))
        }
        return 0
    }
    """
);