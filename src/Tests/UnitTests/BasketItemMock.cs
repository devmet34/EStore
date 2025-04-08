namespace UnitTests;
public class BasketItemMock
{
  public int Id { get; set; }
  public int BasketId { get; set; }

  public BasketItemMock(int basketId)
  {
    Id = Helper.GetRandomInt(1, 99999);
    BasketId = basketId;
  }



}
