
// https://www.youtube.com/watch?v=l44Y6lNmNZ0


```c#
using Grade = System.Decimal;

var mads = new Student("Mads To", 988921, new[] { 3.5m, 2.9m, 1.8m});


WriteLine(dustin.GPA);

public class Student(string name, int id, Grad[] grades)
{
    public Student(string name, int id) : this(name, id, Array.Empty<Grade>()) { }

    public int Id => id;

    public string Name {get; set;} = name;

    public Grade GPA => _grade switch
    {
        [] => 4.0m,
        [var grade] => grade,
        [.. var all] => all.Average()
    };
}
```

Refactoring  to new version of C#.

```c#
// collection expresssion

// params for arg overloading.

// 
```