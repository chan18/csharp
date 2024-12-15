
https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13


* params collections

```c#
public void Concat<T>(params ReadOnlySpan<T> items)
{
    for (int i = 0; i < items.Length; i++)
    {
        Console.Write(items[i]);
        Console.Write(" ");
    }
    Console.WriteLine();
}
```

* New lock object
* New escape sequence
* Method group natural type

* Implicit index access
```c#
var countdown = new TimerRemaining()
{
    buffer =
    {
        [^1] = 0,
        [^2] = 1,
        [^3] = 2,
        [^4] = 3,
        [^5] = 4,
        [^6] = 5,
        [^7] = 6,
        [^8] = 7,
        [^9] = 8,
        [^10] = 9
    }
};
```

* ref and unsafe in iterators and async methods

* allows ref struct
```c#
public class C<T> where T : allows ref struct
{
    // Use T as a ref struct:
    public void M(scoped T p)
    {
        // The parameter p must follow ref safety rules
    }
}
```

* ref struct interfaces

* More partial members
```c#
public partial class C
{
    // Declaring declaration
    public partial string Name { get; set; }
}

public partial class C
{
    // implementation declaration:
    private string _name;
    public partial string Name
    {
        get => _name;
        set => _name = value;
    }
}
```

* Overload resolution priority

* The field keyword

---

#  c# 12
```c#
using Grade = decimal;

var dustin = new Student(894562, "Dustin Campbell");

/*
    C12 collection expresssions.
    It will convert into any collection types.
    compiler will build ( i have to look into IS)
    * List<Grade>
    * params Grade[] grades
    * IEnumerable<Grade> grades
    * ReadOnlySpan<Grade> grades - high performance struct.

*/
dustin.AddGrades([4.0m,3.8m,3.9m]);

WriteLine(dustin.GPA);

public class Student(int id, string name)
{
    List<Grade> _grades = [];

    public void AddGrades(params Grade[] grades) => _grades.AddRange(grades);
    public void AddGrades(IEnumerable<Grade> grades) => _grades.AddRange(grades);
    public void AddGrades(ReadOnlySpan<Grade> grades) => _grades.AddRange(grades);

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

before i start refactoring i use ispy and decompiled and looked into the generated code, The collection expression generated. 

# collection expresssions.
Params
```c#
// from
var dustin = new Student(894562, "Dustin Campbell").AddGrades([4.0m,3.8m,3.9m]);

// to compiler generated.
new Student(894562, "Dustin Campbell").AddGrades(4.0m, 3.8m, 3.9m);
```

IEnumerable
```c#
// from
var dustin = new Student(894562, "Dustin Campbell").AddGrades([4.0m,3.8m,3.9m]);

// to compiler generated.
new Student(894562, "Dustin Campbell").AddGrades(new <>z__ReadOnlyArray<decimal>(new decimal[3] { 4.0m, 3.8m, 3.9m }));
```

ReadOnlySpan
```c#
// from
var dustin = new Student(894562, "Dustin Campbell").AddGrades([4.0m,3.8m,3.9m]);

// to compiler generated.
Student student = new Student(894562, "Dustin Campbell");
global::<>y__InlineArray3<decimal> buffer = default(global::<>y__InlineArray3<decimal>);
global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray3<decimal>, decimal>(ref buffer, 0) = 4.0m;
global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray3<decimal>, decimal>(ref buffer, 1) = 3.8m;
global::<PrivateImplementationDetails>.InlineArrayElementRef<global::<>y__InlineArray3<decimal>, decimal>(ref buffer, 2) = 3.9m;
student.AddGrades(global::<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<global::<>y__InlineArray3<decimal>, decimal>(in buffer, 3));

```

--

# Refactoring into C#13

```c#
using Grade = decimal;

var dustin = new Student(894562, "Dustin Campbell");

dustin.AddGrades([4.0m,3.8m,3.9m]);

WriteLine(dustin.GPA);

public class Student(int id, string name)
{
    List<Grade> _grades = [];

    public void AddGrades(paramsGrade[] grades) => _grades.AddRange(grades);

    // c13 feature as hidden cost.
    public void AddGrades(params IEnumerable<Grade> grades) => _grades.AddRange(grades);
    // c13 feature for perforamnce benifits, readonly spams params overload.
    public void AddGrades(params eadOnlySpan<Grade> grades) => _grades.AddRange(grades);

    public int Id => id;

    // c13 feature field for auto generatd properites.
    // @field for backwards comability. for braking changes.
    public string Name { get => field ; set = field => Value.Trim(); } = name;
    
    public Grade GPA => _grade switch
    {
        [] => 4.0m,
        [var grade] => grade,
        [.. var all] => all.Average()
    };
}

```