

var result = AddAll(new[] { });

WriteLine(result);

int Add(int[] values)
{
    foreach (int value in values)
    {
        result += value;
    }
    return result;
}


