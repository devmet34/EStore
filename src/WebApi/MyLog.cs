namespace EStore.WebApi;

public class MyLog:IMyLog
{
  private string _id;

  public MyLog(string id) 
  {  
    _id = id; 
  }
}
