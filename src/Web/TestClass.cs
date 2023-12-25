namespace EStore.Web
{
  public interface IGuard;
  public class Guard : IGuard
  {
    public int Id { get; set; } = 1;
    private int Id2 { get; set; } = 2;

    public readonly static IGuard For = new Guard();


  }//eo class

  public static class GuardExt
  {
    public static void Null(this IGuard guard, string str)
    {
      if (str == null) throw new ArgumentNullException("str is null");

    }
  }
}
