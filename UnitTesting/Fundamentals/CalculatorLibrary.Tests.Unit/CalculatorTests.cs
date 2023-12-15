using Xunit.Abstractions;

namespace CalculatorLibrary.Tests.Unit;

public class CalculatorTests : IDisposable
{
    private readonly Calculator _sut = new();
    private readonly ITestOutputHelper _testOutputHelper;

    public CalculatorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        //testten önce yapılmasını istediğimiz kodlar
        _testOutputHelper.WriteLine("before test");
    }
    

    
    [Fact]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers()
    {
        //arrange - classların ya da servislerin new lendiği. eger varsa degerlerinin set edildigi kısım. - bazen bu kısım olmaz belki ctor da cagırılır
        //var calculator = new Calculator();

        //act - metotların çağırıldığı ve çalıştırıldığı ve sonuçların yakalandığı kısım
        var result = _sut.Add(5, 4);
        
        _testOutputHelper.WriteLine("testing add method");
        
        //assert - sonucun kontrol edildiği ve sonucun ne olması gerektiğinin uygulamaya söylendiği kısımdır.
        Assert.Equal(9, result);
    }
    
    //farklı parametrelerle test yapmak için
    [Theory]
    [InlineData(5,5,10)]
    [InlineData(0,0,0)]
    [InlineData(-5,-1,-6)]
    [InlineData(0,1,1)]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers2(int a, int b, int expected)
    {
        //arrange - classların ya da servislerin new lendiği. eger varsa degerlerinin set edildigi kısım. - bazen bu kısım olmaz belki ctor da cagırılır
        //var calculator = new Calculator();

        //act - metotların çağırıldığı ve çalıştırıldığı ve sonuçların yakalandığı kısım
        var result = _sut.Add(a, b);
        
        _testOutputHelper.WriteLine("testing add method");
        
        //assert - sonucun kontrol edildiği ve sonucun ne olması gerektiğinin uygulamaya söylendiği kısımdır.
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers()
    {
        //act
        var result = _sut.Subtract(5, 4);
        
        _testOutputHelper.WriteLine("testing Subtract method");
        
        //assert 
        Assert.Equal(1, result);
    }

    public void Dispose()
    {
        //işlem tamamen bittikten sonra class yok olurken
        _testOutputHelper.WriteLine("after everything");
    }
}