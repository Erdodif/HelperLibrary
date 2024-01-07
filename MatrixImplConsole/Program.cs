using Matrix;

Matrix2D<int> numbers;

numbers = new Matrix2D<int>(10, 15);

for (int i = 0; i < numbers.Area; i++)
{
    numbers[i] = i;
}

Console.WriteLine(numbers.ToString());
Console.WriteLine(numbers.ToString(false, true));

Console.WriteLine($"(y:9,x:9) = {numbers[new Position { x = 9, y = 9 }]}");
Console.WriteLine($"(9,9) = {numbers[9, 9]}");
Console.WriteLine($"99 = {numbers[99]}");

for (int i = 0; i < numbers.Area; i++)
{
    Console.WriteLine(numbers[numbers.IndexFromPosition(numbers.PositionFromIndex(i))]);
}